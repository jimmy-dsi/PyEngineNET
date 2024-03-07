using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

engine.BindFunction("get_sum", (PyObject list, PyObject i, PyObject accum) => {
	for (; i < engine.Len.Invoke(list); i++) {
		accum += list[i];
	}
	return accum;
});

var result = engine.Eval("get_sum([1, 2, 3, 4, 5], 0, 0)");

Console.WriteLine(AppContext.BaseDirectory);
