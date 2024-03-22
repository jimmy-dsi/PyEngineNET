using PyEngine;
using PyEngineTester;

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
Console.WriteLine(Engine.PyExpression(dataclass["foo"]));
Console.WriteLine(Engine.PyExpression(dataclass["zzz"]));
Console.WriteLine(Engine.PyExpression(dataclass["bar"]));
dataclass["bar"] = new HashSet<object> { "Hello", "World!" };

PyObject dc = dataclass;
Console.WriteLine(Engine.PyExpression(dc));

engine.Exec("""
@dataclass
class MyOtherDataClass: 
	test: int 
	thing: bool 

	def __add__(self, other):
		return MyOtherDataClass(test= self.test + other, thing= self.thing)
""");
DataClassObject dataclass2 = engine.Eval("MyDataClass(foo= 7, zzz= False, bar= MyOtherDataClass(test = 22, thing = True))");
Console.WriteLine(Engine.PyExpression(dataclass2));

var dco = engine.Eval("MyOtherDataClass(10, True)", eager: true);
dco = (dco + 18).Result;
dco.Attr["thing"] = false;
Console.WriteLine(Engine.PyExpression(dco));

var testNum = engine.Eval("12.5", eager: true);
var py = testNum.Attr["__round__"].Invoke();
Console.WriteLine(Engine.PyExpression(py));

var pyLeft  = engine.Eval("12");
var pyRight = engine.Eval("20");

if (pyLeft == pyRight) {
	Console.WriteLine("No");
}
if (pyLeft != pyRight) {
	Console.WriteLine("Yes");
}
if (pyLeft < pyRight) {
	Console.WriteLine("Yes");
}
if (pyLeft <= pyRight) {
	Console.WriteLine("Yes");
}
if (pyLeft > pyRight) {
	Console.WriteLine("No");
}
if (pyLeft >= pyRight) {
	Console.WriteLine("No");
}

if (pyLeft == 10) {
	Console.WriteLine("No");
}
if (pyLeft != 10) {
	Console.WriteLine("No");
}
if (pyLeft < 10) {
	Console.WriteLine("No");
}
if (pyLeft <= 10) {
	Console.WriteLine("Yes");
}
if (pyLeft > 10) {
	Console.WriteLine("Yes");
}
if (pyLeft >= 10) {
	Console.WriteLine("Yes");
}

var pyEquals             = pyLeft.PyEquals(pyRight);
var pyNotEquals          = pyLeft.PyNotEquals(pyRight);
var pyLessThan           = pyLeft.PyLessThan(pyRight);
var pyLessThanOrEqual    = pyLeft.PyLessThanOrEqual(pyRight);
var pyGreaterThan        = pyLeft.PyGreaterThan(pyRight);
var pyGreaterThanOrEqual = pyLeft.PyGreaterThanOrEqual(pyRight);

if (pyEquals) {
	Console.WriteLine("No");
}
if (pyNotEquals) {
	Console.WriteLine("Yes");
}
if (pyLessThan) {
	Console.WriteLine("Yes");
}
if (pyLessThanOrEqual) {
	Console.WriteLine("Yes");
}
if (pyGreaterThan) {
	Console.WriteLine("No");
}
if (pyGreaterThanOrEqual) {
	Console.WriteLine("No");
}

{
	List<short> shorts = engine.Eval("[1, -17, 65535, 65536]");
	Console.WriteLine(Engine.PyExpression(shorts));
	ushort[] ushorts = engine.Eval("[1, -17, 65535, 65536]");
	Console.WriteLine(Engine.PyExpression(ushorts));
	PyObject pyShorts = ushorts;
	int[] ints = pyShorts;
	Console.WriteLine(Engine.PyExpression(ints));
}
{
	HashSet<short> shorts = engine.Eval("{1, -17, 65535, 65536}");
	Console.WriteLine(Engine.PyExpression(shorts));
	HashSet<ushort> ushorts = engine.Eval("{1, -17, 65535, 65536}");
	Console.WriteLine(Engine.PyExpression(ushorts));
	PyObject pyShorts = ushorts;
	HashSet<int> ints = pyShorts;
	Console.WriteLine(Engine.PyExpression(ints));
}

var pyNone = engine.Eval("None", eager: true);
Console.WriteLine(Engine.PyExpression(pyNone));

int? nullable = pyNone;
Console.WriteLine(Engine.PyExpression(nullable));
PyObject z = nullable;
Console.WriteLine(Engine.PyExpression(z));

object[] nullArray = engine.Eval("[1, 2, None, 'Hello!']");
Console.WriteLine(Engine.PyExpression(nullArray));
PyObject q = nullArray;
Console.WriteLine(Engine.PyExpression(q));

string tests = engine.Eval("None");
Console.WriteLine(Engine.PyExpression(tests));

void hello(Dictionary<string, object> test) {
	hi(test);
}

void hi(Dictionary<string, object> test) {
	engine.BindFunction("test_net_errors", (int divisor) => {
		Test.TestNetErrors(divisor);
	});

	engine.Exec("test_net_errors(0)");
}

try {
	hello(new());
} catch (PyException ex) {
	Console.WriteLine(ex);
}

try {
	engine.Exec("from pathlib import Path");
	var res = engine.Eval("Path('some/path')", eager: true);
} catch (PyException ex) {
	Console.WriteLine(ex);
}

engine.BindFunction("test_exc", (int divisor) => {
	PyObject foo = "string";
	return foo - divisor;
});

try {
	var res = engine.Eval("test_exc(0)", eager: true);
} catch (PyException ex) {
	Console.WriteLine(ex);
}

PyObject pyObject = "17.3";
pyObject = pyObject.Float().Round().Int().Str().List().Set();
Console.WriteLine(pyObject.ToString());
HashSet<string> resultSet = engine.Eval(pyObject);

//try {
//	engine.Exec("exit()");
//} catch (PythonExitedException) {
//	//engine.Exec("print('hello')");
//}

engine.Exec("""
def my_gen():
	yield 1
	yield 2
	yield 3
""");

PyObject myGen = engine.Eval("my_gen()");
foreach (var item in myGen) {
	int value = item;
	Console.WriteLine(value);
}

PyObject myList = engine.Eval("['hello', True, 7, 8, -9]");
foreach (var item in myList) {
	var value = item.Result;
	Console.WriteLine(value.ToString());
}
Console.WriteLine("Done.");

engine.BindGenerator("get_ints", (int n) => GetInts(n));

IEnumerable<PyObject> GetInts(int n) {
	Console.WriteLine("Get first int.");
	yield return n + 1;
	Console.WriteLine("Get second int.");
	yield return n + 2;
	Console.WriteLine("Get third int.");
	yield return n + 3;
	Console.WriteLine("Done.");
}

var myNetGen  = engine.Eval("get_ints(7)");
var myNetGen2 = engine.Eval("get_ints(7)");
try {
	var firstInt   = myNetGen.Next().Result;
	var secondInt  = myNetGen.Next().Result;
	var firstInt2  = myNetGen2.Next().Result;
	var secondInt2 = myNetGen2.Next().Result;
	var thirdInt   = myNetGen.Next().Result;
	var thirdInt2  = myNetGen2.Next().Result;
	var fourthInt  = myNetGen.Next().Result;
	var fourthInt2 = myNetGen2.Next().Result;
} catch (PyException ex) { 
	if (ex.PyExceptionType == "StopIteration") {
		Console.WriteLine("Done.");
	} else {
		throw;
	}
}

var myNetGen3 = engine.Eval("get_ints(7)");
foreach (int item in myNetGen3) {
	Console.WriteLine(item);
}
Console.WriteLine("Done.");

engine.BindGenerator("get_data", (int n, bool b, string s, object[] a, double d) => GetData(n, b, s, a, d));

IEnumerable<PyObject> GetData(int n, bool b, string s, object[] a, double d) {
	yield return n;
	yield return b;
	yield return s;
	yield return a;
	yield return d;
}

engine.Exec("data = get_data(7, True, 'hi', ['q', 'wert', 'y'], 3.14)");
engine.Exec("print(next(data))");
engine.Exec("print(next(data))");
engine.Exec("print(next(data))");
engine.Exec("print(next(data))");
engine.Exec("print(next(data))");
engine.Exec("print(next(data))");