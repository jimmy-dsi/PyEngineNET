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


public class GenBinding<T1, T2>: GeneratorBinding {
	private Func<T1, T2, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2>(Func<T1, T2, IEnumerable<PyObject>> func) => new(func);
}


public class GenBinding<T1, T2, T3>: GeneratorBinding {
	private Func<T1, T2, T3, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3>(Func<T1, T2, T3, IEnumerable<PyObject>> func) => new(func);
}


public class GenBinding<T1, T2, T3, T4>: GeneratorBinding {
	private Func<T1, T2, T3, T4, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4>(Func<T1, T2, T3, T4, IEnumerable<PyObject>> func) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5>(
		Func<T1, T2, T3, T4, T5, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6>(
		Func<T1, T2, T3, T4, T5, T6, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7>(
		Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		             args[8].ConvertTo<T9>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		             args[8].ConvertTo<T9>(), args[9].ConvertTo<T10>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		             args[8].ConvertTo<T9>(), args[9].ConvertTo<T10>(),
		             args[10].ConvertTo<T11>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		             args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		             args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		             args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		             args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		             args[12].ConvertTo<T13>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		             args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		             args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		             args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		             args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		             args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		             args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>(),
		             args[14].ConvertTo<T15>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IEnumerable<PyObject>> func
	) => new(func);
}


public class GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>: GeneratorBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IEnumerable<PyObject>> _func;

	internal GenBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IEnumerable<PyObject>> func) {
		_func = func;
	}

	public IEnumerable<PyObject> Invoke(params PyObject[] args) {
		return _func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		             args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		             args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		             args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		             args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		             args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		             args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>(),
		             args[14].ConvertTo<T15>(), args[15].ConvertTo<T16>());
	}

	// Conversions:
	public static implicit operator GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IEnumerable<PyObject>> func
	) => new(func);
}