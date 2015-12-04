using UnityEngine;
using System.Collections;
using DCI.Roles;
using System.Collections.Generic;

public class GameContext : BaseContext, VariableStorage, Observer
{
	DCI.Roles.GameGraph gameGraph;
	DCI.Roles.GameCamera gameCamera;
	Context CameraMove;
	GraphVieww view;

	// Game variables
	Vector4 bounds;

	Dictionary<string,bool> boolVariables;
	Dictionary<string,float> floatVariables;
	Dictionary<string, int> intVariables;
	Dictionary<string,Vector3> vector3Variables;
	Dictionary<string,float> timers;

	Dictionary<string,Vector3> NodePositions;

	bool initialized = false;
	InputSystem inputSystem;

	// Dragging for objects
	GameObject selectedNode = null;
	GameNode selectedGameNode = null;
	GameObject selectedEdge = null;
	GameEdge selectedGameEdge = null;
	GameObject quessingPane = null;
	bool objectSelected = false;
	string selectedId = "";
	
	// Keeping track of pressed keys and making sure
	// no extra presses are made
	List<KeyCode> keysDowned = new List<KeyCode>();
	List<KeyCode> keysDownedLastTime = new List<KeyCode>();

	public GameContext(DCI.Roles.GameGraph gameGraph,DCI.Roles.GameCamera gameCamera, InputSystem inputSystem, GameObject pane)
	{
		this.gameGraph = gameGraph;
		this.inputSystem = inputSystem;
		this.quessingPane = pane;
		this.gameCamera = gameCamera;
		this.NodePositions = gameGraph.NodePositions;
		this.CameraMove = new MoveCamera(this.gameCamera,this);

	}

	public bool Execute ()
	{
		if(!initialized)
		{
			Initialize();
		}

		Queue<IGameInput> inputs = inputSystem.GetInputs();
		string inputString = inputSystem.GetInputString();

		this.TimerVar("dtime", Time.deltaTime);
		this.UpdateTimers();

		// if:
		// we have inputs and the inputs are not locked then and not collecting text from keyboard
		// act on the inputs 
		// else if:
		// we are renaming and getting inputs 
		// else if:
		// renaming without inputs
		// else:
		// do idle stuff, update camera pointer, update dragging etc.
		
		if(!this.BoolVar("lockInputs") && !this.BoolVar("rename") && !this.BoolVar("quessing"))
		{
			CheckInputs(inputs);
		}
		else if(this.BoolVar("quessing"))
		{
			if(this.TimerVar("inputTimer") <= 0f)
				QuessingLoop(inputString,inputs);
		}
		else if(this.BoolVar("rename") && this.TimerVar("inputTimer") <= 0f)
		{
			RenamingLoop(inputString,inputs);
		}
		else if(this.BoolVar("rename"))
		{
			RenameIdle();
		}
		else
		{
			Idle ();
		}
		
		// Update Camera and Dragged objects
		UpdateSelected();
		CameraMove.Execute();

		// Update mouse cursor
		UpdateCursor();


		/*
		Context context3 = new Quess(gameGraph,"first");
		context3.Execute();

		Context context2 = new Quess(gameGraph,"word1");
		context2.Execute();

		Context context4 = new Quess(gameGraph,"second");
		context4.Execute();

		Context context5 = new Quess(gameGraph,"third");
		context5.Execute();

		Context context6 = new Quess(gameGraph,"sixth");
		context6.Execute();

		Context context7 = new Quess(gameGraph,"fifth");
		context7.Execute();

		Context context8 = new Quess(gameGraph,"fourth");
		context8.Execute();

		*/

		//gameGraph.RemoveNode(gameGraph.GetNode("1"),"delete");

		//gameGraph.ClearGraph();

		//Manager.Message("Reports",gameGraph.Report());

		//UninitializeScene();
		return true;
	}
	private void Initialize()
	{
		base.Execute ();
		InitVars();
		//bounds = gameGraph.GetBounds();
		InitializeScene();
		
		this.Observe(gameGraph);
		initialized = true;
	}
	private void InitializeScene()
	{
		view = new GraphVieww(gameGraph,NodePositions);
		quessingPane.GetComponent<MeshRenderer>().material.color = Color.gray;
	}
	private void UninitializeScene()
	{
		//view.Strip();
	}
	// Checking inputs
	void CheckInputs(Queue<IGameInput> inputs)
	{
		// we did not idle
		this.BoolVar("idled",false);
		
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
				
				if(!keysDowned.Contains(code)) //keysDowned.BinarySearch(code) < 0)
				{
					switch (code) {
						case KeyCode.A:
							Manager.Message("User","A");
							break;
						case KeyCode.S:
							Manager.Message("User","S");
							break;
						case KeyCode.D:
							Manager.Message("User","D");
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
				
				if(this.BoolVar("moveSelected"))
				{
					if(selectedNode != null)
						bounds = gameGraph.UpdateBounds(bounds,SelectedGameNode);
					this.BoolVar("moveCamera",false);
					this.BoolVar("updateCameraToPointer",false);
					this.BoolVar("moveCameraWithDrag",false);
					this.BoolVar("moveSelected",false);
					this.BoolVar("updateSelectedToPointer",false);
					
				}
				else if(this.BoolVar("moveCamera"))
				{
					this.BoolVar("moveCamera",false);
					this.BoolVar("updateCameraToPointer",false);
					this.BoolVar("slowCameraMovement",false);
					this.FloatVar("slowDownTimer",this.FloatVar("slowDownTimerBase"));
					this.FloatVar("slowDragSpeed",this.FloatVar("dragSpeed"));
				}
				
			}
			else if(ip is MouseDown)
			{
				
				if(!this.BoolVar("moveSelected") && objectSelected && Vector3.Distance(this.Vector3Var("dragPosition"),this.Vector3Var("mousePosition")) > 2)
				{
					this.BoolVar("moveSelected",true);
				}
				
				if(this.BoolVar("moveSelected"))
				{
					// update selected to pointer

					this.BoolVar("updateSelectedToPointer",true);
					this.Vector3Var("mousePosition",(ip as MouseDown).pos);
					CursorScript.state = CursorScript.MouseState.drag;
				}
				else if(this.BoolVar("moveCamera"))
				{
					// update camera to pointer
					this.BoolVar("updateCameraToPointer",true);
					this.Vector3Var("mousePosition",(ip as MouseDown).pos);
					CursorScript.state = CursorScript.MouseState.drag;
				}
				else
				{
					this.Vector3Var("mousePosition",(ip as MouseDown).pos);
				}
				
			}
			else if(ip is MouseClick)
			{
				this.Vector3Var("mousePosition",(ip as MouseClick).pos);
				
				if(this.TimerVar("doubleClick") <= 0f)
				{
					this.TimerVar("doubleClick",0.2f);
				}
				else
				{
					this.BoolVar ("dclick",true);
				}
				
				if(this.BoolVar ("dclick"))
				{
					this.BoolVar ("dclick",false);
					
					GameObject target = Bootstrap.CheckRay((ip as MouseClick).pos);
					
					if(target != null && selectedNode != null)
					{
						// Start renaming
						this.BoolVar ("rename", true);
						this.BoolVar ("renameStarted", true);
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
						else if(command1 && objectSelected && target.tag == "Node" && selectedNode != null)
						{
							// Check if connected if not then create edge between selected and target
							// check also if both are actually nodes!
							//if(gameGraph.GetConnectedNodes(null))
							//{

							//}
							//if(!map.IsConnected(selectedObject.GetComponent<NodeMono>(),target.GetComponent<NodeMono>()))
							//{
							//	map.CreateEdge(selectedObject.GetComponent<NodeMono>(),target.GetComponent<NodeMono>());
							//}
						}
						else
						{
							//Select target
							SelectObject(target);
							
							this.Vector3Var("dragPosition",(ip as MouseClick).pos);
						}
					}
					else
					{
						UnselectObject();
						
						if(command1)
						{
							// Create Node here
							Vector3 pos = Camera.main.ScreenToWorldPoint((ip as MouseClick).pos);
							if(this.BoolVar ("lockZ"))
							{
								pos.z = this.FloatVar("locked_z");
							}
							
							//if(!CheckCollisionSphere(pos,LayerMask.NameToLayer("Node"),null))
							//	map.CreateNode(pos);
							//else
							//	Bootstrap.PrintMessage("No Room");
						}
						else
						{
							this.BoolVar ("moveCamera",true);
							this.Vector3Var("dragPosition",(ip as MouseClick).pos);
						}
					}
				}
			}
			else if(ip is MouseWheel)
			{
				this.BoolVar ("orthChange",true);
				
				if((ip as MouseWheel).up)
				{
					this.BoolVar ("orthRise",true);
				}
				else
				{
					this.BoolVar ("orthRise",false);
				}
			}
			else if(ip is MouseRightClick)
			{
				GameObject target = Bootstrap.CheckRay((ip as MouseRightClick).pos);
				
				if(objectSelected && selectedNode != null)
				{
					SelectedGameNode.CycleState();
				}
			}
		}
		
		// set the keys that were pressed to the checking list for next update
		keysDowned = keysDownedLastTime;
	}
	void UpdateCursor()
	{
		GameObject target = Bootstrap.CheckRay(Input.mousePosition);
		
		if(target != null)
		{
			if(objectSelected && selectedEdge == null && selectedNode == target)
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
	void RenamingLoop(string inputString, Queue<IGameInput> inputs)
	{
		string com = inputString;
		
		while(inputs.Count > 0)
		{
			IGameInput input = inputs.Dequeue();
			
			if(input is KeyDown)
			{
				if((input as KeyDown).key == KeyCode.Return)
				{
					this.BoolVar ("rename",false);

					if(this.BoolVar ("trimIdle"))
					{
						//string text = selectedObject.GetComponentInChildren<TextMesh>().text;
						//selectedObject.GetComponentInChildren<TextMesh>().text = text.Remove (text.Length-1);
						this.BoolVar ("trimIdle",false);
					}
					return;
				}
				if((input as KeyDown).key == KeyCode.Backspace)
				{
					this.TimerVar("inputTimer",0.05f);
					//map.RemoveLastCharOfWord();
					this.BoolVar ("trimIdle",false);
				}
			}
		}
		if( inputString != "")
		{
			this.BoolVar ("trimIdle",false);
			this.TimerVar("inputTimer",0.05f);
			//map.AddWords(com);
		}
		if(this.BoolVar("renameStarted"))
		{
			this.BoolVar("renameStarted",false);
			RenameIdle();
		}
		else
		{
			this.TimerVar("renameIdleTimer",0.4f);
		}
	}
	void QuessingLoop (string inputString, Queue<IGameInput> inputs)
	{
		string com = inputString;
		
		while(inputs.Count > 0)
		{
			IGameInput input = inputs.Dequeue();
			
			if(input is KeyDown)
			{
				if((input as KeyDown).key == KeyCode.Return)
				{
					if(quessingPane.GetComponentInChildren<TextMesh>().text != "")
					{

						Quess contextQuess = new Quess(gameGraph,quessingPane.GetComponentInChildren<TextMesh>().text);
						contextQuess.Execute();

						if(contextQuess.Result())
							quessingPane.GetComponent<MeshRenderer>().material.color = Color.green;
						else
							quessingPane.GetComponent<MeshRenderer>().material.color = Color.red;
						quessingPane.GetComponentInChildren<TextMesh>().text = "";
					}

					this.TimerVar("inputTimer",0.08f);
					return;
				}
				if((input as KeyDown).key == KeyCode.Escape)
				{
					quessingPane.GetComponent<MeshRenderer>().material.color = Color.gray;
					quessingPane.GetComponentInChildren<TextMesh>().text = "Quess...";
					this.TimerVar("inputTimer",0.08f);
					this.BoolVar("quessing",false);

					return;
				}
				if((input as KeyDown).key == KeyCode.Backspace)
				{
					string txt = quessingPane.GetComponentInChildren<TextMesh>().text;
					if(txt != "")
						txt = txt.Substring(0,txt.Length-1);
					quessingPane.GetComponentInChildren<TextMesh>().text = txt;
					this.TimerVar("inputTimer",0.08f);
				}
			}
		}
		if( inputString != "")
		{
			this.TimerVar("inputTimer",0.03f);
			quessingPane.GetComponentInChildren<TextMesh>().text += inputString;
		}
	}
	void RenameIdle ()
	{
		if(this.TimerVar("renameIdleTimer") <= 0f)
		{
			if(this.BoolVar ("trimIdle"))
			{
				//string text = selectedObject.GetComponentInChildren<TextMesh>().text;
				//selectedObject.GetComponentInChildren<TextMesh>().text = text.Remove (text.Length-1);
			}
			
			this.BoolVar ("trimIdle",true);
			
			this.TimerVar("renameIdleTimer",0.5f);

			if(!this.BoolVar ("idleShow"))
			{
				//selectedObject.GetComponentInChildren<TextMesh>().text += "_";
			}
			else
			{
				//selectedObject.GetComponentInChildren<TextMesh>().text += " ";
			}
			
			this.BoolVar ("idleShow",!this.BoolVar ("idleShow"));
		}
	}
	// Doing stuff that happen when no inputs are given (Not ran all the time)
	void Idle()
	{
		this.BoolVar ("idled",true);
		keysDowned.Clear();
	}
	public void UpdateSelected()
	{
		if(this.BoolVar("updateSelectedToPointer"))
		{
			Vector3 vOrig = Camera.main.ScreenToViewportPoint(this.Vector3Var("mousePosition"));
			Vector3 v = BoundToViewport(vOrig);
			//Vector3 v = Vector3.zero;
			bool cameraIsNearEdge = IsNearEdge(v);
			
			if(vOrig != v)
			{
				this.BoolVar ("moveCamera",true);
				this.BoolVar ("updateCameraToPointer",true);
				this.BoolVar ("moveCameraWithDrag",true);
			}
			else
			{
				this.BoolVar ("moveCamera",false);
				this.BoolVar ("updateCameraToPointer",false);
			}
			
			v = Camera.main.ViewportToWorldPoint(v);
			
			if(this.BoolVar ("lockZ"))
				v.z = this.FloatVar("locked_z");
			
			
			if(!CheckCollisionSphere(v,LayerMask.NameToLayer("Node"),selectedNode == null ? selectedEdge : selectedNode))
			{
				if(selectedNode != null)
				{
					/*if(selectedGameNode == null)
					{
						selectedGameNode = gameGraph.GetNode(selectedNode.GetComponent<NodeMonoView>().Id);
						selectedId = selectedGameNode.Id;
					}*/
					
					SelectedGameNode.Move(v);
				}
				//map.MoveSelected(v);
			}
		}
	}
	public void UnselectObject()
	{
		if(objectSelected)
		{
			if(selectedNode != null)
			{
				view.Unselect(selectedId,true);
			}
			if(selectedEdge != null)
			{
				view.Unselect(selectedId,false);
			}
		}
		objectSelected = false;
		selectedNode = null;
		selectedGameNode = null;
		selectedEdge = null;
		selectedGameEdge = null;
		selectedId = "";
	}
	public void SelectObject(GameObject ob)
	{

		if(objectSelected)
		{
			UnselectObject();
		}

		objectSelected = true;

		if(ob.tag == "Node")
		{
			selectedNode = ob;
			selectedId = ob.GetComponent<NodeMonoView>().Id;
			view.Select(selectedId,true);
		}
		else if(ob.tag == "Edge")
		{
			selectedEdge = ob;
			selectedId = ob.GetComponent<EdgeMonoView>().Id;
			view.Select(selectedId,false);
		}
		else if(ob.tag == "Quess")
		{
			this.BoolVar ("quessing",true);
			quessingPane.GetComponentInChildren<TextMesh>().text = "";
		}
	}
	public void RemoveObject(GameObject ob)
	{
		if(ob.tag == "Node")
		{
			selectedNode = null;
			selectedId = "";
			objectSelected = false;
			gameGraph.RemoveNode(SelectedGameNode,"delete");
			SelectedGameNode = null;

			//map.Kill(ob.GetComponent<NodeMono>());
		}
		else if(ob.tag == "Edge")
		{
			selectedEdge = null;
			selectedId = "";
			objectSelected = false;
			gameGraph.RemoveEdge(gameGraph.GetEdge(ob.GetComponent<EdgeMonoView>().Id),"delete");
			//map.Kill(ob.GetComponent<EdgeMono>());
		}
	}
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
		
		
		if(objectSelected)
		{
			cc.gameObject.layer += layer;
		}
		
		return returnValue;
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
	private GameNode SelectedGameNode
	{
		get
		{
			if(selectedGameNode != null)
				return selectedGameNode;
			else if(selectedNode != null)
			{
				selectedGameNode = gameGraph.GetNode(selectedId);
				return selectedGameNode;
			}
			else
			{
				return null;
			}
				
		}
		set
		{
			selectedGameNode = value;
		}
	}
	private GameEdge SelectedGameEdge
	{
		get
		{
			if(selectedGameEdge != null)
				return selectedGameEdge;
			else if(selectedEdge != null)
			{
				selectedGameEdge = gameGraph.GetEdge(selectedId);
				return selectedGameEdge;
			}
			else
			{
				return null;
			}
			
		}
		set
		{
			selectedGameEdge = value;
		}
	}

	#region VariableStorage implementation
	public Dictionary<string, bool> BoolVariables {
		get {
			return boolVariables;
		}
		set {
			boolVariables = value;
		}
	}
	public Dictionary<string, float> FloatVariables {
		get {
			return floatVariables;
		}
		set {
			floatVariables = value;
		}
	}
	public Dictionary<string, int> IntVariables {
		get {
			return intVariables;
		}
		set {
			intVariables = value;
		}
	}
	public Dictionary<string, Vector3> Vector3Variables {
		get {
			return vector3Variables;
		}
		set {
			vector3Variables = value;
		}
	}
	public Dictionary<string, float> Timers {
		get {
			return timers;
		}
		set {
			timers = value;
		}
	}
	#endregion

	#region Observer implementation

	public void Update (string message)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	private void InitVars()
	{
		this.BoolVariables = new Dictionary<string,bool>();
		this.FloatVariables = new Dictionary<string,float>();
		this.IntVariables = new Dictionary<string,int>();
		this.Vector3Variables = new Dictionary<string,Vector3>();
		this.Timers = new Dictionary<string,float>();
		
		//this.NodePositions = new Dictionary<string,Vector3>();
		
		// Camera Movement values
		this.FloatVar("dragSpeed",10);
		this.FloatVar("slowDownTimerBase",1);
		this.FloatVar("maxXSpeed",2);
		this.FloatVar("maxYSpeed",2);
		
		// Camera state values
		this.BoolVar("moveCamera",false);
		this.BoolVar("updateCameraToPointer",false);
		this.BoolVar("moveCameraWithDrag",false);
		this.BoolVar("slowCameraMovement",false);
		
		// Values for making a smooth ending to camera movement when camera is moved
		this.TimerVar("slowDownTimer",0);
		this.FloatVar("slowDragSpeed",0);
		this.Vector3Var("lastMousePosition",Vector3.zero);
		
		// Values for camera "zooming"
		this.BoolVar("orthChange",false);
		this.BoolVar("orthRise",false);
		this.IntVar("orthographicSizeMin",15);
		this.IntVar("orthographicSizeMax",15);
		
		// Values requried for drag and drop
		this.Vector3Var("dragPosition",Vector3.zero);
		this.Vector3Var("mousePosition",Vector3.zero);
		this.Vector3Var("clickOriginalPosition",Vector3.zero);
		
		//bool startDrag = false;
		this.BoolVar("moveSelected",false);
		this.BoolVar("updateSelectedToPointer",false);
		
		// Game state flags
		this.BoolVar("init",false);
		this.BoolVar("idled",false);
		this.BoolVar("dclick",false);
		this.BoolVar("rename",false);
		this.BoolVar("renameStarted",false);
		this.BoolVar("quessing",false);
		this.BoolVar("idleShow",false);
		this.BoolVar("trimIdle",false);
		
		this.BoolVar("lockInputs",true);
		
		// Game timers
		this.TimerVar("lockTimer",0.5f);
		this.TimerVar("idleTimer",0.0f);
		this.TimerVar("doubleClick",0.0f);
		this.TimerVar("renameIdleTimer",0.4f);
		this.TimerVar("inputTimer",0.05f);
		
		// LockZ and locked_z keeps the nodes and edges locked to specific (locked_z) z value
		this.BoolVar("lockZ",true);
		this.FloatVar("locked_z",0);
	}

}

/*
		float dragSpeed = 5;
		float slowDownTimerBase = 1f;
		float maxXSpeed = 1f;
		float maxYSpeed = 1f;

		Vector3 dragPosition;
		Vector3 mousePosition;
		Vector3 clickOriginalPosition;
		
		bool moveCamera = false;
		bool updateCameraToPointer = false;
		bool moveCameraWithDrag = false;
		

		bool slowCameraMovement = false;
		float slowDownTimer = 0;
		float slowDragSpeed = 0;
		Vector3 lastMousePosition;

		// Values for camera "zooming"
		bool orthChange = false;
		bool orthRise = false;
		int orthographicSizeMin = 15;
		int orthographicSizeMax = 40;


		//bool startDrag = false;
		bool moveSelected = false;
		bool updateSelectedToPointer = false;
		
		// Game state flags
		bool init = false;
		bool idled = false;
		bool lockInputs = true;
		bool dclick = false;
		bool rename = false;
		bool renameStarted = false;
		bool idleShow = false;
		bool trimIdle = false;
		
		// LockZ and locked_z keeps the nodes and edges locked to specific (locked_z) z value
		bool lockZ = true;
		float locked_z = 0;
		initialized = true;
		*/
