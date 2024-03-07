using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

engine.BindFunction("get_sum", (PyObject list) => {
	int accum = 0;
	for (var i = 0; i < engine.Len.Invoke(list); i++) {
		accum += list[i];
	}
	return accum;
});

int result = engine.Eval("get_sum([1, 2, 3, 4, 5])");
Console.WriteLine(result);
