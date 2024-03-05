namespace PyEngine;

public class ActionBinding: MethodBinding {
	private Action _action;

	internal ActionBinding(Action action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action();
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding(Action action) => new(action);
}


public class ActionBinding<T>: MethodBinding {
	private Action<T> _action;

	internal ActionBinding(Action<T> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T>(Action<T> action) => new(action);
}


public class ActionBinding<T1, T2>: MethodBinding {
	private Action<T1, T2> _action;

	internal ActionBinding(Action<T1, T2> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2>(Action<T1, T2> action) => new(action);
}


public class ActionBinding<T1, T2, T3>: MethodBinding {
	private Action<T1, T2, T3> _action;

	internal ActionBinding(Action<T1, T2, T3> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3>(Action<T1, T2, T3> action) => new(action);
}


public class ActionBinding<T1, T2, T3, T4>: MethodBinding {
	private Action<T1, T2, T3, T4> _action;

	internal ActionBinding(Action<T1, T2, T3, T4> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5>: MethodBinding {
	private Action<T1, T2, T3, T4, T5> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6>: MethodBinding {
	private Action<T1, T2, T3, T4, T5, T6> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7>: MethodBinding {
	private Action<T1, T2, T3, T4, T5, T6, T7> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8>: MethodBinding {
	private Action<T1, T2, T3, T4, T5, T6, T7, T8> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9>
	: MethodBinding
{
	private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		        args[8].ConvertTo<T9>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action
	) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
	: MethodBinding
{
	private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		        args[8].ConvertTo<T9>(), args[9].ConvertTo<T10>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action
	) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
	: MethodBinding
{
	private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(), args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(), args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(), args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(), args[7].ConvertTo<T8>(),
		        args[8].ConvertTo<T9>(), args[9].ConvertTo<T10>(),
		        args[10].ConvertTo<T11>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action
	) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
	: MethodBinding
{
	private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		        args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		        args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action
	) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
	: MethodBinding
{
	private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		        args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		        args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		        args[12].ConvertTo<T13>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action
	) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
	: MethodBinding
{
	private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		        args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		        args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		        args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action
	) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
	: MethodBinding
{
	private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		        args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		        args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		        args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>(),
		        args[14].ConvertTo<T15>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action
	) => new(action);
}


public class ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
	: MethodBinding
{
	private Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> _action;

	internal ActionBinding(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action) {
		_action = action;
	}

	public PyObject Invoke(params PyObject[] args) {
		_action(args[0].ConvertTo<T1>(),   args[1].ConvertTo<T2>(),
		        args[2].ConvertTo<T3>(),   args[3].ConvertTo<T4>(),
		        args[4].ConvertTo<T5>(),   args[5].ConvertTo<T6>(),
		        args[6].ConvertTo<T7>(),   args[7].ConvertTo<T8>(),
		        args[8].ConvertTo<T9>(),   args[9].ConvertTo<T10>(),
		        args[10].ConvertTo<T11>(), args[11].ConvertTo<T12>(),
		        args[12].ConvertTo<T13>(), args[13].ConvertTo<T14>(),
		        args[14].ConvertTo<T15>(), args[15].ConvertTo<T16>());
		return PyObject.None;
	}

	// Conversions:
	public static implicit operator ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
		Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action
	) => new(action);
}