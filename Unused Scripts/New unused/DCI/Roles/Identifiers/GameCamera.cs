using UnityEngine;
using System.Collections;

namespace DCI.Roles
{
	public interface GameCamera
	{
		Vector3 CameraPosition{get;set;}
		float Zoom{get;set;}
		Vector3 ScreenToViewportPoint(Vector3 pos);
		void Translate(Vector3 translation, Space relativeTo);
	}
}