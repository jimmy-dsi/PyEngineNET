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

public struct PyTraceback {
	public string FileName;
	public int    LineNo;
	public string FunctionName;
	public string Text;
	public string FunctionParams;
}