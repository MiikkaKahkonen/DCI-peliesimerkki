using UnityEngine;
using System.Collections;

public class MouseClick : IGameInput {
	public Vector3 pos;

	public MouseClick(Vector3 pos)
	{
		this.pos = pos;
	}
}
