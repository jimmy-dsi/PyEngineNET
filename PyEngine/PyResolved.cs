namespace PyEngine;

internal class PyResolved: PyObject {
	private readonly object _value;
	internal object Value => _value;

	public PyResolved(Engine? engine, object value): base(engine) {
		_value = deserialize(value);
	}

	public override int GetHashCode() {
		return _value.GetHashCode();
	}

	public override void Dispose() { }

	internal override PyObject evaluate() => this;
	internal override PyObject lazyEvaluate() => this;
	internal override string getExpression() {
		return Engine.PyExpression(_value);
	}

	//
	private object deserialize(object value) {
		if (value is Dictionary<object, object>) {
			var v = (Dictionary<object, object>) value;
			if (v.ContainsKey("___type")) {
				if (v.ContainsKey("___set")) {
					return ((object[]) deserialize(v["___set"])).ToHashSet();
				} else {
					var className  = (string) v["___type"];
					var properties = (object[]) deserialize(v["___data"]);
					var propDict   = new Dictionary<string, object>();
					var propNames  = new string[properties.Length];

					for (var i = 0; i < properties.Length; i++) {
						var item = (object[]) properties[i];
						var propName = (string) item[0];
						propNames[i] = propName;
						propDict[propName] = item[1];
					}

					return new DataClassObject(className, propNames, propDict);
				}
			} else {
				var dict = new Dictionary<object, object>();
				foreach (var item in v) {
					dict[deserialize(item.Key)] = deserialize(item.Value);
				}
				return dict;
			}
		} else if (value is List<object>) {
			var v = (List<object>) value;
			var list = new List<object>();
			foreach (var item in v) {
				list.Add(deserialize(item));
			}
			return list.ToArray();
		} else if (value is object[]) {
			var v = (object[]) value;
			var list = new List<object>();
			foreach (var item in v) {
				list.Add(deserialize(item));
			}
			return list.ToArray();
		} else if (value is PyResolved v) {
			return deserialize(v._value);		
		} else if (value is PyObject) {
			return deserialize(((PyResolved) ((PyObject) value).Result)._value);		
		} else if (value.GetType().IsNumericType() || value is DataClassObject || value is bool || value is string || value is byte[] || value is List<byte> || value is HashSet<object>) {
			return value;
		} else {
			throw new InvalidOperationException($"Cannot convert object of type {value.GetType()} to PyObject.");
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
		} else if (typeof(T).IsListType<object>() && _value.GetType().IsListType<object>()) {
			// Handles: object[], List<object>
			return _value.AsListType<T, object>();
		} else if (typeof(T).IsListType<PyObject>() && _value.GetType().IsListType<object>()) {
			// Handles: PyObject[], List<PyObject>
			var arr = _value.AsListType<object[], object>();
			var result = new PyObject[arr.Length];

			for (var i = 0; i < arr.Length; i++) {
				result[i] = new PyResolved(engine, arr[i]);
			}

			return result.AsListType<T, PyObject>();
		} else if (typeof(T) == typeof(HashSet<PyObject>) && _value.GetType() == typeof(HashSet<object>)) {
			// Handles: HashSet<PyObject>
			var set = (HashSet<object>) _value;
			var result = new HashSet<PyObject>();

			foreach (var item in set) {
				result.Add(new PyResolved(engine, item));
			}

			return (T) (object) result;
		} else if (typeof(T) == typeof(Dictionary<PyObject, PyObject>) && _value.GetType() == typeof(Dictionary<object, object>)) {
			// Handles: Dictionary<PyObject, PyObject>
			var dict = (Dictionary<object, object>) _value;
			var result = new Dictionary<PyObject, PyObject>();

			foreach (var item in dict) {
				result[new PyResolved(engine, item.Key)] = new PyResolved(engine, item.Value);
			}

			return (T) (object) result;
		} else if (typeof(T).IsListType<byte>() && _value.GetType().IsListType<byte>()) {
			// Handles: byte[], List<byte>
			var arr = _value.AsListType<byte[], byte>();
			var result = new byte[arr.Length];

			for (var i = 0; i < arr.Length; i++) {
				result[i] = new PyResolved(engine, arr[i]);
			}

			return result.AsListType<T, byte>();
		} else if (typeof(T) == _value.GetType()) {
			// Handles: bool, string, HashSet<object>, Dictionary<object, object>, DataClassObject
			return (T) _value;
		} else {
			throw new InvalidCastException($"Cannot cast PyObject to type {typeof(T)}");
		}
	}

	protected override T[] convertToArray<T>() {
		var vtype = _value.GetType();
		if (vtype.IsListType<object>()) {
			var arr = _value.AsListType<object[], object>();
			return arr.Select(x => {
				var xtype = x.GetType();
				if (xtype.IsNumericType()) {
					if (typeof(T).IsIntegerType()) {
						return x.AsIntType<T>();
					} else {
						return x.AsDecimalType<T>();
					}
				} else if (xtype == typeof(T)) {
					return (T) x;
				} else {
					throw new InvalidCastException($"Cannot cast element of PyObject to type {typeof(T)}");
				}
			}).ToArray();
		} else {
			throw new InvalidCastException($"Cannot cast PyObject to type {typeof(T[])}");
		}
	}

	protected override List<T> convertToList<T>() {
		var vtype = _value.GetType();
		if (vtype.IsListType<object>()) {
			var arr = _value.AsListType<List<object>, object>();
			return arr.Select(x => {
				var xtype = x.GetType();
				if (xtype.IsNumericType()) {
					if (typeof(T).IsIntegerType()) {
						return x.AsIntType<T>();
					} else {
						return x.AsDecimalType<T>();
					}
				} else if (xtype == typeof(T)) {
					return (T) x;
				} else {
					throw new InvalidCastException($"Cannot cast element of PyObject to type {typeof(T)}");
				}
			}).ToList();
		} else {
			throw new InvalidCastException($"Cannot cast PyObject to type {typeof(List<T>)}");
		}
	}
	
	internal override void AssignKeyValue(PyObject key, PyObject value) {
		if (_value.GetType() == typeof(List<object>)) {
			var v = (List<object>) _value;
			v[key.Result] = ((PyResolved) value.Result).Value;
		} else if (_value.GetType() == typeof(Dictionary<object, object>)) {
			var v = (Dictionary<object, object>) _value;
			v[key.Result] = ((PyResolved) value.Result).Value;
		} else {
			throw new InvalidOperationException($"Cannot perform index assignment on resolved non-list, non-dictionary PyObjects.");
		}
	}
}
