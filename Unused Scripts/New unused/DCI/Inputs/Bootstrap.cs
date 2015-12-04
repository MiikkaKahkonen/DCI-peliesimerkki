using UnityEngine;
using System.Collections;

public class Bootstrap : MonoBehaviour {

	// Ingame static transforms for groupping
	public static GameObject nodeParent;
	public static GameObject edgeParent;

	// GameObjects for nodes and edges
	public static GameObject node;
	public static GameObject edge;

	// Setting up from editor
	public GameObject NodeParent;
	public GameObject EdgeParent;
	public GameObject Node;
	public GameObject Edge;

	// input system
	public InputSystem inputSystem;

	// Camera Movement values
	public float dragSpeed = 5;
	public float slowDownTimerBase = 1f;
	public float maxXSpeed = 1f;
	public float maxYSpeed = 1f;

	// string variable for messaging
	private static string message = "";
	static bool newMessage = false;

	// Game variable
	//private Game game;

	// Use this for initialization
	void Start () {

		nodeParent = NodeParent;
		edgeParent = EdgeParent;

		node = Node;
		edge = Edge;

		// Set camera mode to ortographic, as the game is z locked, it's more useful
		Camera.main.orthographic = true;

		//game = new Game();
		//game.Initialize("mynewtest2",inputSystem,"normal");
	}
	
	// Update is called once per frame
	void Update () {
		if(newMessage)
		{
			newMessage = false;
			print (message);
			message = "";
		}
		//game.MyUpdate();
	}

	public static void PrintMessage(string s)
	{
		message += s + "\n";
		newMessage = true;
	}

	public static GameObject CheckRay(Vector3 position)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(position);
		
		if (Physics.Raycast (ray, out hit))
		{
			if(hit.collider.tag == "Node" || hit.collider.tag == "Edge" || hit.collider.tag == "Quess")
			{
				if(hit.collider.transform.parent.renderer.enabled)
				{
					return hit.collider.transform.parent.gameObject;
				}
			}
		}
		return null;
	}
}
