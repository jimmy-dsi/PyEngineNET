namespace PyEngine;

public class GenBinding: GeneratorBinding {
	private Func<IEnumerable<PyObject>> _func;

	internal GenBinding(Func<IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func();
	}

	// Conversions:
	public static implicit operator GenBinding(Func<IEnumerable<PyObject>> func) => new(func);
}


public class GenBinding<T>: GeneratorBinding {
	private Func<T, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T>());
	}

	// Conversions:
	public static implicit operator GenBinding<T>(Func<T, IEnumerable<PyObject>> func) => new(func);
}