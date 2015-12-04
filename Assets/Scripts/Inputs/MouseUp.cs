using UnityEngine;
using System.Collections;

public class MouseUp : IGameInput {
	public Vector3 pos;
	
	public MouseUp(Vector3 pos)
	{
		this.pos = pos;
	}
}
