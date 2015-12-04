using UnityEngine;
using System.Collections;

public class NodeMonoView : MonoBehaviour {

	private string _id = "Id not set";

	public string Id {
		get {
			return _id;
		}
		set {
			_id = value;
		}
	}
}
