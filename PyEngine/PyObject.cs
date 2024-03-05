namespace PyEngine;

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
	public PyObject Result => evaluate();

	internal PyObject(Engine? engine) {
		_engine = engine;
		_disposable = (_engine != null);
		_accessor = new Accessor(this);
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
		throw new NotImplementedException();
	}

	public virtual T ConvertTo<T>() {
		throw new NotImplementedException();
	}

	// Implicit Conversions
	public static implicit operator bool                       (PyObject p) => throw new NotImplementedException();
	public static implicit operator int                        (PyObject p) => throw new NotImplementedException();
	public static implicit operator long                       (PyObject p) => throw new NotImplementedException();
	public static implicit operator float                      (PyObject p) => throw new NotImplementedException();
	public static implicit operator double                     (PyObject p) => throw new NotImplementedException();
	public static implicit operator decimal                    (PyObject p) => throw new NotImplementedException();
	public static implicit operator string                     (PyObject p) => throw new NotImplementedException();
	public static implicit operator List<object>               (PyObject p) => throw new NotImplementedException();
	public static implicit operator object[]                   (PyObject p) => throw new NotImplementedException();
	public static implicit operator HashSet<object>            (PyObject p) => throw new NotImplementedException();
	public static implicit operator Dictionary<object, object> (PyObject p) => throw new NotImplementedException();

	public static implicit operator PyObject (bool                       b) => throw new NotImplementedException();
	public static implicit operator PyObject (int                        i) => throw new NotImplementedException();
	public static implicit operator PyObject (long                       l) => throw new NotImplementedException();
	public static implicit operator PyObject (float                      f) => throw new NotImplementedException();
	public static implicit operator PyObject (double                     d) => throw new NotImplementedException();
	public static implicit operator PyObject (decimal                    d) => throw new NotImplementedException();
	public static implicit operator PyObject (string                     s) => throw new NotImplementedException();
	public static implicit operator PyObject (List<object>               l) => throw new NotImplementedException();
	public static implicit operator PyObject (object[]                   a) => throw new NotImplementedException();
	public static implicit operator PyObject (HashSet<object>            s) => throw new NotImplementedException();
	public static implicit operator PyObject (Dictionary<object, object> d) => throw new NotImplementedException();

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

	public override int GetHashCode() {
		throw new NotImplementedException();
	}

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
