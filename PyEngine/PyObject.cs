namespace PyEngine;

using System.Collections.Generic;
using System.Diagnostics;

public abstract class PyObject: IDisposable {
	public class Accessor {
		private readonly PyObject _pyObject;

		public Accessor(PyObject pyObject) {
			_pyObject = pyObject;
		}

		public PyObject this[string attrName] {
			get {
				return new PyAccess(_pyObject.engine, _pyObject, attrName);
			}
			set {
				// TODO: Exec Python-side code to overwrite key's value
			}
		}
	}

	private Engine? _engine;
	private readonly Accessor _accessor;

	protected Engine engine {
		get => _engine ?? Engine.Default!;
		set { _engine = value; }
	}

	private static PyObject? _none = null;

	protected readonly bool _disposable = true;

	internal static int gvarNum = 0;

	public static PyObject None {
		get {
			if (Engine.Default == null) {
				throw new InvalidOperationException("Default Engine must be set, or an Engine instance must be active in order to access PyObject.None");
			}

			if (_none is null) {
				_none = new PyProxy(null, "___pye_var___None");
			}

			return _none;
		}
	}

	public Accessor Attr   => _accessor;
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public PyObject Result => evaluate();

	internal string Expression => getExpression();

	internal PyObject(Engine? engine) {
		_engine = engine;
		_disposable = (_engine != null);
		_accessor = new Accessor(this);
	}

	public static PyObject Create(object obj, Engine? engine = null) {
		return new PyResolved(engine ?? Engine.Default, obj);
	}

	public virtual PyObject GetProp(string propName) {
		var resolved = evaluate();
		return resolved.GetProp(propName);
	}

	public virtual void SetProp(string propName, PyObject value) {
		var resolved = evaluate();
		resolved.SetProp(propName, value);
	}

	public abstract void Dispose();

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
		} else {
			throw new NotImplementedException();
		}
	}

	public virtual T ConvertTo<T>() {
		if (typeof(T) == typeof(PyObject)) {
			return (T) (object) this;
		} else {
			return convertTo<T>();
		}
	}

	protected virtual T convertTo<T>() => Result.convertTo<T>();

	// Implicit Conversions
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

	//
	internal abstract string getExpression();
	internal abstract PyObject evaluate();

	// Operations
	public PyObject FloorDiv(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyFloorDivision(engine, this, rhs);
	}

	public PyObject Pow(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyExponentiation(engine, this, rhs);
	}

	public PyObject In(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyInclusion(engine, this, rhs);
	}

	public PyObject NotIn(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyExclusion(engine, this, rhs);
	}

	public PyObject Is(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyRefEquality(engine, this, rhs);
	}

	public PyObject IsNot(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyRefInequality(engine, this, rhs);
	}

	public PyObject And(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyLogicalAnd(engine, this, rhs);
	}

	public PyObject Or(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyLogicalOr(engine, this, rhs);
	}

	public PyObject Splat() {
		return new PySplat(engine, this);
	}

	public PyObject Invoke(params PyObject[] args) {
		var engines = new List<Engine> { engine };
		engines.AddRange(args.Select(x => x.engine));
		checkEngines(engines.ToArray());

		return new PyInvokation(engine, this, args);
	}

	// Operations - Overloaded
	public static PyObject operator + (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyAddition(lhs.engine, lhs, rhs);
	}

	public static PyObject operator - (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PySubtraction(lhs.engine, lhs, rhs);
	}

	public static PyObject operator * (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyMultiplication(lhs.engine, lhs, rhs);
	}

	public static PyObject operator / (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyDivision(lhs.engine, lhs, rhs);
	}

	public static PyObject operator % (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyModulo(lhs.engine, lhs, rhs);
	}

	public static PyObject operator & (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyBitwiseAnd(lhs.engine, lhs, rhs);
	}

	public static PyObject operator | (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyBitwiseOr(lhs.engine, lhs, rhs);
	}

	public static PyObject operator ^ (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyBitwiseXor(lhs.engine, lhs, rhs);
	}

	public static PyObject operator << (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyBitshiftLeft(lhs.engine, lhs, rhs);
	}

	public static PyObject operator >> (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyBitshiftRight(lhs.engine, lhs, rhs);
	}

	public static bool operator == (PyObject lhs, object? rhs) {
		return (rhs != null);
	}

	public static bool operator != (PyObject lhs, object? rhs) {
		return (rhs == null);
	}

	public static bool operator == (object? lhs, PyObject rhs) {
		return (lhs != null);
	}

	public static bool operator != (object? lhs, PyObject rhs) {
		return (lhs == null);
	}

	public static PyObject operator == (PyObject? lhs, PyObject? rhs) {
		if (lhs is null && rhs is null) {
			return (PyObject) true;
		} else if (lhs is null || rhs is null) {
			return (PyObject) false;
		}
		checkEngines(lhs.engine, rhs.engine);
		return new PyEquality(lhs.engine, lhs, rhs);
	}

	public static PyObject operator != (PyObject? lhs, PyObject? rhs) {
		if (lhs is null && rhs is null) {
			return (PyObject) false;
		} else if (lhs is null || rhs is null) {
			return (PyObject) true;
		}
		checkEngines(lhs.engine, rhs.engine);
		return new PyInequality(lhs.engine, lhs, rhs);
	}

	public static PyObject operator < (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyLessThan(lhs.engine, lhs, rhs);
	}

	public static PyObject operator <= (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyLessThanOrEqual(lhs.engine, lhs, rhs);
	}

	public static PyObject operator > (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyGreaterThan(lhs.engine, lhs, rhs);
	}

	public static PyObject operator >= (PyObject lhs, PyObject rhs) {
		checkEngines(lhs.engine, rhs.engine);
		return new PyGreaterThanOrEqual(lhs.engine, lhs, rhs);
	}

	public static PyObject operator + (PyObject value) {
		return new PyIdentity(value.engine, value);
	}

	public static PyObject operator - (PyObject value) {
		return new PyNegation(value.engine, value);
	}

	public static PyObject operator ~ (PyObject value) {
		return new PyBitwiseNegation(value.engine, value);
	}

	public static PyObject operator ! (PyObject value) {
		return new PyLogicalNegation(value.engine, value);
	}

	public PyObject this[PyObject key] {
		get {
			checkEngines(engine, key.engine);
			return new PyKeyLookup(engine, this, key);
		}
		set {
			checkEngines(engine, key.engine);
			// TODO: Exec Python-side code to overwrite key's value
		}
	}

	public override bool Equals(object? obj) {
		if (ReferenceEquals(this, obj)) {
			return true;
		}

		if (ReferenceEquals(obj, null)) {
			return false;
		}

		if (obj is PyObject) {
			return this == obj;
		}

		throw new NotImplementedException();
	}

	public override abstract int GetHashCode();

	//
	private static void checkEngines(params Engine[] engines) {
		Engine? e = null;
		foreach (var engine in engines) {
			if (e == null) {
				e = engine;
			} else if (engine != e) {
				throw new InvalidOperationException("An operation cannot be performed on PyObjects using different Engine instances.");
			}
		}
	}
}
