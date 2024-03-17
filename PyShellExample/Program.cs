using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

engine.Exec("import sys");
engine.Exec("print(f'Python {sys.version}')");

while (true) {
	Console.Write(">>> ");
	var execCmd = Console.ReadLine();
	try {
		var result = engine.Eval(execCmd);
		if (result.IsNot(PyObject.None)) {
			Console.WriteLine(result.ToString());
		}
	} catch (PyException ex) {
		Console.WriteLine("Traceback (most recent call last):");
		foreach (var item in ex.PyTraceback.Skip(1)) {
			Console.WriteLine($"  File \"{item.FileName}\", line {item.LineNo}, in {item.FunctionName}");
		}
	}
}