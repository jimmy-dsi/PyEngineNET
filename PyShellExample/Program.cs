using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

engine.Exec("import sys");
engine.Exec("print(f'Python {sys.version}')");

while (true) {
	Console.Write(">>> ");
	var execCmd = Console.ReadLine();
	var result = engine.Eval(execCmd);
	if (result.IsNot(PyObject.None)) {
		Console.WriteLine(result.ToString());
	}
}