using UnityEngine;
using System.Collections;

public class DCamera : Data,RGameCamera
{
	Camera _cam;

	public DCamera(Camera cam) : base()
	{
		this._cam = cam;
	}


	#region RGameCamera implementation
	public Camera Cam {
		get {
			return _cam;
		}
		set {
			_cam = value;
		}
	}
	#endregion
}
