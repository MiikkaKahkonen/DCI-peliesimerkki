using UnityEngine;
using System.Collections;

public class DEdge : Data, RGameEdge
{
	string _headId;
	string _tailId;

	#region RGameEdge implementation
	public string HeadId {
		get {
			return _headId;
		}
		set {
			_headId = value;
		}
	}
	public string TailId {
		get {
			return _tailId;
		}
		set {
			_tailId = value;
		}
	}
	#endregion
}
