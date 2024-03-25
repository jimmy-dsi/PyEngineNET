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

	private readonly Dictionary<string, MethodBinding>    _boundFuncs = new();
	private readonly Dictionary<string, GeneratorBinding> _boundGens  = new();

	private readonly Dictionary<int, IEnumerator<PyObject>> _mappedGenerators = new();
	private readonly Dictionary<string, Type> _excTypes = new();
	private Exception? _lastException = null;

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
		if (_disposedValue) {
			throw new ObjectDisposedException(GetType().Name);
		}

		if (_subProcess != null) {
			throw new InvalidOperationException("Cannot perform Start() on already active engine.");
		}

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

	public void Exec(PyObject pyObject) => Exec(pyObject.ToString());

	public void Exec(string pythonCode) {
		if (_disposedValue) {
			throw new ObjectDisposedException(GetType().Name);
		}

		Default = this;
		
		Dictionary<string, object> result = sendAndReceive(new() { ["cm"] = "exec", ["dt"] = pythonCode });
		while (true) {
			switch ((string) result["cm"]) {
				case "call":
					processCall(ref result);
					break;

				case "step":
					processStep(ref result);
					break;

				case "err": {
					var excInfo = (PyObject[]) (PyObject) (object[]) result["dt"];
					throw capturePyException(excInfo);
				}

				case "done":
					return;

				default:
					throw new InvalidOperationException($"Unknown command received from Python driver: \"{(string) result["cm"]}\"");
			}
		}
	}

	public PyObject Eval(PyObject pyObject) => Eval(pyObject.ToString());

	public PyObject Eval(string pythonExpression, bool eager = false) {
		if (_disposedValue) {
			throw new ObjectDisposedException(GetType().Name);
		}

		Default = this;

		if (eager) {
			Dictionary<string, object> result = sendAndReceive(new() { ["cm"] = "eval", ["dt"] = pythonExpression });
			while (true) {
				switch ((string) result["cm"]) {
					case "call":
						processCall(ref result);
						break;

					case "step":
						processStep(ref result);
						break;

					case "err": {
						var excInfo = (PyObject[]) (PyObject) (object[]) result["dt"];
						throw capturePyException(excInfo);
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
			Exec($"global {pyObject.pyGVarName}; {pyObject.pyGVarName} = {pythonExpression}");

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

	public void SetExceptionHandler(string pyFuncName) {
		if (_disposedValue) {
			throw new ObjectDisposedException(GetType().Name);
		}

		Exec($"def ___exc_handler(ex): \n"
		   + $"    {pyFuncName}(ex)");
	}

	public void SetPyExceptionHandler(string pyFuncName) {
		if (_disposedValue) {
			throw new ObjectDisposedException(GetType().Name);
		}

		Exec($"def ___py_exc_handler(ex): \n"
		   + $"    {pyFuncName}(ex)");
	}

	public void SetNetExceptionHandler(string pyFuncName) {
		if (_disposedValue) {
			throw new ObjectDisposedException(GetType().Name);
		}

		Exec($"def ___net_exc_handler(ex): \n"
		   + $"    {pyFuncName}(ex)");
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

		try {
			PyObject? returnVal = null;
			if (_boundGens.ContainsKey(methodName)) {
				var method = _boundGens[methodName];
				var id = result["id"].AsIntType<int>();
				_mappedGenerators[id] = method.Invoke(argArray).GetEnumerator();
				returnVal = PyObject.None; // pyengine doesn't need a result in this case; can discard.
			} else {
				var method = _boundFuncs[methodName];
				returnVal = method.Invoke(argArray);
			}
			result = sendAndReceive(new() { ["cm"] = "retn", ["dt"] = returnVal?.getExpression() ?? "None" });
		} catch (Exception ex) {
			processException(ex, ref result);
		}
	}

	private void processStep(ref Dictionary<string, object> result) {
		var id = result["id"].AsIntType<int>();
		if (!_mappedGenerators.ContainsKey(id)) {
			result = sendAndReceive(new() { ["cm"] = "stop" });
			return;
		}

		try {
			var gen = _mappedGenerators[id];

			var isActive = gen.MoveNext();
			if (isActive) {
				var yieldValue = gen.Current;
				result = sendAndReceive(new() { ["cm"] = "yld", ["dt"] = yieldValue?.getExpression() ?? "None" });
			} else {
				_mappedGenerators.Remove(id);
				result = sendAndReceive(new() { ["cm"] = "stop" });
			}
		} catch (Exception ex) {
			processException(ex, ref result);
		}
	}

	private Exception capturePyException(PyObject[] excInfo) {
		var approxExcType   = (string)     excInfo[0];
		var reportedExcType = (string)     excInfo[1];
		var excMessage      = (string)     excInfo[2];
		var traceback       = (PyObject[]) excInfo[3];

		switch (approxExcType) {
			case "IOError":
				return new PyIOError(reportedExcType, excMessage, traceback);
			case "OSError":
				return new PyOSError(reportedExcType, excMessage, traceback);
			case "FileNotFoundError":
				return new PyFileNotFoundError(reportedExcType, excMessage, traceback);
			case "FileExistsError":
				return new PyFileExistsError(reportedExcType, excMessage, traceback);
			case "PermissionError":
				return new PyPermissionError(reportedExcType, excMessage, traceback);
			case "ConnectionError":
				return new PyConnectionError(reportedExcType, excMessage, traceback);
			case "TimeoutError":
				return new PyTimeoutError(reportedExcType, excMessage, traceback);
			case "IsADirectoryError":
				return new PyIsADirectoryError(reportedExcType, excMessage, traceback);
			case "ValueError":
				return new PyValueError(reportedExcType, excMessage, traceback);
			case "TypeError":
				return new PyTypeError(reportedExcType, excMessage, traceback);
			case "AttributeError":
				return new PyAttributeError(reportedExcType, excMessage, traceback);
			case "ImportError":
				return new PyImportError(reportedExcType, excMessage, traceback);
			case "ModuleNotFoundError":
				return new PyModuleNotFoundError(reportedExcType, excMessage, traceback);
			case "NameError":
				return new PyNameError(reportedExcType, excMessage, traceback);
			case "LookupError":
				return new PyLookupError(reportedExcType, excMessage, traceback);
			case "IndexError":
				return new PyIndexError(reportedExcType, excMessage, traceback);
			case "KeyError":
				return new PyKeyError(reportedExcType, excMessage, traceback);
			case "StopIteration":
				return new PyStopIteration(reportedExcType, excMessage, traceback);
			case "StopAsyncIteration":
				return new PyStopAsyncIteration(reportedExcType, excMessage, traceback);
			case "ArithmeticError":
				return new PyArithmeticError(reportedExcType, excMessage, traceback);
			case "ZeroDivisionError":
				return new PyZeroDivisionError(reportedExcType, excMessage, traceback);
			case "OverflowError":
				return new PyOverflowError(reportedExcType, excMessage, traceback);
			case "FloatingPointError":
				return new PyFloatingPointError(reportedExcType, excMessage, traceback);
			case "AssertionError":
				return new PyAssertionError(reportedExcType, excMessage, traceback);
			case "RuntimeError":
				return new PyRuntimeError(reportedExcType, excMessage, traceback);
			case "NotImplementedError":
				return new PyNotImplementedError(reportedExcType, excMessage, traceback);
			case "MemoryError":
				return new PyMemoryError(reportedExcType, excMessage, traceback);
			case "BufferError":
				return new PyBufferError(reportedExcType, excMessage, traceback);
			case "ReferenceError":
				return new PyReferenceError(reportedExcType, excMessage, traceback);
			case "SystemError":
				return new PySystemError(reportedExcType, excMessage, traceback);
			case "SyntaxError":
				return new PySyntaxError(reportedExcType, excMessage, traceback);
			case "EOFError":
				return new PyEOFError(reportedExcType, excMessage, traceback);
			case "NETException": {
				if (_lastException is PyException pex) {
					pex.SetTraceback(traceback);
					return pex;
				} else {
					var innerException = new PyException(reportedExcType, excMessage, traceback);
					try {
						return (Exception) Activator.CreateInstance(_excTypes[reportedExcType], excMessage, innerException)!;
					} catch (MissingMethodException) { }
					try {
						return (Exception) Activator.CreateInstance(_excTypes[reportedExcType], excMessage)!;
					} catch (MissingMethodException) { }
					try {
						return (Exception) Activator.CreateInstance(_excTypes[reportedExcType])!;
					} catch (MissingMethodException) {
						return _lastException!;
					}
				}
			}
			default:
				return new PyException(reportedExcType, excMessage, traceback);
		}
	}

	private void processException(Exception ex, ref Dictionary<string, object> result) {
		var traceback = getExcTraceback(ex);
		_excTypes[ex.GetType().ToString()] = ex.GetType();
		_lastException = ex;
		result = sendAndReceive(new() { ["cm"] = "err", ["dt"] = PyExpression(new object[] { ex.GetType().ToString(), ex.Message, traceback }) });
	}

	private List<object> getExcTraceback(Exception ex) {
		var traceback = new List<object>();

		var stackTrace = new StackTrace(ex, true);
		var stackFrames = stackTrace.GetFrames();

		foreach (var item in stackFrames.Reverse()) {
			var frameMethod = item.GetMethod();
			var fullMethodName = frameMethod == null ? "<unknown method>" : $"{frameMethod.ReflectedType}.{frameMethod.Name}";

			traceback.Add(new List<object> {
				item.GetFileName() ?? "",
				item.GetFileLineNumber(), 
				fullMethodName,
				"",
				frameMethod == null ? "" : string.Join(", ", frameMethod.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))
			});
		}

		if (ex is PyException pex) {
			foreach (var item in pex.PyTraceback) {
				var fullMethodName = item.FunctionName;

				traceback.Add(new List<object> {
					item.FileName,
					item.LineNo, 
					fullMethodName,
					item.Text,
					item.FunctionParams
				});
			}
		}

		if (ex.InnerException != null) {
			traceback.AddRange(getExcTraceback(ex.InnerException));
		}

		return traceback;
	}

	internal void BindMethod(string pyFuncName, MethodBinding csMethod) {
		Default = this;

		if (!Util.IdentRegex.IsMatch(pyFuncName)) {
			throw new FormatException("Python function name must be a valid identifier.");
		}

		_boundFuncs[pyFuncName] = csMethod;
		Exec($"global {pyFuncName} \n"
		   + $"def {pyFuncName}(*args): \n"
		   + $"    return ___call_cs_method('{pyFuncName}', None, *args) \n");
	}

	internal void BindGeneratorMethod(string pyFuncName, GeneratorBinding csMethod) {
		Default = this;

		if (!Util.IdentRegex.IsMatch(pyFuncName)) {
			throw new FormatException("Python function name must be a valid identifier.");
		}

		_boundGens[pyFuncName] = csMethod;
		Exec($"global {pyFuncName} \n"
		   + $"class {pyFuncName}(___NETGenerator): \n"
		   + $"    def __init__(self, *args): \n"
		   + $"        super().__init__('{pyFuncName}', *args) \n");
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
		try {
			var length = _pipeStreamReader.ReadInt32();
			if (length < 0) {
				throw new PipeDataException("Invalid data size from pipe");
			}
			var data = _pipeStreamReader.ReadBytes(length);
			return MessagePackSerializer.Deserialize<Dictionary<string, object>>(data);
		} catch (EndOfStreamException) {
			Dispose();
			throw new PythonExitedException("The Python process has been exited.");
		}
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
