namespace PyEngine;

using System.Text;

public class PyException: Exception {
	private PyTraceback[] _traceback;

	public string PyExceptionType { get; init; }
	public string PyMessage       { get; init; }

	public IEnumerable<PyTraceback> PyTraceback {
		get {
			foreach (var item in _traceback) {
				yield return item;
			}
		}
	}

	public int PyTracebackLength => _traceback.Length;

	internal PyException(): base() {
		PyExceptionType = "";
		PyMessage = "";
		
		_traceback = new PyTraceback[0];
	}

	internal PyException(string message): base(message) {
		PyExceptionType = "";
		PyMessage = message;
		
		_traceback = new PyTraceback[0];
	}

	internal PyException(string pyExcType, string message): base(pyExcType + " - " + message) {
		PyExceptionType = pyExcType;
		PyMessage       = message;

		_traceback = new PyTraceback[0];
	}

	internal PyException(string pyExcType, string message, PyObject[] traceback): base(pyExcType + " - " + message) {
		PyExceptionType = pyExcType;
		PyMessage       = message;

		var tb = new List<PyTraceback>();
		foreach (var item in traceback) {
			var pyTuple = (PyObject[]) item;
			tb.Add(new PyTraceback {
				FileName       = (string) pyTuple[0],
				LineNo         = (int)    pyTuple[1],
				FunctionName   = (string) pyTuple[2],
				Text           = (string) pyTuple[3],
				FunctionParams = pyTuple.Length > 4 ? (string) pyTuple[4] : ""
			});
		}

		_traceback = tb.ToArray();
	}

	internal void AppendTraceback(PyObject[] traceback) {
		var tb = new List<PyTraceback>();
		foreach (var item in traceback) {
			var pyTuple = (PyObject[]) item;
			tb.Add(new PyTraceback {
				FileName       = (string) pyTuple[0],
				LineNo         = (int)    pyTuple[1],
				FunctionName   = (string) pyTuple[2],
				Text           = (string) pyTuple[3],
				FunctionParams = pyTuple.Length > 4 ? (string) pyTuple[4] : ""
			});
		}

		tb.AddRange(_traceback);
		_traceback = tb.ToArray();
	}

	internal void SetTraceback(PyObject[] traceback) {
		var tb = new List<PyTraceback>();
		foreach (var item in traceback) {
			var pyTuple = (PyObject[]) item;
			tb.Add(new PyTraceback {
				FileName       = (string) pyTuple[0],
				LineNo         = (int)    pyTuple[1],
				FunctionName   = (string) pyTuple[2],
				Text           = (string) pyTuple[3],
				FunctionParams = pyTuple.Length > 4 ? (string) pyTuple[4] : ""
			});
		}

		_traceback = tb.ToArray();
	}

	public override string ToString() {
		var baseStringLines = base.ToString().Split('\n');
		// Append base exception message
		var sb = new StringBuilder($"{baseStringLines[0]}");
		// Append Python traceback in reverse order; In Python it's most recent last, but in C# it's most recent first
		foreach (var item in _traceback.Reverse()) {
			sb.Append("\n");
			if (item.FileName == "") {
				// Do not try and display file/line information if we do not have access to debug symbols
				sb.Append($"   at {item.FunctionName}({item.FunctionParams})");
			} else {
				sb.Append($"   at {item.FunctionName}({item.FunctionParams}) in {item.FileName}:line {item.LineNo}");
			}
		}
		// Append the original stacktrace to the end, since in C#, it's most recent first
		sb.Append("\n");
		sb.Append($"{string.Join('\n', baseStringLines.Skip(1))}");
		return sb.ToString();
	}
}

public class PyIOError: PyException {
	internal PyIOError(): base() { }
	internal PyIOError(string message): base(message) { }
	internal PyIOError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyIOError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyOSError: PyException {
	internal PyOSError(): base() { }
	internal PyOSError(string message): base(message) { }
	internal PyOSError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyOSError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyFileNotFoundError: PyOSError {
	internal PyFileNotFoundError(): base() { }
	internal PyFileNotFoundError(string message): base(message) { }
	internal PyFileNotFoundError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyFileNotFoundError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyFileExistsError: PyOSError {
	internal PyFileExistsError(): base() { }
	internal PyFileExistsError(string message): base(message) { }
	internal PyFileExistsError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyFileExistsError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyPermissionError: PyOSError {
	internal PyPermissionError(): base() { }
	internal PyPermissionError(string message): base(message) { }
	internal PyPermissionError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyPermissionError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyConnectionError: PyOSError {
	internal PyConnectionError(): base() { }
	internal PyConnectionError(string message): base(message) { }
	internal PyConnectionError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyConnectionError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyTimeoutError: PyOSError {
	internal PyTimeoutError(): base() { }
	internal PyTimeoutError(string message): base(message) { }
	internal PyTimeoutError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyTimeoutError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyIsADirectoryError: PyOSError {
	internal PyIsADirectoryError(): base() { }
	internal PyIsADirectoryError(string message): base(message) { }
	internal PyIsADirectoryError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyIsADirectoryError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyValueError: PyException {
	internal PyValueError(): base() { }
	internal PyValueError(string message): base(message) { }
	internal PyValueError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyValueError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyTypeError: PyException {
	internal PyTypeError(): base() { }
	internal PyTypeError(string message): base(message) { }
	internal PyTypeError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyTypeError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyAttributeError: PyException {
	internal PyAttributeError(): base() { }
	internal PyAttributeError(string message): base(message) { }
	internal PyAttributeError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyAttributeError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyImportError: PyException {
	internal PyImportError(): base() { }
	internal PyImportError(string message): base(message) { }
	internal PyImportError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyImportError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyModuleNotFoundError: PyImportError {
	internal PyModuleNotFoundError(): base() { }
	internal PyModuleNotFoundError(string message): base(message) { }
	internal PyModuleNotFoundError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyModuleNotFoundError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyNameError: PyException {
	internal PyNameError(): base() { }
	internal PyNameError(string message): base(message) { }
	internal PyNameError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyNameError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyLookupError: PyException {
	internal PyLookupError(): base() { }
	internal PyLookupError(string message): base(message) { }
	internal PyLookupError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyLookupError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyIndexError: PyLookupError {
	internal PyIndexError(): base() { }
	internal PyIndexError(string message): base(message) { }
	internal PyIndexError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyIndexError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyKeyError: PyLookupError {
	internal PyKeyError(): base() { }
	internal PyKeyError(string message): base(message) { }
	internal PyKeyError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyKeyError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyStopIteration: PyException {
	internal PyStopIteration(): base() { }
	internal PyStopIteration(string message): base(message) { }
	internal PyStopIteration(string pyExcType, string message): base(pyExcType, message) { }
	internal PyStopIteration(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyStopAsyncIteration: PyException {
	internal PyStopAsyncIteration(): base() { }
	internal PyStopAsyncIteration(string message): base(message) { }
	internal PyStopAsyncIteration(string pyExcType, string message): base(pyExcType, message) { }
	internal PyStopAsyncIteration(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyArithmeticError: PyException {
	internal PyArithmeticError(): base() { }
	internal PyArithmeticError(string message): base(message) { }
	internal PyArithmeticError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyArithmeticError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyZeroDivisionError: PyArithmeticError {
	internal PyZeroDivisionError(): base() { }
	internal PyZeroDivisionError(string message): base(message) { }
	internal PyZeroDivisionError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyZeroDivisionError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyOverflowError: PyArithmeticError {
	internal PyOverflowError(): base() { }
	internal PyOverflowError(string message): base(message) { }
	internal PyOverflowError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyOverflowError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyFloatingPointError: PyArithmeticError {
	internal PyFloatingPointError(): base() { }
	internal PyFloatingPointError(string message): base(message) { }
	internal PyFloatingPointError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyFloatingPointError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyAssertionError: PyException {
	internal PyAssertionError(): base() { }
	internal PyAssertionError(string message): base(message) { }
	internal PyAssertionError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyAssertionError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyRuntimeError: PyException {
	internal PyRuntimeError(): base() { }
	internal PyRuntimeError(string message): base(message) { }
	internal PyRuntimeError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyRuntimeError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyNotImplementedError: PyException {
	internal PyNotImplementedError(): base() { }
	internal PyNotImplementedError(string message): base(message) { }
	internal PyNotImplementedError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyNotImplementedError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyMemoryError: PyException {
	internal PyMemoryError(): base() { }
	internal PyMemoryError(string message): base(message) { }
	internal PyMemoryError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyMemoryError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyBufferError: PyException {
	internal PyBufferError(): base() { }
	internal PyBufferError(string message): base(message) { }
	internal PyBufferError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyBufferError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyReferenceError: PyException {
	internal PyReferenceError(): base() { }
	internal PyReferenceError(string message): base(message) { }
	internal PyReferenceError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyReferenceError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PySystemError: PyException {
	internal PySystemError(): base() { }
	internal PySystemError(string message): base(message) { }
	internal PySystemError(string pyExcType, string message): base(pyExcType, message) { }
	internal PySystemError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PySyntaxError: PyException {
	internal PySyntaxError(): base() { }
	internal PySyntaxError(string message): base(message) { }
	internal PySyntaxError(string pyExcType, string message): base(pyExcType, message) { }
	internal PySyntaxError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyEOFError: PyException {
	internal PyEOFError(): base() { }
	internal PyEOFError(string message): base(message) { }
	internal PyEOFError(string pyExcType, string message): base(pyExcType, message) { }
	internal PyEOFError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public struct PyTraceback {
	public string FileName;
	public int    LineNo;
	public string FunctionName;
	public string Text;
	public string FunctionParams;
}