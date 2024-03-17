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
				FileName     = (string) pyTuple[0],
				LineNo       = (int)    pyTuple[1],
				FunctionName = (string) pyTuple[2],
				Text         = (string) pyTuple[3]
			});
		}

		_traceback = tb.ToArray();
	}

	public override string ToString() {
		var sb = new StringBuilder(base.ToString());
		foreach (var item in _traceback) {
			sb.Append("\n");
			sb.Append($"   at {item.FunctionName}() in {item.FileName}:line {item.LineNo}");
		}
		return sb.ToString();
	}
}

public struct PyTraceback {
	public string FileName;
	public int    LineNo;
	public string FunctionName;
	public string Text;
}