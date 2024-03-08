namespace PyEngine;

internal class PyResolved: PyObject {
	private readonly object _value;

	public PyResolved(Engine? engine, object value): base(engine) {
		_value = loadObject(value);
	}

	public override void Dispose() { }

	internal override PyObject evaluate() => this;
	internal override string getExpression() {
		return Engine.PyExpression(_value);
	}

	//
	private object loadObject(object value) {
		if (value is Dictionary<object, object>) {
			var v = (Dictionary<object, object>) value;
			if (v.ContainsKey("___type")) {
				if (v.ContainsKey("___set")) {
					return ((object[]) loadObject(v["___set"])).ToHashSet();
				} else {
					// TODO: Convert to "Data class" object
					throw new NotImplementedException();
				}
			} else {
				var dict = new Dictionary<object, object>();
				foreach (var item in v) {
					dict[loadObject(item.Key)] = loadObject(item.Value);
				}
				return dict;
			}
		} else if (value is object[]) {
			var v = (object[]) value;
			var list = new List<object>();
			foreach (var item in v) {
				list.Add(loadObject(item));
			}
			return list.ToArray();
		} else {
			return value;
		}
	}

	// Conversion
	protected override T convertTo<T>() {
		if (typeof(T).IsIntegerType() && _value.GetType().IsIntegerType()) {
			// Handles: byte, sbyte, ushort, short, uint, int, ulong, long
			return _value.AsIntType<T>();
		} else if (typeof(T).IsDecimalType() && _value.GetType().IsNumericType()) {
			// Handles: float, double, decimal
			return _value.AsDecimalType<T>();
		} else if (typeof(T).IsListType() && _value.GetType().IsListType()) {
			// Handles: object[], List<object>, IEnumerable<object>
			return _value.AsListType<T>();
		} else if (typeof(T) == _value.GetType()) {
			// Handles: bool
			return (T) _value;
		} else {
			//Console.WriteLine(_value.GetType());
			throw new InvalidCastException($"Cannot cast PyObject to type {typeof(T)}");
		}
	}
}
