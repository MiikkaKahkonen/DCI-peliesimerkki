using UnityEngine;
using System.Collections;
using DCI.Roles;

public class DataCamera : Data, DCI.Roles.GameCamera
{
	Camera cam;
    
	public DataCamera(Camera camera)
	{
		cam = camera;
	}

	#region GameCamera implementation
	public Vector3 CameraPosition {
		get {
			return cam.transform.position;
		}
		set {
			cam.transform.position = value;
		}
	}
	public float Zoom {
		get {
			return cam.orthographicSize;
		}
		set {
			cam.orthographicSize = value;
		}
	}

	public Vector3 ScreenToViewportPoint (Vector3 pos)
	{
		return cam.ScreenToViewportPoint(pos);
	}

	public void Translate(Vector3 translation, Space relativeTo)
	{
		cam.transform.Translate(translation,relativeTo);
	}
	#endregion
}
