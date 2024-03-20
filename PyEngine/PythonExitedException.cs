namespace PyEngine;

public class PythonExitedException: Exception {
	public PythonExitedException(string message): base(message) { }
}
