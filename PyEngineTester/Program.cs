using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

engine.BindFunction("get_sum", (PyObject[] list) => {
	int accum = 0;
	foreach (var item in list) {
		accum += (int) item;
	}
	return accum;
});

int result = engine.Eval("get_sum([1, 2, 3, 4, 5])");
Console.WriteLine(result);

engine.BindFunction("add_point_5", (double n) => {
	return n + 0.5;
});

double result2 = engine.Eval("add_point_5(7)");
Console.WriteLine(result2);

List<PyObject> result3 = engine.Eval("[1, 2, {-7.6: {'Bob', False}}]");
Console.WriteLine(Engine.PyExpression(result3));
Dictionary<PyObject, PyObject> result3_2 = result3[2];
Console.WriteLine(Engine.PyExpression(result3_2));
HashSet<PyObject> result3_2_v = result3_2[-7.6];
Console.WriteLine(Engine.PyExpression(result3_2_v));

engine.Exec("x = True");
bool result4 = engine.Eval("x");
Console.WriteLine(result4);

object[] result5 = engine.Eval("(1, 'string', [10, False])");
Console.WriteLine(Engine.PyExpression(result5));