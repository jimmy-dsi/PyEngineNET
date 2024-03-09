namespace PyEngine;

public class DataClassObject {
	private string   _fullyQualName;
	private string[] _propNames;
	private Dictionary<string, object> _properties;

	internal DataClassObject(string fullyQualName, string[] propNames, Dictionary<string, object> properties) {
		_fullyQualName = fullyQualName;
		_propNames     = propNames;
		_properties    = properties;
	}

	public IEnumerable<string> PropNames {
		get {
			foreach (var prop in _propNames) {
				yield return prop;
			}
		}
	}

	public object this[string propName] {
		get {
			if (_properties.ContainsKey(propName)) {
				return _properties[propName];
			} else {
				throw new KeyNotFoundException($"The property `{propName}` does not exist in dataclass `{_fullyQualName}`");
			}
		}
		set {
			if (_properties.ContainsKey(propName)) {
				_properties[propName] = value;
			} else {
				throw new KeyNotFoundException($"The property `{propName}` does not exist in dataclass `{_fullyQualName}`");
			}
		}
	}

	internal string ClassName => _fullyQualName;
}
