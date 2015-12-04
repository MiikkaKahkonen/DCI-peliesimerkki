using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataNode : Data,DCI.Roles.GameNode
{
	string _id;
	Vector3 _position;
	string _word;
	string _emptyWord;

	Manager.DCINodeState _state;

	List<DCI.Roles.Observer> _observers;
	bool _hasChanged;

	public DataNode()
	{
		_observers  = new List<DCI.Roles.Observer>();
		_state = Manager.DCINodeState.Hidden;
		_hasChanged = false;
	}

	#region GraphNode implementation
	public string Id {
		get {
			return _id;
		}
		set {
			_id = value;
		}
	}

	public Vector3 Position {
		get {
			return _position;
		}
		set {
			_position = value;
		}
	}

	public Manager.DCINodeState State {
		get {
			return _state;
		}
		set {
			_state = value;
		}
	}

	public string Word {
		get {
			return _word;
		}
		set {
			_word = value;
		}
	}

	public string EmptyWord {
		get {
			return _emptyWord;
		}
		set {
			_emptyWord = value;
		}
	}

	#endregion


	#region Observable implementation
	public List<DCI.Roles.Observer> Observers {
		get {
			return _observers;
		}
		set {
			_observers = value;
		}
	}
	public bool Changed {
		get {
			return _hasChanged;
		}
		set {
			_hasChanged = value;
		}
	}
	#endregion
}
