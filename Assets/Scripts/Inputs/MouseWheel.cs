using UnityEngine;
using System.Collections;

public class MouseWheel : IGameInput {
	public bool up;

	public MouseWheel(bool up)
	{
		this.up = up;
	}
}
