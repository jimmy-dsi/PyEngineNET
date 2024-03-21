namespace PyEngine;

public interface MethodBinding {
	public PyObject Invoke(params PyObject[] args);
}

public interface GeneratorBinding {
	public IEnumerable<PyObject> Invoke(params PyObject[] args);
}
