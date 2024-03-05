using PyEngine;

var engine = new Engine("python", "pyengine.py");
engine.Start();

Console.WriteLine(AppContext.BaseDirectory);