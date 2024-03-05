namespace PyEngine;

public class FuncBinding<Result>: MethodBinding {
	private Func<Result> _func;

	internal FuncBinding(Func<Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func());
	}

	// Conversions:
	public static implicit operator FuncBinding<Result>(Func<Result> func) => new(func);
}


public class FuncBinding<T, Result>: MethodBinding {
	private Func<T, Result> _func;

	internal FuncBinding(Func<T, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T, Result>(Func<T, Result> func) => new(func);
}


public class FuncBinding<T1, T2, Result>: MethodBinding {
	private Func<T1, T2, Result> _func;

	internal FuncBinding(Func<T1, T2, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, Result>(Func<T1, T2, Result> func) => new(func);
}


public class FuncBinding<T1, T2, T3, Result>: MethodBinding {
	private Func<T1, T2, T3, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, Result>(Func<T1, T2, T3, Result> func) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, Result>(Func<T1, T2, T3, T4, Result> func) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, Result>(
		Func<T1, T2, T3, T4, T5, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, Result>(
		Func<T1, T2, T3, T4, T5, T6, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		                                  args[8].ConvertTo<T9>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		                                  args[8].ConvertTo<T9>(), args[9].ConvertTo<T10>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		                                  args[8].ConvertTo<T9>(), args[9].ConvertTo<T10>(),
		                                  args[10].ConvertTo<T11>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		                                  args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		                                  args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		                                  args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		                                  args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		                                  args[12].ConvertTo<T13>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		                                  args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		                                  args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		                                  args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		                                  args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		                                  args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		                                  args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>(),
		                                  args[14].ConvertTo<T15>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Result> func
	) => new(func);
}


public class FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Result>: MethodBinding {
	private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Result> _func;

	internal FuncBinding(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Result> func) {
		_func = func;
	}

	public PyObject Invoke(params PyObject[] args) {
		return PyObject.ConvertFrom(_func(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		                                  args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		                                  args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		                                  args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		                                  args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		                                  args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		                                  args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>(),
		                                  args[14].ConvertTo<T15>(), args[15].ConvertTo<T16>()));
	}

	// Conversions:
	public static implicit operator FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Result>(
		Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Result> func
	) => new(func);
}