using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGraphBank : DCI.Roles.GraphStorage
{

	Dictionary<string,DCI.Roles.GameGraph> _data;
	string _current_key;


	public GameGraphBank()
	{
		_data = new Dictionary<string, DCI.Roles.GameGraph>();
		_current_key = "default";
	}

	#region GraphStorage implementation

	public DCI.Roles.GameGraph Data {
		get {
			if(_data.ContainsKey(_current_key))
				return _data[_current_key];
			else
				return null;
		}
		set {
			if(_data.ContainsKey(_current_key))
				_data[_current_key] = value;
			else
				_data.Add(_current_key,value);
		}
	}

	public string Name {
		get {
			return _current_key;
		}
		set {
			_current_key = value;
		}
	}

	#endregion



}
