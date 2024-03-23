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

	public PyException(): base() {
		PyExceptionType = "";
		PyMessage = "";
		
		_traceback = new PyTraceback[0];
	}

	public PyException(string message): base(message) {
		PyExceptionType = "";
		PyMessage = message;
		
		_traceback = new PyTraceback[0];
	}

	public PyException(string pyExcType, string message): base(pyExcType + " - " + message) {
		PyExceptionType = pyExcType;
		PyMessage       = message;

		_traceback = new PyTraceback[0];
	}

	public PyException(string pyExcType, string message, PyObject[] traceback): base(pyExcType + " - " + message) {
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
	public PyIOError(): base() { }
	public PyIOError(string message): base(message) { }
	public PyIOError(string pyExcType, string message): base(pyExcType, message) { }
	public PyIOError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyOSError: PyException {
	public PyOSError(): base() { }
	public PyOSError(string message): base(message) { }
	public PyOSError(string pyExcType, string message): base(pyExcType, message) { }
	public PyOSError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyFileNotFoundError: PyOSError {
	public PyFileNotFoundError(): base() { }
	public PyFileNotFoundError(string message): base(message) { }
	public PyFileNotFoundError(string pyExcType, string message): base(pyExcType, message) { }
	public PyFileNotFoundError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyFileExistsError: PyOSError {
	public PyFileExistsError(): base() { }
	public PyFileExistsError(string message): base(message) { }
	public PyFileExistsError(string pyExcType, string message): base(pyExcType, message) { }
	public PyFileExistsError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyPermissionError: PyOSError {
	public PyPermissionError(): base() { }
	public PyPermissionError(string message): base(message) { }
	public PyPermissionError(string pyExcType, string message): base(pyExcType, message) { }
	public PyPermissionError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyConnectionError: PyOSError {
	public PyConnectionError(): base() { }
	public PyConnectionError(string message): base(message) { }
	public PyConnectionError(string pyExcType, string message): base(pyExcType, message) { }
	public PyConnectionError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyTimeoutError: PyOSError {
	public PyTimeoutError(): base() { }
	public PyTimeoutError(string message): base(message) { }
	public PyTimeoutError(string pyExcType, string message): base(pyExcType, message) { }
	public PyTimeoutError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyIsADirectoryError: PyOSError {
	public PyIsADirectoryError(): base() { }
	public PyIsADirectoryError(string message): base(message) { }
	public PyIsADirectoryError(string pyExcType, string message): base(pyExcType, message) { }
	public PyIsADirectoryError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyValueError: PyException {
	public PyValueError(): base() { }
	public PyValueError(string message): base(message) { }
	public PyValueError(string pyExcType, string message): base(pyExcType, message) { }
	public PyValueError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyTypeError: PyException {
	public PyTypeError(): base() { }
	public PyTypeError(string message): base(message) { }
	public PyTypeError(string pyExcType, string message): base(pyExcType, message) { }
	public PyTypeError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyAttributeError: PyException {
	public PyAttributeError(): base() { }
	public PyAttributeError(string message): base(message) { }
	public PyAttributeError(string pyExcType, string message): base(pyExcType, message) { }
	public PyAttributeError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyImportError: PyException {
	public PyImportError(): base() { }
	public PyImportError(string message): base(message) { }
	public PyImportError(string pyExcType, string message): base(pyExcType, message) { }
	public PyImportError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyModuleNotFoundError: PyImportError {
	public PyModuleNotFoundError(): base() { }
	public PyModuleNotFoundError(string message): base(message) { }
	public PyModuleNotFoundError(string pyExcType, string message): base(pyExcType, message) { }
	public PyModuleNotFoundError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyNameError: PyException {
	public PyNameError(): base() { }
	public PyNameError(string message): base(message) { }
	public PyNameError(string pyExcType, string message): base(pyExcType, message) { }
	public PyNameError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyLookupError: PyException {
	public PyLookupError(): base() { }
	public PyLookupError(string message): base(message) { }
	public PyLookupError(string pyExcType, string message): base(pyExcType, message) { }
	public PyLookupError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyIndexError: PyLookupError {
	public PyIndexError(): base() { }
	public PyIndexError(string message): base(message) { }
	public PyIndexError(string pyExcType, string message): base(pyExcType, message) { }
	public PyIndexError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyKeyError: PyLookupError {
	public PyKeyError(): base() { }
	public PyKeyError(string message): base(message) { }
	public PyKeyError(string pyExcType, string message): base(pyExcType, message) { }
	public PyKeyError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyStopIteration: PyException {
	public PyStopIteration(): base() { }
	public PyStopIteration(string message): base(message) { }
	public PyStopIteration(string pyExcType, string message): base(pyExcType, message) { }
	public PyStopIteration(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyStopAsyncIteration: PyException {
	public PyStopAsyncIteration(): base() { }
	public PyStopAsyncIteration(string message): base(message) { }
	public PyStopAsyncIteration(string pyExcType, string message): base(pyExcType, message) { }
	public PyStopAsyncIteration(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyArithmeticError: PyException {
	public PyArithmeticError(): base() { }
	public PyArithmeticError(string message): base(message) { }
	public PyArithmeticError(string pyExcType, string message): base(pyExcType, message) { }
	public PyArithmeticError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyZeroDivisionError: PyArithmeticError {
	public PyZeroDivisionError(): base() { }
	public PyZeroDivisionError(string message): base(message) { }
	public PyZeroDivisionError(string pyExcType, string message): base(pyExcType, message) { }
	public PyZeroDivisionError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyOverflowError: PyArithmeticError {
	public PyOverflowError(): base() { }
	public PyOverflowError(string message): base(message) { }
	public PyOverflowError(string pyExcType, string message): base(pyExcType, message) { }
	public PyOverflowError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyFloatingPointError: PyArithmeticError {
	public PyFloatingPointError(): base() { }
	public PyFloatingPointError(string message): base(message) { }
	public PyFloatingPointError(string pyExcType, string message): base(pyExcType, message) { }
	public PyFloatingPointError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyAssertionError: PyException {
	public PyAssertionError(): base() { }
	public PyAssertionError(string message): base(message) { }
	public PyAssertionError(string pyExcType, string message): base(pyExcType, message) { }
	public PyAssertionError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyRuntimeError: PyException {
	public PyRuntimeError(): base() { }
	public PyRuntimeError(string message): base(message) { }
	public PyRuntimeError(string pyExcType, string message): base(pyExcType, message) { }
	public PyRuntimeError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyNotImplementedError: PyException {
	public PyNotImplementedError(): base() { }
	public PyNotImplementedError(string message): base(message) { }
	public PyNotImplementedError(string pyExcType, string message): base(pyExcType, message) { }
	public PyNotImplementedError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyMemoryError: PyException {
	public PyMemoryError(): base() { }
	public PyMemoryError(string message): base(message) { }
	public PyMemoryError(string pyExcType, string message): base(pyExcType, message) { }
	public PyMemoryError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyBufferError: PyException {
	public PyBufferError(): base() { }
	public PyBufferError(string message): base(message) { }
	public PyBufferError(string pyExcType, string message): base(pyExcType, message) { }
	public PyBufferError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyReferenceError: PyException {
	public PyReferenceError(): base() { }
	public PyReferenceError(string message): base(message) { }
	public PyReferenceError(string pyExcType, string message): base(pyExcType, message) { }
	public PyReferenceError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PySystemError: PyException {
	public PySystemError(): base() { }
	public PySystemError(string message): base(message) { }
	public PySystemError(string pyExcType, string message): base(pyExcType, message) { }
	public PySystemError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PySyntaxError: PyException {
	public PySyntaxError(): base() { }
	public PySyntaxError(string message): base(message) { }
	public PySyntaxError(string pyExcType, string message): base(pyExcType, message) { }
	public PySyntaxError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public class PyEOFError: PyException {
	public PyEOFError(): base() { }
	public PyEOFError(string message): base(message) { }
	public PyEOFError(string pyExcType, string message): base(pyExcType, message) { }
	public PyEOFError(string pyExcType, string message, PyObject[] traceback): base(pyExcType, message, traceback) { }
}

public struct PyTraceback {
	public string FileName;
	public int    LineNo;
	public string FunctionName;
	public string Text;
	public string FunctionParams;
}