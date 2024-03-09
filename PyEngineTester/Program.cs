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

PyObject test = result3_2_v;
HashSet<object> test2 = test;
Console.WriteLine(Engine.PyExpression(test2));

PyObject test3 = result3_2;
Dictionary<object, object> test4 = test3;
Console.WriteLine(Engine.PyExpression(test4));

PyObject test5 = result3;
object[] test6 = test5;
Console.WriteLine(Engine.PyExpression(test6));

engine.Exec("x = True");
bool result4 = engine.Eval("x");
Console.WriteLine(result4);

var result5 = (PyObject[]) engine.Eval("(1, 'string', [10, False, b\"ab\"])");
Console.WriteLine(Engine.PyExpression(result5));

PyObject test7 = result5;
List<object> test8 = test7;
Console.WriteLine(Engine.PyExpression(test8));

List<byte> result6 = engine.Eval("bytearray([0, 255, 1, 6, 8])");
Console.WriteLine(Engine.PyExpression(result6));

PyObject test9 = result6;
byte[] test10 = test9;
Console.WriteLine(Engine.PyExpression(test10));

engine.Exec("from dataclasses import dataclass, fields");
engine.Exec("@dataclass \n"
          + "class MyDataClass: \n"
          + "    foo: int \n"
          + "    zzz: bool \n"
          + "    bar: str \n" );
object[] fields = engine.Eval("[[x.name, str(x.type)] for x in fields(MyDataClass)]");
Console.WriteLine(Engine.PyExpression(fields));

engine.Exec("import datetime");
string name0 = engine.Eval("f'{MyDataClass.__module__}.{MyDataClass.__name__}'");
string name1 = engine.Eval("f'{datetime.datetime.__module__}.{datetime.datetime.__name__}'");
Console.WriteLine(Engine.PyExpression(name0));
Console.WriteLine(Engine.PyExpression(name1));

DataClassObject dataclass = engine.Eval("MyDataClass(foo= 7, zzz= False, bar= {'Hello', 'there!'})");
Console.WriteLine(Engine.PyExpression(dataclass));

PyObject dc = dataclass;
Console.WriteLine(Engine.PyExpression(dc));