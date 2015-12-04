using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataEdge : Data, DCI.Roles.GameEdge
{
	DCI.Roles.GameNode _head;
	DCI.Roles.GameNode _tail;

	string _id;
	string _headid;
	string _tailid;

	Manager.DCIEdgeState _state;
	List<DCI.Roles.Observer> _observers;
	bool _hasChanged;

	public DataEdge()
	{
		_id = Manager.GetNewEdgeId();
		_observers = new List<DCI.Roles.Observer>();
		_hasChanged = false;
	}

	#region Observer implementation

	public void Update (string message) 
	{
		if(message != "delete")
		{
			if((int)this.Head.State < (int)Manager.DCINodeState.Quessable || (int)this.Tail.State < (int)Manager.DCINodeState.Quessable)
				this.State = Manager.DCIEdgeState.Shown;
			else
				this.State = Manager.DCIEdgeState.Hidden;
		}
		else
		{
			_head = null;
			_tail = null;
		}
	}

	#endregion

	#region Observable implementation

	public System.Collections.Generic.List<DCI.Roles.Observer> Observers {
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

	#region GraphEdge implementation

	public DCI.Roles.GameNode Head {
		get {
			return _head;
		}
		set {
			_head = value;
			_headid = value.Id;
		}
	}

	public DCI.Roles.GameNode Tail {
		get {
			return _tail;
		}
		set {
			_tail = value;
			_tailid = value.Id;
		}
	}

	public string Id {
		get {
			return _id;
		}
		set {
			_id = value;
		}
	}

	public Manager.DCIEdgeState State {
		get {
			return _state;
		}
		set {
			_state = value;
		}
	}

	#endregion


}
