using UnityEngine;
using System.Collections;

public class MouseDown : IGameInput {
	public Vector3 pos;
	
	public MouseDown(Vector3 pos)
	{
		this.pos = pos;
	}
}
