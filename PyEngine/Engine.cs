namespace PyEngine;

using MessagePack;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;

public class Engine: IDisposable {
	public static Engine? Default { get; set; } = null;

	private static int _nextInstanceId = 0;

	private readonly int    _instanceId;
	private readonly string _pipeName;

	private readonly bool _usesScript = false;

	private readonly string? _executablePath = null;
	private readonly string? _pythonPath     = null;
	private readonly string? _scriptPath     = null;

	private readonly NamedPipeServerStream _pipeServer;
	private BinaryWriter _pipeStreamWriter;
	private BinaryReader _pipeStreamReader;

	private readonly Dictionary<string, MethodBinding> _boundFuncs = new();

	private Process? _subProcess = null;

	private bool _disposedValue;

	public Engine(string executablePath = ""): this(executablePath, null, null) {
		_usesScript = false;
	}

	public Engine(string pythonPath, string scriptPath): this(null, pythonPath, scriptPath) {
		_usesScript = true;
	}

	private Engine(string? executablePath, string? pythonPath, string? scriptPath) {
		Default = this;

		if (executablePath == "") {
			var os = Util.GetOS();
			if (os == OSPlatform.Windows) {
				executablePath = "pyengine.exe";
			} else /*if (os == OSPlatform.Linux)*/ {
				executablePath = "pyengine";
			}
		}

		_executablePath = executablePath;
		_pythonPath     = pythonPath;
		_scriptPath     = scriptPath;

		_instanceId = _nextInstanceId;
		_pipeName   = $"PyEngine___{Process.GetCurrentProcess().Id.AsHex()}_{_instanceId.AsHex()}";

		_pipeServer = new NamedPipeServerStream(_pipeName);

		_nextInstanceId++;
	}
	
	internal bool PyPrimitivesInit = false;
	internal PyObject Len;
	internal PyObject Str;

	public void Start() {
		Default = this;

		if (_usesScript) {
			_subProcess = new Process() {
				StartInfo = new() {
					FileName        = _pythonPath,
					Arguments       = $"\"{Path.Join(AppContext.BaseDirectory, _scriptPath)}\" \"{_pipeName}\"",
					UseShellExecute = false
				}
			};
		} else {
			_subProcess = new Process() {
				StartInfo = new() {
					FileName        = $"{Path.Join(AppContext.BaseDirectory, _executablePath)}",
					Arguments       = $"\"{_pipeName}\"",
					UseShellExecute = false
				}
			};
		}

		try {
			_subProcess.Start();
		} catch (Exception) {
			if (_executablePath != null) {
				throw new FileNotFoundException($"The executable path \"{Path.Join(AppContext.BaseDirectory, _executablePath)}\" could not be located, is locked, or is not a valid executable.");
			} else {
				throw new FileNotFoundException($"The Python executable \"{_pythonPath}\" could not be located, is locked, or is not a valid executable.");
			}
		}

		_pipeServer.WaitForConnection();
		_pipeStreamWriter = new BinaryWriter(_pipeServer);
		_pipeStreamReader = new BinaryReader(_pipeServer);

		var ready = receive();
		// Wait until we receive a signal from the desired sub-process. Otherwise, close and wait for a new connection.
		if ((string?) ready?["cm"] == "ready" && ready?["dt"]?.ForceInt() == _subProcess.Id) {
			Console.WriteLine("Python is ready!");
		} else {
			_pipeServer.Close();
			throw new InvalidDataException("Process connected to named pipe does not match expected process ID.");
		}

		if (!PyPrimitivesInit) {
			Len = Eval("len");
			Str = Eval("str");
		}
	}

	public void Exec(string pythonCode) {
		Default = this;
		
		Dictionary<string, object> result;
		do {
			result = sendAndReceive(new() { ["cm"] = "exec", ["dt"] = pythonCode });
			switch ((string) result["cm"]) {
				case "call": {
					var methodName = (string) result["func"];
					var args = Eval("[*args]");
					int argCount = Len.Invoke(args).Result;

					var argArray = new PyObject[] { };
					for (var i = 0; i < argCount; i++) {
						argArray.Append(args[i]);
					}

					var method = _boundFuncs[methodName];
					try {
						var returnVal = method.Invoke(argArray);
						result = sendAndReceive(new() { ["cm"] = "retn", ["dt"] = returnVal.getExpression() });
					} catch (Exception ex) {
						result = sendAndReceive(new() { ["cm"] = "err", ["dt"] = new List<object> { ex.GetType().ToString(), ex.Message } });
					}

					break;
				}

				case "err": {
					var excInfo = (List<object>) result["dt"];
					throw new PyException((string) excInfo[0], (string) excInfo[1]);
				}

				case "done":
					break;

				default:
					// TODO: Thrown exception about unknown command code
					break;
			}
		} while ((string) result["cm"] != "done" && (string) result["cm"] != "err");
	}

	public PyObject Eval(string pythonExpression) {
		Default = this;

		var pyObject = PyProxy.Create(this);
		Exec($"global {pyObject.pyGVarName} \n"
		   + $"{pyObject.pyGVarName} = {pythonExpression} \n");

		return pyObject;
	}

	public void BindMethod(string pyFuncName, MethodBinding csMethod) {
		Default = this;

		if (!Util.IdentRegex.IsMatch(pyFuncName)) {
			throw new FormatException("Python function name must be a valid identifier.");
		}

		_boundFuncs[pyFuncName] = csMethod;
		Exec($"def {pyFuncName}(*args): \n"
		   + $"    return ___call_cs_method('{pyFuncName}', *args) \n");
	}

	public void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing) {
		if (!_disposedValue) {
			if (disposing) {
				if (Default == this) {
					Default = null;
				}
			}

			if (_subProcess != null) {
				try {
					_subProcess.Kill();
				} catch { } finally {
					_subProcess.Dispose();
				}
				_pipeServer.Dispose();
				_disposedValue = true;
				_subProcess = null;
			}
		}
	}

	private Dictionary<string, object> receive() {
		var length = _pipeStreamReader.ReadInt32();
		var data   = _pipeStreamReader.ReadBytes(length);
		return MessagePackSerializer.Deserialize<Dictionary<string, object>>(data);
	}

	private Dictionary<string, object> sendAndReceive(Dictionary<string, object> data) {
		// First, send
		var bytes = MessagePackSerializer.Serialize(data);
		_pipeStreamWriter.Write(bytes.Length);
		_pipeStreamWriter.Write(bytes);

		// Now, receive
		return receive();
	}

	~Engine() {
		Dispose(disposing: false);
	}
}
