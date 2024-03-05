namespace PyEngine;

public class PyException: Exception {
	public string PyExceptionType { get; init; }

	public PyException(): base() {
		PyExceptionType = "";
	}
	public PyException(string message): base(message) {
		PyExceptionType = "";
	}
	public PyException(string pyExcType, string message): base(pyExcType + " - " + message) {
		PyExceptionType = pyExcType;
	}
}
