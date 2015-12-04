using UnityEngine;
using System.Collections;

public class KeyDown : IGameInput {

	public KeyCode key;

	public KeyDown(KeyCode code)
	{
		key = code;
	}
}
