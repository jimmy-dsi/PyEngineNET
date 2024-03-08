using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

engine.BindFunction("get_sum", (IEnumerable<object> list) => {
	int accum = 0;
	foreach (var item in list) {
		accum += (byte) item;
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

List<object> result3 = engine.Eval("[1, 2, {-7.6: {'Bob', False}}]");
Console.WriteLine(Engine.PyExpression(result3));