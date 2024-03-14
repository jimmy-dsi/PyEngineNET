namespace PyEngine;

using System.Collections;

public abstract partial class PyObject: IDisposable {
	// Conversions
	public static PyObject ConvertFrom<T>(T value) {
		if (value is PyObject) {
			return (value as PyObject)!;
		} else if (typeof(T).IsNumericType()) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(string)) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(List<byte>)) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(List<object>)) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(List<PyObject>)) {
			var list = new List<object>();
			foreach (var item in (List<PyObject>) (object) value!) {
				list.Add(((PyResolved) item.Result).Value);
			}
			return new PyResolved(Engine.Default, list);
		} else if (typeof(T) == typeof(byte[])) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(object[])) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(PyObject[])) {
			var list = new List<object>();
			foreach (var item in (PyObject[]) (object) value!) {
				list.Add(((PyResolved) item.Result).Value);
			}
			return new PyResolved(Engine.Default, list.ToArray());
		} else if (typeof(T) == typeof(HashSet<object>)) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(HashSet<PyObject>)) {
			var set = new HashSet<object>();
			foreach (var item in (HashSet<PyObject>) (object) value!) {
				set.Add(((PyResolved) item.Result).Value);
			}
			return new PyResolved(Engine.Default, set);
		} else if (typeof(T) == typeof(Dictionary<object, object>)) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(Dictionary<PyObject, PyObject>)) {
			var dict = new Dictionary<object, object>();
			foreach (var item in (Dictionary<PyObject, PyObject>) (object) value!) {
				dict[((PyResolved) item.Key.Result).Value] = ((PyResolved) item.Value.Result).Value;
			}
			return new PyResolved(Engine.Default, dict);
		} else if (typeof(T) == typeof(bool)) {
			return new PyResolved(Engine.Default, value!);
		} else if (typeof(T) == typeof(DataClassObject)) {
			return new PyResolved(Engine.Default, value!);
		} else if (value is IEnumerable) {
			var v = (IEnumerable) value;
			return new PyResolved(Engine.Default, v.ToEnum().ToArray());
		} else {
			throw new InvalidOperationException($"Cannot convert object of type {value!.GetType()} to PyObject.");
		}
	}

	internal virtual T ConvertTo<T>() {
		if (typeof(T) == typeof(PyObject)) {
			return (T) (object) this;
		} else {
			return convertTo<T>();
		}
	}

	protected virtual T       convertTo<T>()      => Result.convertTo<T>();
	protected virtual T[]     convertToArray<T>() => Result.convertToArray<T>();
	protected virtual List<T> convertToList<T>()  => Result.convertToList<T>();

	// Implicit Conversions (To)
	public static implicit operator bool                           (PyObject p) => p.convertTo<bool>();
	public static implicit operator byte                           (PyObject p) => p.convertTo<byte>();
	public static implicit operator sbyte                          (PyObject p) => p.convertTo<sbyte>();
	public static implicit operator ushort                         (PyObject p) => p.convertTo<ushort>();
	public static implicit operator short                          (PyObject p) => p.convertTo<short>();
	public static implicit operator uint                           (PyObject p) => p.convertTo<uint>();
	public static implicit operator int                            (PyObject p) => p.convertTo<int>();
	public static implicit operator ulong                          (PyObject p) => p.convertTo<ulong>();
	public static implicit operator long                           (PyObject p) => p.convertTo<long>();
	public static implicit operator float                          (PyObject p) => p.convertTo<float>();
	public static implicit operator double                         (PyObject p) => p.convertTo<double>();
	public static implicit operator decimal                        (PyObject p) => p.convertTo<decimal>();
	public static implicit operator string                         (PyObject p) => p.convertTo<string>();
	public static implicit operator byte[]                         (PyObject p) => p.convertTo<byte[]>();
	public static implicit operator List<byte>                     (PyObject p) => p.convertTo<List<byte>>();
	public static implicit operator List<object>                   (PyObject p) => p.convertTo<List<object>>();
	public static implicit operator List<PyObject>                 (PyObject p) => p.convertTo<List<PyObject>>();
	public static implicit operator object[]                       (PyObject p) => p.convertTo<object[]>();
	public static explicit operator PyObject[]                     (PyObject p) => p.convertTo<PyObject[]>();
	public static implicit operator HashSet<object>                (PyObject p) => p.convertTo<HashSet<object>>();
	public static implicit operator HashSet<PyObject>              (PyObject p) => p.convertTo<HashSet<PyObject>>();
	public static implicit operator Dictionary<object, object>     (PyObject p) => p.convertTo<Dictionary<object, object>>();
	public static implicit operator Dictionary<PyObject, PyObject> (PyObject p) => p.convertTo<Dictionary<PyObject, PyObject>>();
	public static implicit operator DataClassObject                (PyObject p) => p.convertTo<DataClassObject>();
	// Containers of primitive types
	public static implicit operator bool[]                         (PyObject p) => p.convertToArray<bool>();
	public static implicit operator sbyte[]                        (PyObject p) => p.convertToArray<sbyte>();
	public static implicit operator ushort[]                       (PyObject p) => p.convertToArray<ushort>();
	public static implicit operator short[]                        (PyObject p) => p.convertToArray<short>();
	public static implicit operator uint[]                         (PyObject p) => p.convertToArray<uint>();
	public static implicit operator int[]                          (PyObject p) => p.convertToArray<int>();
	public static implicit operator ulong[]                        (PyObject p) => p.convertToArray<ulong>();
	public static implicit operator long[]                         (PyObject p) => p.convertToArray<long>();
	public static implicit operator float[]                        (PyObject p) => p.convertToArray<float>();
	public static implicit operator double[]                       (PyObject p) => p.convertToArray<double>();
	public static implicit operator decimal[]                      (PyObject p) => p.convertToArray<decimal>();
	public static implicit operator string[]                       (PyObject p) => p.convertToArray<string>();
	//
	public static implicit operator List<bool>                     (PyObject p) => p.convertToList<bool>();
	public static implicit operator List<sbyte>                    (PyObject p) => p.convertToList<sbyte>();
	public static implicit operator List<ushort>                   (PyObject p) => p.convertToList<ushort>();
	public static implicit operator List<short>                    (PyObject p) => p.convertToList<short>();
	public static implicit operator List<uint>                     (PyObject p) => p.convertToList<uint>();
	public static implicit operator List<int>                      (PyObject p) => p.convertToList<int>();
	public static implicit operator List<ulong>                    (PyObject p) => p.convertToList<ulong>();
	public static implicit operator List<long>                     (PyObject p) => p.convertToList<long>();
	public static implicit operator List<float>                    (PyObject p) => p.convertToList<float>();
	public static implicit operator List<double>                   (PyObject p) => p.convertToList<double>();
	public static implicit operator List<decimal>                  (PyObject p) => p.convertToList<decimal>();
	public static implicit operator List<string>                   (PyObject p) => p.convertToList<string>();
	
	// Implicit Conversions (From)
	public static implicit operator PyObject (bool                           o) => ConvertFrom(o);
	public static implicit operator PyObject (byte                           o) => ConvertFrom(o);
	public static implicit operator PyObject (sbyte                          o) => ConvertFrom(o);
	public static implicit operator PyObject (ushort                         o) => ConvertFrom(o);
	public static implicit operator PyObject (short                          o) => ConvertFrom(o);
	public static implicit operator PyObject (uint                           o) => ConvertFrom(o);
	public static implicit operator PyObject (int                            o) => ConvertFrom(o);
	public static implicit operator PyObject (ulong                          o) => ConvertFrom(o);
	public static implicit operator PyObject (long                           o) => ConvertFrom(o);
	public static implicit operator PyObject (float                          o) => ConvertFrom(o);
	public static implicit operator PyObject (double                         o) => ConvertFrom(o);
	public static implicit operator PyObject (decimal                        o) => ConvertFrom(o);
	public static implicit operator PyObject (string                         o) => ConvertFrom(o);
	public static implicit operator PyObject (List<byte>                     o) => ConvertFrom(o);
	public static implicit operator PyObject (List<object>                   o) => ConvertFrom(o);
	public static implicit operator PyObject (List<PyObject>                 o) => ConvertFrom(o);
	public static implicit operator PyObject (byte[]                         o) => ConvertFrom(o);
	public static implicit operator PyObject (object[]                       o) => ConvertFrom(o);
	public static explicit operator PyObject (PyObject[]                     o) => ConvertFrom(o);
	public static implicit operator PyObject (HashSet<object>                o) => ConvertFrom(o);
	public static implicit operator PyObject (HashSet<PyObject>              o) => ConvertFrom(o);
	public static implicit operator PyObject (Dictionary<object, object>     o) => ConvertFrom(o);
	public static implicit operator PyObject (Dictionary<PyObject, PyObject> o) => ConvertFrom(o);
	public static implicit operator PyObject (DataClassObject                o) => ConvertFrom(o);
	// Containers of primitive types
	public static implicit operator PyObject (bool[]                         o) => ConvertFrom(o);
	public static implicit operator PyObject (sbyte[]                        o) => ConvertFrom(o);
	public static implicit operator PyObject (ushort[]                       o) => ConvertFrom(o);
	public static implicit operator PyObject (short[]                        o) => ConvertFrom(o);
	public static implicit operator PyObject (uint[]                         o) => ConvertFrom(o);
	public static implicit operator PyObject (int[]                          o) => ConvertFrom(o);
	public static implicit operator PyObject (ulong[]                        o) => ConvertFrom(o);
	public static implicit operator PyObject (long[]                         o) => ConvertFrom(o);
	public static implicit operator PyObject (float[]                        o) => ConvertFrom(o);
	public static implicit operator PyObject (double[]                       o) => ConvertFrom(o);
	public static implicit operator PyObject (decimal[]                      o) => ConvertFrom(o);
	public static implicit operator PyObject (string[]                       o) => ConvertFrom(o);
	//
	public static implicit operator PyObject (List<bool>                     o) => ConvertFrom(o);
	public static implicit operator PyObject (List<sbyte>                    o) => ConvertFrom(o);
	public static implicit operator PyObject (List<ushort>                   o) => ConvertFrom(o);
	public static implicit operator PyObject (List<short>                    o) => ConvertFrom(o);
	public static implicit operator PyObject (List<uint>                     o) => ConvertFrom(o);
	public static implicit operator PyObject (List<int>                      o) => ConvertFrom(o);
	public static implicit operator PyObject (List<ulong>                    o) => ConvertFrom(o);
	public static implicit operator PyObject (List<long>                     o) => ConvertFrom(o);
	public static implicit operator PyObject (List<float>                    o) => ConvertFrom(o);
	public static implicit operator PyObject (List<double>                   o) => ConvertFrom(o);
	public static implicit operator PyObject (List<decimal>                  o) => ConvertFrom(o);
	public static implicit operator PyObject (List<string>                   o) => ConvertFrom(o);
}
