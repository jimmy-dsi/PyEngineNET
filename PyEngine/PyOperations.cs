namespace PyEngine;

internal abstract class PyOperation: PyObject {
	internal PyOperation(Engine? engine): base(engine) { }

	internal override PyObject evaluate() {
		var resultObject = engine.Eval(getExpression(), eager: true);
		return resultObject;
	}

	public override int GetHashCode() {
		return Result.GetHashCode();
	}
}


internal class PyAddition: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyAddition(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} + {_rhs.getExpression()})";
	}
}


internal class PySubtraction: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PySubtraction(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} - {_rhs.getExpression()})";
	}
}


internal class PyMultiplication: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyMultiplication(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} * {_rhs.getExpression()})";
	}
}


internal class PyDivision: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyDivision(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} / {_rhs.getExpression()})";
	}
}


internal class PyFloorDivision: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyFloorDivision(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} // {_rhs.getExpression()})";
	}
}


internal class PyModulo: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyModulo(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} % {_rhs.getExpression()})";
	}
}


internal class PyExponentiation: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyExponentiation(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} ** {_rhs.getExpression()})";
	}
}


internal class PyBitwiseAnd: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyBitwiseAnd(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} & {_rhs.getExpression()})";
	}
}


internal class PyBitwiseOr: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyBitwiseOr(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} | {_rhs.getExpression()})";
	}
}


internal class PyBitwiseXor: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyBitwiseXor(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} ^ {_rhs.getExpression()})";
	}
}


internal class PyBitshiftLeft: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyBitshiftLeft(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} << {_rhs.getExpression()})";
	}
}


internal class PyBitshiftRight: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyBitshiftRight(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} >> {_rhs.getExpression()})";
	}
}


internal class PyEquality: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyEquality(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} == {_rhs.getExpression()})";
	}
}


internal class PyInequality: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyInequality(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} != {_rhs.getExpression()})";
	}
}


internal class PyLessThan: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyLessThan(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} < {_rhs.getExpression()})";
	}
}


internal class PyLessThanOrEqual: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyLessThanOrEqual(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} <= {_rhs.getExpression()})";
	}
}


internal class PyGreaterThan: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyGreaterThan(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} > {_rhs.getExpression()})";
	}
}


internal class PyGreaterThanOrEqual: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyGreaterThanOrEqual(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} >= {_rhs.getExpression()})";
	}
}


internal class PyInclusion: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyInclusion(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} in {_rhs.getExpression()})";
	}
}


internal class PyExclusion: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyExclusion(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} not in {_rhs.getExpression()})";
	}
}


internal class PyRefEquality: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyRefEquality(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} is {_rhs.getExpression()})";
	}
}


internal class PyRefInequality: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyRefInequality(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} is not {_rhs.getExpression()})";
	}
}


internal class PyLogicalAnd: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyLogicalAnd(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} and {_rhs.getExpression()})";
	}
}


internal class PyLogicalOr: PyOperation {
	private PyObject _lhs;
	private PyObject _rhs;

	internal PyLogicalOr(Engine? engine, PyObject lhs, PyObject rhs): base(engine) {
		_lhs = lhs;
		_rhs = rhs;
	}

	public override void Dispose() {
		_lhs.Dispose();
		_rhs.Dispose();
	}

	internal override string getExpression() {
		return $"({_lhs.getExpression()} or {_rhs.getExpression()})";
	}
}


internal class PyIdentity: PyOperation {
	private PyObject _value;

	internal PyIdentity(Engine? engine, PyObject value): base(engine) {
		_value = value;
	}

	public override void Dispose() {
		_value.Dispose();
	}

	internal override string getExpression() {
		return $"(+{_value.getExpression()})";
	}
}


internal class PyNegation: PyOperation {
	private PyObject _value;

	internal PyNegation(Engine? engine, PyObject value): base(engine) {
		_value = value;
	}

	public override void Dispose() {
		_value.Dispose();
	}

	internal override string getExpression() {
		return $"(-{_value.getExpression()})";
	}
}


internal class PyBitwiseNegation: PyOperation {
	private PyObject _value;

	internal PyBitwiseNegation(Engine? engine, PyObject value): base(engine) {
		_value = value;
	}

	public override void Dispose() {
		_value.Dispose();
	}

	internal override string getExpression() {
		return $"(~{_value.getExpression()})";
	}
}


internal class PyLogicalNegation: PyOperation {
	private PyObject _value;

	internal PyLogicalNegation(Engine? engine, PyObject value): base(engine) {
		_value = value;
	}

	public override void Dispose() {
		_value.Dispose();
	}

	internal override string getExpression() {
		return $"(not {_value.getExpression()})";
	}
}


internal class PySplat: PyOperation {
	private PyObject _value;

	internal PySplat(Engine? engine, PyObject value): base(engine) {
		_value = value;
	}

	public override void Dispose() {
		_value.Dispose();
	}

	internal override string getExpression() {
		return $"(*{_value.getExpression()})";
	}
}


internal class PyInvokation: PyOperation {
	private PyObject   _value;
	private PyObject[] _params;

	internal PyInvokation(Engine? engine, PyObject value, params PyObject[] args): base(engine) {
		_value  = value;
		_params = args;
	}

	public override void Dispose() {
		_value.Dispose();
		foreach (var param in _params) {
			param.Dispose();
		}
	}

	internal override string getExpression() {
		var argExprs = new List<string>();
		foreach (var param in _params) {
			argExprs.Add(param.getExpression());
		}
		return $"{_value.getExpression()}({string.Join(", ", argExprs)})";
	}
}


internal class PyAccess: PyOperation {
	private PyObject _value;
	private string   _memberName;

	internal PyAccess(Engine? engine, PyObject value, string memberName): base(engine) {
		_value      = value;
		_memberName = memberName;
	}

	public override void Dispose() {
		_value.Dispose();
	}

	internal override string getExpression() {
		return $"{_value.getExpression()}.{_memberName}";
	}
}


internal class PyKeyLookup: PyOperation {
	private PyObject _value;
	private PyObject _key;

	internal PyKeyLookup(Engine? engine, PyObject value, PyObject key): base(engine) {
		_value = value;
		_key   = key;
	}

	public override void Dispose() {
		_value.Dispose();
		_key.Dispose();
	}

	internal override string getExpression() {
		return $"{_value.getExpression()}[{_key.getExpression()}]";
	}
}