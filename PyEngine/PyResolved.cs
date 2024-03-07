namespace PyEngine;

internal class PyResolved: PyObject {
	private readonly object _value;

	public PyResolved(Engine? engine, object value): base(engine) {
		_value = value;
	}

	public override void Dispose() { }

	internal override PyObject evaluate() => this;
	internal override string getExpression() => throw new NotImplementedException();
}
