using PyEngine;
using System.Text;
using System.Text.RegularExpressions;

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
			var pyLine = Console.ReadLine();
			pyCode = ReadBlock(pyLine);
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
				Console.WriteLine($"    {pyCode.Split('\n').Last()}");
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
} catch (PythonExitedException) {
	// Catch the exception raised when the Python process unexpectedly quits.
}

string? ReadBlock(string? firstLine) {
	if (firstLine == null) return null;

	var regex   = Regex.Match(firstLine, @"^\@?[A-Za-z_]+");
	var keyword = regex.Success ? regex.Value : "";

	switch (keyword) {
		case "def":
		case "if":
		case "for":
		case "while":
		case "class":
		case "match":
		case "try":
		case "async":
			break;
		default:
			if (keyword.StartsWith('@')) break;
			return firstLine;
	}

	var sb = new StringBuilder(firstLine);
	string? nextLine = null;

	while (true) {
		Console.Write("... ");
		nextLine = Console.ReadLine();
		if (nextLine == null) {
			return null;
		} else if (nextLine.Trim() == "") {
			break;
		} else {
			sb.Append('\n').Append(nextLine);
		}
	}

	return sb.ToString();
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