using UnityEngine;
using System.Collections;

public class CursorScript : MonoBehaviour {

	public enum MouseState { point,hand,drag }

	public Sprite point;
	public Sprite hand;
	public Sprite drag;

	public static MouseState state;
	public static Vector3 position;

	private MouseState current_state = MouseState.point;
	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		state = MouseState.point;
	}
	
	// Update is called once per frame
	void Update () {
		if( state != current_state)
		{
			Sprite new_sprite = null;
			switch(state)
			{
				case MouseState.drag : new_sprite = drag ;break;
				case MouseState.point : new_sprite = point ;break;
				case MouseState.hand : new_sprite = hand ;break;
			}

			gameObject.GetComponent<SpriteRenderer>().sprite = new_sprite;
			gameObject.renderer.material.SetColor("_Color", Color.red);
			current_state = state;
		}
	}

	void OnGUI()
	{
		Vector3 pos = Input.mousePosition;
		pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = -5;
		pos.y -= 1;

		transform.position = pos;
		position = pos;
	}

}
