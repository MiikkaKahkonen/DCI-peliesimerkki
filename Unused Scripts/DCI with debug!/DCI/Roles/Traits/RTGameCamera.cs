using UnityEngine;
using System.Collections;

public static class RTGameGamera
{
	public static bool MoveTowards(this RGameCamera self, Vector3 to)
	{
		self.Cam.transform.position = Vector3.MoveTowards(self.Cam.transform.position,to,1f);
		if(self.Cam.transform.position == to)
			return true;
		return false;
	}
}