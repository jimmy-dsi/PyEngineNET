namespace PyEngine;

internal class PyResolved: PyObject {
	private readonly object _value;

	public PyResolved(Engine? engine, object value): base(engine) {
		_value = value;
	}

	public override void Dispose() { }

	internal override PyObject evaluate() => this;
	internal override string getExpression() {
		return Engine.PyExpression(_value);
	}

	// Conversion
	protected override T convertTo<T>() {
		if (typeof(T).IsIntegerType() && _value.GetType().IsIntegerType()) {
			return _value.AsIntType<T>();
		} else if (typeof(T) == _value.GetType()) {
			return (T) _value;
		} else {
			throw new InvalidCastException($"Cannot cast PyObject to type {typeof(T)}");
		}
	}
}
