using UnityEngine;
using System.Collections;

public class MouseRightClick : IGameInput {
	public Vector3 pos;
	
	public MouseRightClick(Vector3 pos)
	{
		this.pos = pos;
	}
}
