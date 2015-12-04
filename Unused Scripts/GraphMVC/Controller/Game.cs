using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game /*: MonoBehaviour*/ {

	// DEBUG
	/*bool drawStart = false;

	Vector3 lineStart = Vector3.zero;
	Vector3 lineEnd = Vector3.zero;*/
	//GameObject go = null;
	//GameObject go2 = null;

	// Static variables required by unity gameObjects for smoother game feel
	// bounds locks the camera movement specific min-max x and min-max y
	static Vector4 bounds;
	static GameObject nodeParent;
	static GameObject edgeParent;
	public static string mode = "";

	// GameObjects for nodes and edges
	//private GameObject node;
	//private GameObject edge;

	// LockZ and locked_z keeps the nodes and edges locked to specific (locked_z) z value
	bool lockZ = true;
	float locked_z = 0;

	// Map has and handels all data and view operations for the current map
	MapController map;


	// FileSystem for loading and saving the map. Now works with text and XML
	BaseFileSystem myFileSystem;
	InputSystem inputSystem;
	// Camera Movement values
	float dragSpeed = 5;
	float slowDownTimerBase = 1f;
	float maxXSpeed = 1f;
	float maxYSpeed = 1f;

	// Values requried for drag and drop
	Vector3 dragPosition;
	Vector3 mousePosition;
	Vector3 clickOriginalPosition;

	bool moveCamera = false;
	bool updateCameraToPointer = false;
	bool moveCameraWithDrag = false;

	// Values for making a smooth ending to camera movement when camera is moved
	bool slowCameraMovement = false;
	float slowDownTimer = 0;
	float slowDragSpeed = 0;
	Vector3 lastMousePosition;

	// Values for camera "zooming"
	bool orthChange = false;
	bool orthRise = false;
	int orthographicSizeMin = 15;
	int orthographicSizeMax = 40;

	// Dragging for objects
	GameObject selectedObject = null;
	//bool startDrag = false;
	bool moveSelected = false;
	bool updateSelectedToPointer = false;

	// Keeping track of pressed keys and making sure
	// no extra presses are made
	List<KeyCode> keysDowned = new List<KeyCode>();
	List<KeyCode> keysDownedLastTime = new List<KeyCode>();
	
	// Game state flags
	bool init = false;
	bool idled = false;
	bool lockInputs = true;
	bool dclick = false;
	bool rename = false;
	bool renameStarted = false;
	bool idleShow = false;
	bool trimIdle = false;

	// Values game timers
	float lockTimer = 0.5f;
	float idleTimer = 0.0f;
	float doubleClick = 0.0f;
	float renameIdleTimer = 0.4f;
	float inputTimer = 0.05f;

	// Initialization for the game
	public void Initialize(string fileToLoad, InputSystem system, string mode)
	{
		Game.mode = mode;

		// setting up inputs
		inputSystem = system;

		init = true;
		// initialise filesystem
		myFileSystem = BaseFileSystem.GetInstance();

		if(fileToLoad != "")
			myFileSystem.SetRelativePath(fileToLoad);

		// Init game map from text file
		List<string> file = null;
		try{
			file = myFileSystem.LoadTextFile();

		}
		catch(UnityException e)
		{
			Debug.Log("Problem opening file: "+fileToLoad);
			Application.Quit();
		}

		map = new MapController(file, Bootstrap.node,Bootstrap.edge);

		// Set camera bounds, map figures out what the maximum bounds for the map are
		SetBounds(map.GetBounds());
	}
	// Ran on every frame
	public void MyUpdate() {
		// Do not do updates if init not done
		if(!init)
			return;

		Queue<IGameInput> inputs = inputSystem.GetInputs();
		string inputString = inputSystem.GetInputString();

		// if:
		// we have inputs and the inputs are not locked then and not collecting text from keyboard
		// act on the inputs 
		// else if:
		// we are renaming and getting inputs 
		// else if:
		// renaming without inputs
		// else:
		// do idle stuff, update camera pointer, update dragging etc.

		if(!lockInputs && !rename)
		{
			CheckInputs(inputs);
		}
		else if(rename && inputTimer <= 0f)
		{
			TextLoop(inputString,inputs);
		}
		else if(rename)
		{
			RenameIdle();
		}
		else
		{
			Idle ();
		}

		// Update Camera and Dragged objects
		UpdateSelected();
		UpdateCamera();


		// Update mouse cursor
		UpdateCursor();

		// Update timers
		UpdateTimers();

	}
	void RenameIdle ()
	{
		if(renameIdleTimer <= 0f)
		{
			if(trimIdle)
			{
				string text = selectedObject.GetComponentInChildren<TextMesh>().text;
				selectedObject.GetComponentInChildren<TextMesh>().text = text.Remove (text.Length-1);
			}

			trimIdle = true;

			renameIdleTimer = 0.5f;

			if(!idleShow)
			{
				selectedObject.GetComponentInChildren<TextMesh>().text += "_";
			}
			else
			{
				selectedObject.GetComponentInChildren<TextMesh>().text += " ";
			}

			idleShow = !idleShow;
		}
	}
	// Update all game timers
	void UpdateTimers()
	{
		float dtime = Time.deltaTime;
		// Input lock timer
		if(lockTimer <= 0f)
		{
			lockInputs = false;
			lockTimer = 0f;
		}
		else
		{
			lockTimer -= dtime;
		}

		// Idle timer
		if(idled)
		{
			idleTimer += dtime;
		}
		else
		{
			idleTimer = 0f;
		}

		// dclick
		if(doubleClick > 0f)
		{
			doubleClick -= dtime;
		}

		if(renameIdleTimer > 0f)
		{
			renameIdleTimer -= dtime;
		}

		if(inputTimer > 0f)
		{
			inputTimer -= dtime;
		}

	}

	void TextLoop(string inputString, Queue<IGameInput> inputs)
	{
		string com = inputString;
		
		while(inputs.Count > 0)
		{
			IGameInput input = inputs.Dequeue();
			
			if(input is KeyDown)
			{
				if((input as KeyDown).key == KeyCode.Return)
				{
					rename = false;
					if(trimIdle)
					{
						string text = selectedObject.GetComponentInChildren<TextMesh>().text;
						selectedObject.GetComponentInChildren<TextMesh>().text = text.Remove (text.Length-1);
						trimIdle = false;
					}
					return;
				}
				if((input as KeyDown).key == KeyCode.Backspace)
				{
					inputTimer = 0.05f;
					map.RemoveLastCharOfWord();
					trimIdle = false;
				}
			}
		}
		if( inputString != "")
		{
			trimIdle = false;
			inputTimer = 0.05f;
			map.AddWords(com);
		}
		if(renameStarted)
		{
			renameStarted = false;
			RenameIdle();
		}
		else
		{
			renameIdleTimer = 0.4f;
		}
	}

	// Checking inputs
	void CheckInputs(Queue<IGameInput> inputs)
	{
		// we did not idle
		idled = false;

		// Get the command buttons
		bool command1 = inputSystem.GetCommand1Down();
		bool command2 = inputSystem.GetCommand2Down();

		// Clear the keyDowed list for use
		keysDownedLastTime = new List<KeyCode>();

		// While we have inputs descipher them one by one

		// This will be changed when the command pattern is implemented
		while(inputs.Count > 0)
		{
			IGameInput ip = inputs.Dequeue();

			if(ip is KeyDown)
			{
				KeyDown key = (ip as KeyDown);
				KeyCode code = key.key;
				
				if(keysDowned.BinarySearch(code) < 0)
				{
					switch (code) {
					case KeyCode.A:
						Bootstrap.PrintMessage("A");
						myFileSystem.SetRelativePath ("mynewtest2");
						myFileSystem.SaveTextFile (map.GetAsStringList ());
						break;
					case KeyCode.S:
						Bootstrap.PrintMessage("S");
						map.ClearSelected ();
						myFileSystem.SetRelativePath ("mynewtest2");
						map.SetModel (myFileSystem.LoadTextFile ());
						map.SetView (Bootstrap.node, Bootstrap.edge);
						map.StartView ();
						map.Initialize ();
						bounds = map.GetBounds ();
						break;
					case KeyCode.D:
						Bootstrap.PrintMessage("D");
						map.ClearData ();
						break;
					}
					keysDownedLastTime.Add(code);
				}
				else
				{
					keysDownedLastTime.Add(code);
				}
			}
			else if(ip is MouseUp)
			{

				if(moveSelected)
				{
					bounds = map.GetBounds();
					moveCamera = false;
					updateCameraToPointer = false;
					moveCameraWithDrag = false;
					moveSelected = false;
					updateSelectedToPointer = false;
					
				}
				else if(moveCamera)
				{
					moveCamera = false;
					updateCameraToPointer = false;
					slowCameraMovement = true;
					slowDownTimer = slowDownTimerBase;
					slowDragSpeed = dragSpeed;
				}
				
			}
			else if(ip is MouseDown)
			{

				if(!moveSelected && selectedObject != null && Vector3.Distance(dragPosition,mousePosition) > 2)
				{
					moveSelected = true;
				}

				if(moveSelected)
				{
					// update selected to pointer
					updateSelectedToPointer = true;
					mousePosition = (ip as MouseDown).pos;
					CursorScript.state = CursorScript.MouseState.drag;
				}
				else if(moveCamera)
				{
					// update camera to pointer
					updateCameraToPointer = true;
					mousePosition = (ip as MouseDown).pos;
					CursorScript.state = CursorScript.MouseState.drag;
				}
				else
				{
					mousePosition = (ip as MouseDown).pos;
				}
				
			}
			else if(ip is MouseClick)
			{
				mousePosition = (ip as MouseClick).pos;

				if(doubleClick <= 0f)
				{
					doubleClick = 0.2f;
				}
				else
				{
					dclick = true;
				}
				
				if(dclick)
				{
					dclick = false;
					
					GameObject target = Bootstrap.CheckRay((ip as MouseClick).pos);
					
					if(target != null && target == selectedObject && selectedObject.tag == "Node")
					{
						// Start renaming
						rename = true;
						renameStarted = true;
					}
				}
				else 
				{
					GameObject target = Bootstrap.CheckRay((ip as MouseClick).pos);
					
					if(target != null)
					{
						if(command2)
						{
							// Remove target
							UnselectObject();
							RemoveObject(target);
						}
						else if(command1 && selectedObject != null && target.tag == "Node" && selectedObject.tag == "Node")
						{
							// Check if connected if not then create edge between selected and target
							// check also if both are actually nodes!
							
							if(!map.IsConnected(selectedObject.GetComponent<NodeMono>(),target.GetComponent<NodeMono>()))
							{
								map.CreateEdge(selectedObject.GetComponent<NodeMono>(),target.GetComponent<NodeMono>());
							}
						}
						else
						{
							//Select target
							SelectObject(target);

							dragPosition = (ip as MouseClick).pos;
						}
					}
					else
					{
						UnselectObject();
						
						if(command1)
						{
							// Create Node here
							Vector3 pos = Camera.main.ScreenToWorldPoint((ip as MouseClick).pos);
							if(lockZ)
							{
								pos.z = locked_z;
							}
							
							if(!CheckCollisionSphere(pos,LayerMask.NameToLayer("Node"),null))
								map.CreateNode(pos);
							else
								Bootstrap.PrintMessage("No Room");
						}
						else
						{
							moveCamera = true;
							dragPosition = (ip as MouseClick).pos;
						}
					}
				}
			}
			else if(ip is MouseWheel)
			{
				orthChange = true;
				
				if((ip as MouseWheel).up)
				{
					orthRise = true;
				}
				else
				{
					orthRise = false;
				}
			}
			else if(ip is MouseRightClick)
			{
				GameObject target = Bootstrap.CheckRay((ip as MouseRightClick).pos);
				
				if(target != null && target.tag == "Node" && selectedObject != null)
				{
					map.CycleState();
				}
			}
		}

		// set the keys that were pressed to the checking list for next update
		keysDowned = keysDownedLastTime;
	}

	// Doing stuff that happen when no inputs are given (Not ran all the time)
	void Idle()
	{
		idled = true;
		keysDowned.Clear();
	}
	void UpdateCursor()
	{
		GameObject target = Bootstrap.CheckRay(Input.mousePosition);
		
		if(target != null)
		{
			if(map.GetSelected() == target && target.tag != "Edge")
			{
				CursorScript.state = CursorScript.MouseState.hand;
			}
			else
			{
				CursorScript.state = CursorScript.MouseState.point;
			}
		}
		else
		{
			CursorScript.state = CursorScript.MouseState.hand;
		}


	}
	public void LockInputs(float time)
	{
		lockTimer = time;
		lockInputs = true;
	}

	public void UpdateCamera()
	{

		if(updateCameraToPointer && !moveCameraWithDrag)
		{

			lastMousePosition = Camera.main.ScreenToViewportPoint(mousePosition - dragPosition);
			float x = lastMousePosition.x * dragSpeed;
			float y = lastMousePosition.y * dragSpeed;

			if(x > maxXSpeed | x < -maxXSpeed)
			{
				if( x < -maxXSpeed)
				{
					x = -maxXSpeed;
				} 
				else
				{
					x = maxXSpeed;
				}
			}
			if(y > maxYSpeed | y < -maxYSpeed)
			{
				if( y < -maxYSpeed)
				{
					y = -maxYSpeed;
				}
				else
				{
					y = maxYSpeed;
				}
			}

			Vector3 move = new Vector3(x, y, 0);
			move = RestrictMovement(-move);
			Camera.main.transform.Translate(move, Space.World);
		}
		else if(updateCameraToPointer && moveCameraWithDrag)
		{
			lastMousePosition = Camera.main.ScreenToViewportPoint(mousePosition - dragPosition);
			float x = lastMousePosition.x * 1;
			float y = lastMousePosition.y * 1;
			
			Vector3 move = new Vector3(x , y , 0);

			if(move.x > 0)
				move.x = 0.75f;
			if(move.x < 0)
				move.x = -0.75f;
			if(move.y > 0)
				move.y = 0.75f;
			if(move.y < 0)
				move.y = -0.75f;

			Camera.main.transform.Translate(move, Space.World);
			//BoundCamera();
		}
		else if(slowCameraMovement)
		{
			slowDownTimer -= Time.deltaTime;

			if(slowDownTimer > 0)
			{
				slowDragSpeed = slowDragSpeed - slowDragSpeed/(slowDownTimer/Time.deltaTime);
				Vector3 move = new Vector3(lastMousePosition.x * slowDownTimer, lastMousePosition.y * slowDownTimer, 0);
				move = RestrictMovement(-move);
				Camera.main.transform.Translate(move, Space.World);
			}
			else
			{
				slowCameraMovement = false;
			}

		}

		if(orthChange)
		{
			if(orthRise)
				Camera.main.orthographicSize++;
			else
				Camera.main.orthographicSize--;

			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax );

			orthChange = false;
		}

	}
	public void UpdateSelected()
	{
		if(updateSelectedToPointer)
		{


			Vector3 vOrig = Camera.main.ScreenToViewportPoint(mousePosition);
			Vector3 v = BoundToViewport(vOrig);
			//bool cameraIsNearEdge = IsNearEdge(v);

			if(vOrig != v)
			{
				moveCamera = true;
				updateCameraToPointer = true;
				moveCameraWithDrag = true;
			}
			else
			{
				moveCamera = false;
				updateCameraToPointer = false;
			}

			v = Camera.main.ViewportToWorldPoint(v);

			if(lockZ)
				v.z = locked_z;

		
			if(!CheckCollisionSphere(v,LayerMask.NameToLayer("Node"),selectedObject))
			{
				map.MoveSelected(v);
			}
		}
	}
	/*private void drawLine(Vector3 start,Vector3 end)
	{
		drawStart = true;
		lineStart = start;
		lineEnd = end;
	}*/

	// Check that node can be placed to a valid position
	private bool CheckCollisionSphere(Vector3 position, int layer,GameObject ignoreThis)
	{
		bool returnValue = false;
		CapsuleCollider cc = null;
		Vector3 start = position;
		Vector3 end = position;
		float rad = 0f;


		if(ignoreThis != null)
		{
			cc = ignoreThis.transform.GetComponentInChildren<CapsuleCollider>();
			cc.gameObject.layer -= layer;

			float x = ((cc.height)*(cc.transform.parent.localScale.y-cc.transform.parent.localScale.x));
			start.x -= x;
			end.x += x;
			rad = cc.radius*cc.transform.parent.localScale.x;
		}
		else
		{
			start.x -= 4;
			end.x += 4;
			rad = 2.5f;

		}



		int mask = 1 << layer;

		if(Physics.CheckCapsule(start,end,rad,mask))
			returnValue = true;
		else
			returnValue = false;
		

		if(selectedObject != null)
		{
			cc.gameObject.layer += layer;
		}

		return returnValue;
	}
	/*private void BoundCamera()
	{
		Vector3 pos = Camera.main.transform.position;

		if(pos.x < bounds.x)
			pos.x = bounds.x;
		if(pos.x > bounds.y)
			pos.x = bounds.y;
		if(pos.y < bounds.z)
			pos.y = bounds.z;
		if(pos.y > bounds.w)
			pos.y = bounds.w;

		Camera.main.transform.position = pos;
	}*/

	private Vector3 RestrictMovement(Vector3 direction)
	{
		Vector3 pos = Camera.main.transform.position;

		if(pos.x < bounds.x && direction.x < 0)
			direction.x = 0;
		if(pos.x > bounds.y && direction.x > 0)
			direction.x = 0;
		if(pos.y < bounds.z && direction.y < 0)
			direction.y = 0;
		if(pos.y > bounds.w && direction.y > 0)
			direction.y = 0;

		return direction;
	}

	public void UnselectObject()
	{
		map.Unselect();
		selectedObject = null;
	}
	public void SelectObject(GameObject ob)
	{/*

		if(selectedObject != null)
		{

			selectedObject.SendMessage("Unselect");
		}
	*/


		selectedObject = ob;
		//selectedObject.SendMessage("Select");

		if(ob.tag == "Node")
		{
			map.Select(ob.GetComponent<NodeMono>());
		}
		else if(ob.tag == "Edge")
		{
			
			map.Select(ob.GetComponent<EdgeMono>());
		}
	}
	public void RemoveObject(GameObject ob)
	{
		if(ob.tag == "Node")
		{
			map.Kill(ob.GetComponent<NodeMono>());
		}
		else if(ob.tag == "Edge")
		{
			
			map.Kill(ob.GetComponent<EdgeMono>());
		}
	}
	/*public void CreateMap(List<string> mapdata)
	{
		string[] attributes;
		string[] attributesValues;
		string[] positionAsStrings = null;

		int id = -1;
		int headid = -1;
		int tailid = -1;
		string word = "";
		NodeModel.NodeState state = NodeModel.NodeState.Hidden;
		Vector3 position = Vector3.zero;

		foreach(string line in mapdata)
		{
			attributes = line.Split(':');

			if(attributes[0] == "node")
			{
				for(int i = 1; i < attributes.Length; i++)
				{
					attributesValues = attributes[i].Split('=');

					switch(attributesValues[0])
					{
						case "position" :	positionAsStrings = attributesValues[1].Split(',');
											position = new Vector3(
												int.Parse(positionAsStrings[0]),
												int.Parse(positionAsStrings[1]),
												int.Parse(positionAsStrings[2]));											
											break;

						case "id" : 		id = int.Parse(attributesValues[1]);
											break;

						case "word" : 		word = attributesValues[1];
											break;

						case "state" : 		state = (NodeModel.NodeState)int.Parse(attributesValues[1]);
											break;
					}
				}

				NodeController ncon = new NodeController();
				ncon.CreateNode(position,word,state,id);
				ncon.CreateView(Instantiate(node,Vector3.zero,node.transform.rotation) as GameObject);
				ncon.Initialize();
				nodes.Add(ncon);

			}
			else if(attributes[0] == "edge")
			{
				for(int i = 1; i < attributes.Length; i++)
				{
					attributesValues = attributes[i].Split('=');

					switch(attributesValues[0])
					{
						case "headid" :	headid = int.Parse(attributesValues[1]);											
										break;
						
						case "tailid" :	tailid = int.Parse(attributesValues[1]);
										break;
					}
				}

				EdgeController vcon = new EdgeController();
				vcon.ConnectEdge((nodes.GetNode(headid).Model as NodeModel),(nodes.GetNode(tailid).Model as NodeModel));
				vcon.CreateView(Instantiate(edge,Vector3.zero,Quaternion.identity) as GameObject);
				vcon.Initialize();
				
				edges.Add(vcon);
			}

		}
	}*/
	public void SaveMap()
	{
		List<string> comp = map.GetAsStringList();
		myFileSystem.SetRelativePath("testxml");
		myFileSystem.SaveXmlFile(comp);
	}
	public static void SetBounds(Vector4 bound)
	{
		Vector4 new_vec = new Vector4(-10,10,-10,10);
		bounds = new_vec + bound;
	}
	public static Vector3 BoundToViewport(Vector3 position)
	{
		
		if(position.x < 0)
			position.x = 0;
		if(position.x > 1)
			position.x = 1;
		if(position.y < 0)
			position.y = 0;
		if(position.y > 1)
			position.y = 1;
		
		return position;
	}
	bool IsNearEdge (Vector3 position)
	{
		if(position.x < 0.1f)
			return true;
		if(position.x > 1)
			return true;
		if(position.y < 0)
			return true;
		if(position.y > 1)
			return true;
		return false;
	}
}
