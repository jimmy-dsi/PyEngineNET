namespace PyEngine;

public interface MethodBinding {
	public PyObject Invoke(params PyObject[] args);
}
