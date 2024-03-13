namespace PyEngine;

internal class PyProxy: PyObject, IDisposable {
	private string? _pyGVarName;
	private bool _disposedValue;

	internal string pyGVarName => _pyGVarName!;

	internal PyProxy(Engine? engine, string pyKey): base(engine) {
		_pyGVarName = pyKey;
		_disposedValue = false;
	}

	internal static PyProxy Create(Engine engine) {
		var gvarName = $"___pye_var___{gvarNum.ShuffleHash()}";
		gvarNum++;
		return new(engine, gvarName);
	}

	public override int GetHashCode() {
		checkPyKey();
		return Result.GetHashCode();
	}

	public override void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	// Conversions
	public override T ConvertTo<T>() {
		if (typeof(T) == typeof(PyObject)) {
			return (T) (object) this;
		} else {
			checkPyKey();
			return convertTo<T>();
		}
	}
	
	internal override void AssignKeyValue(PyObject key, PyObject value) {
		checkPyKey();
		engine.Exec($"{pyGVarName}[{key.Expression}] = {value.Expression}");
	}

	//
	internal override string getExpression() {
		checkPyKey();
		return _pyGVarName!;
	}

	internal override PyObject evaluate() {
		checkPyKey();
		var resultObject = engine.Eval(getExpression(), eager: true);
		return resultObject;
	}

	internal override PyObject lazyEvaluate() {
		checkPyKey();
		var resultObject = engine.Eval(getExpression());
		return resultObject;
	}

	protected virtual void Dispose(bool disposing) {
		if (_disposable && !_disposedValue) {
			var pyKey = _pyGVarName;
			if (disposing) {
				_pyGVarName = null;
			}

			// Delete global variable on Python side
			engine.Exec($"global {pyKey} \n"
			          + $"del {pyKey} \n");

			_disposedValue = true;
		}
	}

	~PyProxy() {
		Dispose(disposing: false);
	}

	internal void checkPyKey() {
		if (_pyGVarName == null) {
			throw new InvalidOperationException("Cannot perform operations on disposed PyObject.");
		}
	}
}
