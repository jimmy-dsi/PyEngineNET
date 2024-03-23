namespace PyEngine;

internal class PyResolved: PyObject {
	private readonly object? _value;
	internal object? Value => _value;

	public PyResolved(Engine? engine, object? value): base(engine) {
		_value = deserialize(value);
	}

	public override int GetHashCode() {
		return _value?.GetHashCode() ?? 0;
	}

	public override void Dispose() { }

	internal override PyObject evaluate() => this;
	internal override PyObject lazyEvaluate() => this;
	internal override string getExpression() {
		return Engine.PyExpression(_value);
	}

	//
	private object? deserialize(object? value) {
		if (value is null) {
			return value;
		} else if (value is Dictionary<object, object>) {
			var v = (Dictionary<object, object>) value!;
			if (v.ContainsKey("___type")) {
				if (v.ContainsKey("___set")) {
					return ((object[]) deserialize(v["___set"])!).ToHashSet();
				} else {
					var className  = (string) v["___type"];
					var properties = (object[]) deserialize(v["___data"])!;
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
				var dict = new Dictionary<object, object?>();
				foreach (var item in v) {
					dict[deserialize(item.Key)!] = deserialize(item.Value);
				}
				return dict;
			}
		} else if (value is List<object?>) {
			var v = (List<object?>) value;
			var list = new List<object?>();
			foreach (var item in v) {
				list.Add(deserialize(item));
			}
			return list.ToArray();
		} else if (value is object?[]) {
			var v = (object?[]) value;
			var list = new List<object?>();
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
		if (_value is null) {
			return typeof(T).IsNullableType() ? default(T) : throw new NullReferenceException();
		}

		if (typeof(T).IsIntegerType() && _value!.GetType().IsIntegerType()) {
			// Handles: byte, sbyte, ushort, short, uint, int, ulong, long
			return _value.AsIntType<T>();
		} else if (typeof(T).IsDecimalType() && _value!.GetType().IsNumericType()) {
			// Handles: float, double, decimal
			return _value.AsDecimalType<T>();
		} else if (typeof(T).IsListType<object>() && _value!.GetType().IsListType<object>()) {
			// Handles: object[], List<object>
			return _value.AsListType<T, object>();
		} else if (typeof(T).IsListType<PyObject>() && _value!.GetType().IsListType<object>()) {
			// Handles: PyObject[], List<PyObject>
			var arr = _value.AsListType<object[], object>();
			var result = new PyObject[arr.Length];

			for (var i = 0; i < arr.Length; i++) {
				result[i] = new PyResolved(engine, arr[i]);
			}

			return result.AsListType<T, PyObject>();
		} else if (typeof(T) == typeof(HashSet<PyObject>) && _value!.GetType() == typeof(HashSet<object>)) {
			// Handles: HashSet<PyObject>
			var set = (HashSet<object>) _value;
			var result = new HashSet<PyObject>();

			foreach (var item in set) {
				result.Add(new PyResolved(engine, item));
			}

			return (T) (object) result;
		} else if (typeof(T) == typeof(Dictionary<PyObject, PyObject>) && _value!.GetType() == typeof(Dictionary<object, object>)) {
			// Handles: Dictionary<PyObject, PyObject>
			var dict = (Dictionary<object, object>) _value;
			var result = new Dictionary<PyObject, PyObject>();

			foreach (var item in dict) {
				result[new PyResolved(engine, item.Key)] = new PyResolved(engine, item.Value);
			}

			return (T) (object) result;
		} else if (typeof(T).IsListType<byte>() && _value!.GetType().IsListType<byte>()) {
			// Handles: byte[], List<byte>
			var arr = _value.AsListType<byte[], byte>();
			var result = new byte[arr.Length];

			for (var i = 0; i < arr.Length; i++) {
				result[i] = new PyResolved(engine, arr[i]);
			}

			return result.AsListType<T, byte>();
		} else if (typeof(T).IsConvArrayType() && _value!.GetType().IsListType<object>()) { 
			if      (typeof(T) == typeof(sbyte[]))    return (T) (object) convertToArray<sbyte>();
			else if (typeof(T) == typeof(ushort[]))   return (T) (object) convertToArray<ushort>();
			else if (typeof(T) == typeof(short[]))    return (T) (object) convertToArray<short>();
			else if (typeof(T) == typeof(uint[]))     return (T) (object) convertToArray<uint>();
			else if (typeof(T) == typeof(int[]))      return (T) (object) convertToArray<int>();
			else if (typeof(T) == typeof(ulong[]))    return (T) (object) convertToArray<ulong>();
			else if (typeof(T) == typeof(long[]))     return (T) (object) convertToArray<long>();
			else if (typeof(T) == typeof(float[]))    return (T) (object) convertToArray<float>();
			else if (typeof(T) == typeof(double[]))   return (T) (object) convertToArray<double>();
			else if (typeof(T) == typeof(decimal[]))  return (T) (object) convertToArray<decimal>();
			else if (typeof(T) == typeof(string[]))   return (T) (object) convertToArray<string>();
			else /*if (typeof(T) == typeof(bool[]))*/ return (T) (object) convertToArray<bool>();
		} else if (typeof(T).IsConvListType() && _value!.GetType().IsListType<object>()) { 
			if      (typeof(T) == typeof(List<sbyte>))    return (T) (object) convertToList<sbyte>();
			else if (typeof(T) == typeof(List<ushort>))   return (T) (object) convertToList<ushort>();
			else if (typeof(T) == typeof(List<short>))    return (T) (object) convertToList<short>();
			else if (typeof(T) == typeof(List<uint>))     return (T) (object) convertToList<uint>();
			else if (typeof(T) == typeof(List<int>))      return (T) (object) convertToList<int>();
			else if (typeof(T) == typeof(List<ulong>))    return (T) (object) convertToList<ulong>();
			else if (typeof(T) == typeof(List<long>))     return (T) (object) convertToList<long>();
			else if (typeof(T) == typeof(List<float>))    return (T) (object) convertToList<float>();
			else if (typeof(T) == typeof(List<double>))   return (T) (object) convertToList<double>();
			else if (typeof(T) == typeof(List<decimal>))  return (T) (object) convertToList<decimal>();
			else if (typeof(T) == typeof(List<string>))   return (T) (object) convertToList<string>();
			else /*if (typeof(T) == typeof(List<bool>))*/ return (T) (object) convertToList<bool>();
		} else if (typeof(T).IsConvSetType() && _value is HashSet<object>) { 
			if      (typeof(T) == typeof(HashSet<byte>))     return (T) (object) convertToSet<byte>();
			else if (typeof(T) == typeof(HashSet<sbyte>))    return (T) (object) convertToSet<sbyte>();
			else if (typeof(T) == typeof(HashSet<ushort>))   return (T) (object) convertToSet<ushort>();
			else if (typeof(T) == typeof(HashSet<short>))    return (T) (object) convertToSet<short>();
			else if (typeof(T) == typeof(HashSet<uint>))     return (T) (object) convertToSet<uint>();
			else if (typeof(T) == typeof(HashSet<int>))      return (T) (object) convertToSet<int>();
			else if (typeof(T) == typeof(HashSet<ulong>))    return (T) (object) convertToSet<ulong>();
			else if (typeof(T) == typeof(HashSet<long>))     return (T) (object) convertToSet<long>();
			else if (typeof(T) == typeof(HashSet<float>))    return (T) (object) convertToSet<float>();
			else if (typeof(T) == typeof(HashSet<double>))   return (T) (object) convertToSet<double>();
			else if (typeof(T) == typeof(HashSet<decimal>))  return (T) (object) convertToSet<decimal>();
			else if (typeof(T) == typeof(HashSet<string>))   return (T) (object) convertToSet<string>();
			else /*if (typeof(T) == typeof(HashSet<bool>))*/ return (T) (object) convertToSet<bool>();
		} else if (typeof(T) == _value!.GetType()) {
			// Handles: bool, string, HashSet<object>, Dictionary<object, object>, DataClassObject
			return (T) _value;
		} else {
			throw new InvalidCastException($"Cannot cast PyObject to type {typeof(T)}");
		}
	}

	protected override T[] convertToArray<T>() {
		var vtype = _value!.GetType();
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
		var vtype = _value!.GetType();
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

	protected override HashSet<T> convertToSet<T>() {
		var vtype = _value!.GetType();
		if (_value is HashSet<object> set) {
			return set.Select(x => {
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
			}).ToHashSet();
		} else {
			throw new InvalidCastException($"Cannot cast PyObject to type {typeof(HashSet<T>)}");
		}
	}
	
	internal override void AssignKeyValue(PyObject key, PyObject value) {
		if (_value!.GetType() == typeof(List<object?>)) {
			var v = (List<object?>) _value;
			v[key.Result] = ((PyResolved) value.Result).Value;
		} else if (_value.GetType() == typeof(Dictionary<object, object?>)) {
			var v = (Dictionary<object, object?>) _value;
			v[key.Result] = ((PyResolved) value.Result).Value;
		} else {
			throw new InvalidOperationException($"Cannot perform index assignment on resolved non-list, non-dictionary PyObjects.");
		}
	}
}
