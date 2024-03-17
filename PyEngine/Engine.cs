namespace PyEngine;

using MessagePack;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;

public partial class Engine: IDisposable {
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
	internal PyObject? len = null;
	internal PyObject? str = null;

	internal PyObject Len => len ?? throw new InvalidOperationException("Engine instance has not been started.");
	internal PyObject Str => str ?? throw new InvalidOperationException("Engine instance has not been started.");

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
		// Make sure we receive a signal from the desired sub-process. Otherwise, report an error.
		if ((string?) ready?["cm"] == "ready" && ready?["dt"]?.ForceInt() == _subProcess.Id) {
			//Console.WriteLine("Python is ready!");
		} else {
			_pipeServer.Close();
			throw new InvalidDataException("Process connected to named pipe does not match expected process ID.");
		}

		if (!PyPrimitivesInit) {
			len = Eval("len");
			str = Eval("str");
		}
	}

	public void Exec(string pythonCode) {
		Default = this;
		
		Dictionary<string, object> result = sendAndReceive(new() { ["cm"] = "exec", ["dt"] = pythonCode });
		while (true) {
			switch ((string) result["cm"]) {
				case "call":
					processCall(ref result);
					break;

				case "err": {
					var excInfo = (PyObject[]) (PyObject) (object?[]) result["dt"];
					throw new PyException((string) excInfo[0], (string) excInfo[1], (PyObject[]) excInfo[2]);
				}

				case "done":
					return;

				default:
					throw new InvalidOperationException($"Unknown command received from Python driver: \"{(string) result["cm"]}\"");
			}
		}
	}

	public PyObject Eval(string pythonExpression, bool eager = false) {
		Default = this;

		if (eager) {
			Dictionary<string, object> result = sendAndReceive(new() { ["cm"] = "eval", ["dt"] = pythonExpression });
			while (true) {
				switch ((string) result["cm"]) {
					case "call":
						processCall(ref result);
						break;

					case "err": {
						var excInfo = (object[]) result["dt"];
						throw new PyException((string) excInfo[0], (string) excInfo[1]);
					}

					case "res": {
						var pyResolve = new PyResolved(this, result["dt"]);
						return pyResolve;
					}

					default:
						throw new InvalidOperationException($"Unknown command received from Python driver: \"{(string) result["cm"]}\"");
				}
			}
		} else {
			var pyObject = PyProxy.Create(this);
			Exec($"global {pyObject.pyGVarName} \n"
				+ $"{pyObject.pyGVarName} = {pythonExpression} \n");

			return pyObject;
		}
	}

	public static string PyExpression(object? value) {
		if (value is null) {
			return "None";
		}

		var vtype = value.GetType();
		if (value is PyObject) {
			return PyExpression(((PyResolved) ((PyObject) value).Result).Value);
		} else if (vtype == typeof(bool)) {
			var v = (bool) value;
			return v ? "True" : "False";
		} else if (vtype.IsNumericType()) {
			return value.ToString()!;
		} else if (vtype == typeof(string)) {
			return ((string) value).Escape();
		} else if (vtype == typeof(List<object>)) {
			var v = (List<object>) value;
			return $"[{string.Join(", ", v.Select(PyExpression))}]";
		} else if (vtype == typeof(List<PyObject>)) {
			var v = (List<PyObject>) value;
			return $"[{string.Join(", ", v.Select(PyExpression))}]";
		} else if (vtype == typeof(object[])) {
			var v = (object[]) value;
			return $"[{string.Join(", ", v.Select(PyExpression))}]";
		} else if (vtype == typeof(PyObject[])) {
			var v = (PyObject[]) value;
			return $"[{string.Join(", ", v.Select(PyExpression))}]";
		} else if (vtype == typeof(HashSet<object>)) {
			var v = (HashSet<object>) value;
			if (v.Count == 0) {
				return "set()";
			} else {
				return $"{{{string.Join(", ", v.Select(PyExpression))}}}";
			}
		} else if (vtype == typeof(HashSet<PyObject>)) {
			var v = (HashSet<PyObject>) value;
			if (v.Count == 0) {
				return "set()";
			} else {
				return $"{{{string.Join(", ", v.Select(PyExpression))}}}";
			}
		} else if (vtype == typeof(Dictionary<object, object>)) {
			var v = (Dictionary<object, object>) value;
			return $"{{{string.Join(", ", v.Select(x => $"{PyExpression(x.Key)} : {PyExpression(x.Value)}"))}}}";
		} else if (vtype == typeof(Dictionary<PyObject, PyObject>)) {
			var v = (Dictionary<PyObject, PyObject>) value;
			return $"{{{string.Join(", ", v.Select(x => $"{PyExpression(x.Key)} : {PyExpression(x.Value)}"))}}}";
		} else if (vtype == typeof(byte[])) {
			var v = (byte[]) value;
			return $"bytearray([{string.Join(", ", v.Select(x => PyExpression(x)))}])";
		} else if (vtype == typeof(List<byte>)) {
			var v = (List<byte>) value;
			return $"bytearray([{string.Join(", ", v.Select(x => PyExpression(x)))}])";
		} else if (value is DataClassObject) {
			var v = (DataClassObject) value;
			return $"{v.ClassName}({string.Join(", ", v.PropNames.Select(x => $"{x}= {PyExpression(v[x])}"))})";
		} else if (value is HashSet<bool>
		        || value is HashSet<byte>
		        || value is HashSet<sbyte>
		        || value is HashSet<ushort>
		        || value is HashSet<short>
		        || value is HashSet<uint>
		        || value is HashSet<int>
		        || value is HashSet<ulong>
		        || value is HashSet<long>
		        || value is HashSet<float>
		        || value is HashSet<double>
		        || value is HashSet<decimal>
		        || value is HashSet<string>)
		{
			var v = ((IEnumerable) value).ToEnum().ToHashSet();
			if (v.Count == 0) {
				return "set()";
			} else {
				return $"{{{string.Join(", ", v.Select(PyExpression))}}}";
			}
		} else if (value is IEnumerable) {
			var v = (IEnumerable) value;
			return $"[{string.Join(", ", v.ToEnum().Select(PyExpression))}]";
		} else {
			throw new InvalidOperationException($"Cannot convert value of type `{vtype}` to a Python expression.");
		}
	}

	private void processCall(ref Dictionary<string, object> result) {
		var methodName = (string) result["func"];
		var args = Eval("[*___args]");
		var argCountPyObj = len!.Invoke(args);
		var argCountP = argCountPyObj.Result;
		int argCount = argCountP;

		var argList = new List<PyObject> { };
		for (var i = 0; i < argCount; i++) {
			argList.Add(args[i]);
		}
		var argArray = argList.ToArray();

		var method = _boundFuncs[methodName];
		try {
			var returnVal = method.Invoke(argArray);
			result = sendAndReceive(new() { ["cm"] = "retn", ["dt"] = returnVal.getExpression() });
		} catch (Exception ex) {
			result = sendAndReceive(new() { ["cm"] = "err", ["dt"] = new List<object> { ex.GetType().ToString(), ex.Message } });
		}
	}

	internal void BindMethod(string pyFuncName, MethodBinding csMethod) {
		Default = this;

		if (!Util.IdentRegex.IsMatch(pyFuncName)) {
			throw new FormatException("Python function name must be a valid identifier.");
		}

		_boundFuncs[pyFuncName] = csMethod;
		Exec($"global {pyFuncName} \n"
		   + $"def {pyFuncName}(*args): \n"
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
		if (length < 0) {
			throw new PipeDataException("Invalid data size from pipe");
		}
		var data = _pipeStreamReader.ReadBytes(length);
		return MessagePackSerializer.Deserialize<Dictionary<string, object>>(data);
	}

	private Dictionary<string, object> sendAndReceive(Dictionary<string, object> data) {
		// First, send
		byte[] bytes;
		try {
			bytes = MessagePackSerializer.Serialize(data);
		} catch (OverflowException) {
			throw new PipeDataException("Data too large for pipe");
		} catch (OutOfMemoryException) {
			throw new PipeDataException("Data too large for pipe");
		}

		_pipeStreamWriter.Write(bytes.Length);
		_pipeStreamWriter.Write(bytes);
		_pipeStreamWriter.Flush();

		// Now, receive
		return receive();
	}

	~Engine() {
		Dispose(disposing: false);
	}
}
