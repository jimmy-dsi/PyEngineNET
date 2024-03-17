using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

var Print = engine.Eval("print");
var Repr  = engine.Eval("repr");

engine.Exec("import sys");
engine.Exec("print(f'Python {sys.version}')");
engine.Exec("""
def def_exc_handler(ex):
	return
""");
engine.SetExceptionHandler("def_exc_handler");

var execInProgress = false;

Console.CancelKeyPress += (sender, e) => {
	e.Cancel = true;

	if (!execInProgress) {
		Console.WriteLine();
		Console.WriteLine("KeyboardInterrupt");
		Console.Write(">>> ");
	}
};

try {
	while (true) {
		Console.Write(">>> ");
		string? pyCode = null;
		while (pyCode == null) {
			pyCode = Console.ReadLine();
		}

		if (pyCode.Trim() == "") {
			continue;
		}

		execInProgress = true;

		try {
			var success = TryEval(pyCode);
			if (success) continue;
			engine.Exec(pyCode); // Fall back to executing if it cannot be evaluated as an expression.
		} catch (PyException ex) {
			if (ex.PyExceptionType is "SyntaxError" or "IndentationError") {
				var messageSplit = ex.PyMessage.Split('(');
				var fileInfo = messageSplit[1].Split(')')[0].Split(',');
				Console.WriteLine($"  File \"{fileInfo[0]}\", {fileInfo[1].Trim()}");
				Console.WriteLine($"    {pyCode}");
				Console.WriteLine($"{ex.PyExceptionType}: {messageSplit[0]}");
			} else {
				Console.WriteLine("Traceback (most recent call last):");
				foreach (var item in ex.PyTraceback.Skip(1)) {
					Console.WriteLine($"  File \"{item.FileName}\", line {item.LineNo}, in {item.FunctionName}");
				}
				Console.WriteLine($"{ex.PyExceptionType}: {ex.PyMessage}");
			}
		} finally {
			execInProgress = false;
		}
	}
} catch (EndOfStreamException) {
	// Catch the exception raised when the Python process unexpectedly quits.
} finally {
	engine.Dispose();
}


bool TryEval(string pyCode) {
	try {
		var result = engine.Eval(pyCode);
		if (result.IsNot(PyObject.None)) {
			engine.Exec(Print.Invoke(Repr.Invoke(result)));
		}
	} catch (PyException ex) {
		if (ex.PyExceptionType == "SyntaxError") {
			return false;
		} else {
			throw;
		}
	}
	return true;
}