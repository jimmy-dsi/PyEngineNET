﻿using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

var Print = engine.Eval("print");
var Repr  = engine.Eval("repr");

engine.Exec("import sys");
engine.Exec("import platform");
engine.Exec("print(f'Python {sys.version} on {sys.platform}')");
engine.Exec("del sys");
engine.Exec("del platform");
engine.Exec("""
def def_exc_handler(ex):
	return
""");
engine.SetExceptionHandler("def_exc_handler");
	
// Add example C# function to demonstrate Python/.NET interop
engine.BindFunction("net_factorize", (long n) => {
	var factors = new List<long>();
	for (long i = 2; i <= n/2; i++) {
		if (n % i == 0) {
			factors.Add(i);
		}
	}
	return factors.ToArray();
});

// Add Python equivalent for comparison
engine.Exec("""
def py_factorize(n):
	factors = []
	for i in range(2, n//2 + 1):
		if n % i == 0:
			factors.append(i)
	return factors
""");

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
			if (!success) engine.Exec(pyCode); // Fall back to executing if it cannot be evaluated as an expression.
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