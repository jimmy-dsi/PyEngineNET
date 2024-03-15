namespace PyEngine;

using System.Collections.Generic;
using System.Diagnostics;

public abstract partial class PyObject: IDisposable {
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
				if (_pyObject is PyProxy) {
					var py = (PyProxy) _pyObject;
					py.checkPyKey();
					py.engine.Exec($"{py.pyGVarName}.{attrName} = {value.Expression}");
				} else if (_pyObject is PyResolved) {
					var py = (PyResolved) _pyObject;
					if (py.Value is DataClassObject) {
						var dco = (DataClassObject) py.Value;
						dco[attrName] = value;
					} else {
						throw new InvalidOperationException($"Cannot assign attributes on resolved non-dataclass PyObjects.");
					}
				} else {
					throw new InvalidOperationException($"PyObject representing a Python operation must be evaluated before performing attribute assignment.");
				}
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

	public Accessor Attr => _accessor;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public PyObject Result => evaluate();
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public PyObject LazyResult => lazyEvaluate();

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
		return Attr[propName].Result;
	}

	public virtual void SetProp(string propName, PyObject value) {
		Attr[propName] = value;
	}

	public abstract void Dispose();

	//
	internal abstract string getExpression();
	internal abstract PyObject evaluate();
	internal abstract PyObject lazyEvaluate();

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

	public PyObject PyEquals(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyEquality(engine, this, rhs);
	}

	public PyObject PyNotEquals(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyInequality(engine, this, rhs);
	}

	public PyObject PyLessThan(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyLessThan(engine, this, rhs);
	}

	public PyObject PyLessThanOrEqual(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyLessThanOrEqual(engine, this, rhs);
	}

	public PyObject PyGreaterThan(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyGreaterThan(engine, this, rhs);
	}

	public PyObject PyGreaterThanOrEqual(PyObject rhs) {
		checkEngines(engine, rhs.engine);
		return new PyGreaterThanOrEqual(engine, this, rhs);
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

	public static bool operator == (object? lhs, PyObject? rhs) {
		if (lhs is null && rhs is null) {
			return true;
		} else if (lhs is null || rhs is null) {
			return false;
		}

		var value = ((PyResolved) rhs.Result).Value;
		return value is null ? lhs is null : lhs.Equals(value);
	}

	public static bool operator == (PyObject? lhs, object? rhs) {
		if (lhs is null && rhs is null) {
			return true;
		} else if (lhs is null || rhs is null) {
			return false;
		}

		var value = ((PyResolved) lhs.Result).Value;
		return value is null ? rhs is null : value.Equals(rhs);
	}

	public static bool operator == (PyObject? lhs, PyObject? rhs) {
		if (lhs is null && rhs is null) {
			return true;
		} else if (lhs is null || rhs is null) {
			return false;
		}

		var value1 = ((PyResolved) lhs.Result).Value;
		var value2 = ((PyResolved) rhs.Result).Value;

		if (value1 is null || value2 is null) {
			return (value1 is null) == (value2 is null);
		}

		return value1.Equals(value2);
	}

	public static bool operator != (object? lhs, PyObject? rhs) {
		if (lhs is null && rhs is null) {
			return false;
		} else if (lhs is null || rhs is null) {
			return true;
		}
		
		var value = ((PyResolved) rhs.Result).Value;
		return value is null ? lhs is not null : !lhs.Equals(value);
	}

	public static bool operator != (PyObject? lhs, object? rhs) {
		if (lhs is null && rhs is null) {
			return false;
		} else if (lhs is null || rhs is null) {
			return true;
		}

		var value = ((PyResolved) lhs.Result).Value;
		return value is null ? rhs is not null : !value.Equals(rhs);
	}

	public static bool operator != (PyObject? lhs, PyObject? rhs) {
		if (lhs is null && rhs is null) {
			return false;
		} else if (lhs is null || rhs is null) {
			return true;
		}

		var value1 = ((PyResolved) lhs.Result).Value;
		var value2 = ((PyResolved) rhs.Result).Value;

		if (value1 is null || value2 is null) {
			return (value1 is null) != (value2 is null);
		}

		return !value1.Equals(value2);
	}

	public static bool operator < (object? lhs, PyObject rhs) {
		var rhsValue = ((PyResolved) rhs.Result).Value;
		
		if (lhs is IComparable v1 && rhsValue is IComparable v2) {
			return v1.NumCompareTo(v2) < 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhs?.GetType()?.ToString() ?? "null"} and {rhsValue?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator < (PyObject lhs, object? rhs) {
		var lhsValue = ((PyResolved) lhs.Result).Value;
		
		if (lhsValue is IComparable v1 && rhs is IComparable v2) {
			return v1.NumCompareTo(v2) < 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhsValue?.GetType()?.ToString() ?? "null"} and {rhs?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator < (PyObject lhs, PyObject rhs) {
		var lhsValue = ((PyResolved) lhs.Result).Value;
		var rhsValue = ((PyResolved) rhs.Result).Value;
		
		if (lhsValue is IComparable v1 && rhsValue is IComparable v2) {
			return v1.NumCompareTo(v2) < 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhsValue?.GetType()?.ToString() ?? "null"} and {rhsValue?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator <= (object? lhs, PyObject rhs) {
		var rhsValue = ((PyResolved) rhs.Result).Value;
		
		if (lhs is IComparable v1 && rhsValue is IComparable v2) {
			return v1.NumCompareTo(v2) <= 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhs?.GetType()?.ToString() ?? "null"} and {rhsValue?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator <= (PyObject lhs, object? rhs) {
		var lhsValue = ((PyResolved) lhs.Result).Value;
		
		if (lhsValue is IComparable v1 && rhs is IComparable v2) {
			return v1.NumCompareTo(v2) <= 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhsValue?.GetType()?.ToString() ?? "null"} and {rhs?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator <= (PyObject lhs, PyObject rhs) {
		var lhsValue = ((PyResolved) lhs.Result).Value;
		var rhsValue = ((PyResolved) rhs.Result).Value;
		
		if (lhsValue is IComparable v1 && rhsValue is IComparable v2) {
			return v1.NumCompareTo(v2) <= 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhsValue?.GetType()?.ToString() ?? "null"} and {rhsValue?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator > (object? lhs, PyObject rhs) {
		var rhsValue = ((PyResolved) rhs.Result).Value;
		
		if (lhs is IComparable v1 && rhsValue is IComparable v2) {
			return v1.NumCompareTo(v2) > 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhs?.GetType()?.ToString() ?? "null"} and {rhsValue?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator > (PyObject lhs, object? rhs) {
		var lhsValue = ((PyResolved) lhs.Result).Value;
		
		if (lhsValue is IComparable v1 && rhs is IComparable v2) {
			return v1.NumCompareTo(v2) > 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhsValue?.GetType()?.ToString() ?? "null"} and {rhs?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator > (PyObject lhs, PyObject rhs) {
		var lhsValue = ((PyResolved) lhs.Result).Value;
		var rhsValue = ((PyResolved) rhs.Result).Value;
		
		if (lhsValue is IComparable v1 && rhsValue is IComparable v2) {
			return v1.NumCompareTo(v2) > 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhsValue?.GetType()?.ToString() ?? "null"} and {rhsValue?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator >= (object? lhs, PyObject rhs) {
		var rhsValue = ((PyResolved) rhs.Result).Value;
		
		if (lhs is IComparable v1 && rhsValue is IComparable v2) {
			return v1.NumCompareTo(v2) >= 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhs?.GetType()?.ToString() ?? "null"} and {rhsValue?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator >= (PyObject lhs, object? rhs) {
		var lhsValue = ((PyResolved) lhs.Result).Value;
		
		if (lhsValue is IComparable v1 && rhs is IComparable v2) {
			return v1.NumCompareTo(v2) >= 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhsValue?.GetType()?.ToString() ?? "null"} and {rhs?.GetType()?.ToString() ?? "null"}.");
		}
	}

	public static bool operator >= (PyObject lhs, PyObject rhs) {
		var lhsValue = ((PyResolved) lhs.Result).Value;
		var rhsValue = ((PyResolved) rhs.Result).Value;
		
		if (lhsValue is IComparable v1 && rhsValue is IComparable v2) {
			return v1.NumCompareTo(v2) >= 0;
		} else {
			throw new InvalidOperationException($"Cannot compare PyObjects of types {lhsValue?.GetType()?.ToString() ?? "null"} and {rhsValue?.GetType()?.ToString() ?? "null"}.");
		}
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
			AssignKeyValue(key, value);
		}
	}

	internal abstract void AssignKeyValue(PyObject key, PyObject value);

	public override bool Equals(object? obj) {
		if (obj is PyObject) {
			return this == (PyObject) obj;
		} else {
			return this == obj;
		}
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
