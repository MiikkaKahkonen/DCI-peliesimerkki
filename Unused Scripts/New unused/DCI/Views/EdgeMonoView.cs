using UnityEngine;
using System.Collections;

public class EdgeMonoView : MonoBehaviour {

	private string _headId = "Head id not set";
	private string _tailId = "Tail id not set";
	private bool headIsSet = false;
	private bool tailIsSet = false;
	
	public string Id {
		get {
			if(headIsSet && tailIsSet)
			{
				return _headId+_tailId;
			}
			return "Head or Tail not set.";
		}
	}

	public string Head {
		get {
			return _headId;
		}
		set {
			headIsSet = true;
			_headId = value;
		}
	}
	public string Tail {
		get {
			return _tailId;
		}
		set {
			tailIsSet = true;
			_tailId = value;
		}
	}
}
