namespace PyEngine;

public partial class Engine: IDisposable {
	public void BindFunction<TResult>(string pyFuncName, Func<TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<TResult>) csMethod);

	public void BindFunction<T, TResult>(string pyFuncName, Func<T, TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<T, TResult>) csMethod);

	public void BindFunction<T1, T2, TResult>(string pyFuncName, Func<T1, T2, TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<T1, T2, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, TResult>(string pyFuncName, Func<T1, T2, T3, TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, TResult>(string pyFuncName, Func<T1, T2, T3, T4, TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, TResult>(string pyFuncName, Func<T1, T2, T3, T4, T5, TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, TResult>(string pyFuncName, Func<T1, T2, T3, T4, T5, T6, TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, TResult>(string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> csMethod) =>
		BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> csMethod) =>
			BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> csMethod) =>
			BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> csMethod) =>
			BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> csMethod) =>
			BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> csMethod) =>
			BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> csMethod) =>
			BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> csMethod) =>
			BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> csMethod) =>
			BindMethod(pyFuncName, (FuncBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>) csMethod);

	public void BindFunction(string pyFuncName, Action csMethod) =>
		BindMethod(pyFuncName, (ActionBinding) csMethod);

	public void BindFunction<T>(string pyFuncName, Action<T> csMethod) =>
		BindMethod(pyFuncName, (ActionBinding<T>) csMethod);

	public void BindFunction<T1, T2>(string pyFuncName, Action<T1, T2> csMethod) =>
		BindMethod(pyFuncName, (ActionBinding<T1, T2>) csMethod);

	public void BindFunction<T1, T2, T3>(string pyFuncName, Action<T1, T2, T3> csMethod) =>
		BindMethod(pyFuncName, (ActionBinding<T1, T2, T3>) csMethod);

	public void BindFunction<T1, T2, T3, T4>(string pyFuncName, Action<T1, T2, T3, T4> csMethod) =>
		BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5>(string pyFuncName, Action<T1, T2, T3, T4, T5> csMethod) =>
		BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6>(string pyFuncName, Action<T1, T2, T3, T4, T5, T6> csMethod) =>
		BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7>(string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7> csMethod) =>
		BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8>(string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8> csMethod) =>
		BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> csMethod) =>
			BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> csMethod) =>
			BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> csMethod) =>
			BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> csMethod) =>
			BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> csMethod) =>
			BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> csMethod) =>
			BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
		string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> csMethod) =>
			BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>) csMethod);

	public void BindFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
		string pyFuncName, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> csMethod) =>
			BindMethod(pyFuncName, (ActionBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>) csMethod);

	public void BindGenerator(string pyFuncName, Func<IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding) csMethod);

	public void BindGenerator<T>(string pyFuncName, Func<T, IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding<T>) csMethod);

	public void BindGenerator<T1, T2>(string pyFuncName, Func<T1, T2, IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2>) csMethod);

	public void BindGenerator<T1, T2, T3>(string pyFuncName, Func<T1, T2, T3, IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3>) csMethod);

	public void BindGenerator<T1, T2, T3, T4>(string pyFuncName, Func<T1, T2, T3, T4, IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5>(string pyFuncName, Func<T1, T2, T3, T4, T5, IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6>(string pyFuncName, Func<T1, T2, T3, T4, T5, T6, IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7>(string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8>(string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<PyObject>> csMethod) =>
		BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<PyObject>> csMethod) =>
			BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<PyObject>> csMethod) =>
			BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IEnumerable<PyObject>> csMethod) =>
			BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IEnumerable<PyObject>> csMethod) =>
			BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IEnumerable<PyObject>> csMethod) =>
			BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IEnumerable<PyObject>> csMethod) =>
			BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IEnumerable<PyObject>> csMethod) =>
			BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>) csMethod);

	public void BindGenerator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
		string pyFuncName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IEnumerable<PyObject>> csMethod) =>
			BindGeneratorMethod(pyFuncName, (GenBinding<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>) csMethod);
}
