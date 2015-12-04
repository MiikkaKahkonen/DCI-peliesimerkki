using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGame : BaseContext, RVariableStorage
{
	public enum DCINodeState {AlwaysShown = 0,Quessed = 1,Quessable = 2, Hidden = 3}
	public enum DCIEdgeState {Shown = 0,Hidden = 1}

	RGameGraph gameGraph;
	CQuess quessContext;
	CUpdateCamera updateCamera;
	InputSystem inputSystem;
	CQuit quitContext;

	Dictionary<string,bool> _boolVariables;
	Dictionary<string,float> _floatVariables;
	Dictionary<string, int> _intVariables;
	Dictionary<string,Vector3> _vector3Variables;
	Dictionary<string,float> _timers;
	
	Dictionary<string,Vector3> _nodePositions;

	VGame gameView;

	public CGame(RGameGraph gameGraph,Dictionary<string,Vector3> nodePositions,InputSystem inputSystem)
	{

		_boolVariables = new Dictionary<string, bool>();
		_floatVariables = new Dictionary<string, float>();
		_intVariables = new Dictionary<string, int>();
		_vector3Variables = new Dictionary<string, Vector3>();
		_timers = new Dictionary<string, float>();

		this.gameGraph = gameGraph;
		_nodePositions = nodePositions;
		this.inputSystem = inputSystem;
		gameView = new VGame(gameGraph,_nodePositions);
		quessContext = new CQuess(gameGraph,this.inputSystem,this);

		this.updateCamera = new CUpdateCamera(this);

		InitVars();
	}

	public new void Execute()
	{
		//base.Execute();
		this.TimerVar ("dtime",Time.deltaTime);
		UpdateTimers();

		if(this.TimerVar("inputTimer") <= 0)
		{

			if(this.BoolVar("quessing"))
			{
				string result = quessContext.Execute();

				if(result != "")
				{
					if(result != "exit")
					{
						gameView.UpdateNode(gameGraph.Nodes[result]);
						List<RGameNode> list = gameGraph.UpdateNeighbours(result);
						gameView.UpdateNodes(list);
						List<NodePair> pairs = new List<NodePair>();
						foreach(RGameNode node in list)
						{
							pairs.Add(new NodePair(gameGraph.Nodes[result],node));
						}
						gameView.UpdateEdges(pairs);
					}
				}
			}
			else if(this.BoolVar("moveCamera"))
			{
				Queue<IGameInput> inputQueue = inputSystem.GetInputs();

				while(inputQueue.Count > 0)
				{
					IGameInput ip = inputQueue.Dequeue();

					if(ip is MouseUp)
					{
						this.BoolVar("moveCamera",false);
					}
					else if(ip is MouseDown)
					{
						this.Vector3Var("mousePosition",(ip as MouseDown).pos);
					}
				}
			}
			else if(this.BoolVar("tryExit"))
			{
				int result = quitContext.Execute();
				if(result > 0)
				{
					if(result < 3)
					{
						if(result == 1)
						{
							_nodePositions = gameView.GetNodePositions();
							CSaveGame saveGame = new CSaveGame(gameGraph,BaseFileSystem.GetInstance(),_nodePositions);
							saveGame.Execute();
						}
#if UNITY_EDITOR

						UnityEditor.EditorApplication.isPlaying = false;
#else
						Application.Quit();
#endif
					}
					else
					{
						this.BoolVar("tryExit",false);
					}
				}
			}
			else
			{
				Queue<IGameInput> inputQueue = inputSystem.GetInputs();
				string inputString = inputSystem.GetInputString();

				while(inputQueue.Count > 0)
				{
					IGameInput ip = inputQueue.Dequeue();

					if(ip is KeyDown)
					{
						this.TimerVar("inputTimer",1f);

						if((ip as KeyDown).key == KeyCode.F5)
						{
							this.BoolVar("SaveGame",true);
							this.TimerVar("inputTimer",0.5f);
						}
					}

					if(ip is MouseClick)
					{
						GameObject ob = Manager.CheckRay((ip as MouseClick).pos);

						if(ob != null)
						{
							if(ob.tag == "Quess")
							{
								this.BoolVar("quessing",true);
								this.TimerVar("inputTimer",0.2f);
							}
							else if(ob.tag == "Exit")
							{
								_nodePositions = gameView.GetNodePositions();
								quitContext = new CQuit(inputSystem);
								this.TimerVar("inputTimer",0.2f);
								this.BoolVar("tryExit",true);
							}
						}
					}

					if(ip is MouseDown)
					{
						GameObject ob = Manager.CheckRay((ip as MouseDown).pos);

						if(ob == null)
						{
							this.BoolVar("moveCamera",true);
							this.Vector3Var("mousePosition",(ip as MouseDown).pos);
						}
					}
				}
			}
		}

		if(this.BoolVar ("SaveGame"))
		{
			this.BoolVar("SaveGame",false);
			this.BoolVar ("gameSaved",true);
			this.TimerVar("inputTimer",1f);
			_nodePositions = gameView.GetNodePositions();
			CSaveGame saveGame = new CSaveGame(gameGraph,BaseFileSystem.GetInstance(),_nodePositions);
			saveGame.Execute();
		}

		if(this.BoolVar("moveCamera"))
		{
			updateCamera.Execute();
		}


		//base.Done();
	}

	public static Vector3 GetRandomNodePosition()
	{
		Vector3 newvec = Vector3.zero;
		newvec.x = Random.Range(0,100);
		newvec.y = Random.Range(0,100);
		newvec.z = 2;
		return newvec;
	}

	// Update all game timers
	private void UpdateTimers()
	{
		RVariableStorage self = this;

		float dtime = self.TimerVar ("dtime");

		// Input lock timer
		if(self.TimerVar("lockTimer") <= 0f)
		{
			self.BoolVar("lockInputs",false);
			self.TimerVar("lockTimer",0f);
		}
		else
		{
			self.TimerVar("lockTimer", self.TimerVar("lockTimer")- dtime);
		}
		
		// Idle timer
		if(self.BoolVar("idled"))
		{
			self.TimerVar("idleTimer", self.TimerVar("idleTimer")+ dtime);
		}
		else
		{
			self.TimerVar("idleTimer", 0f);
		}
		
		// dclick
		if(self.TimerVar("doubleClick") > 0f)
		{
			self.TimerVar("doubleClick",self.TimerVar("doubleClick")- dtime);
		}
		
		if(self.TimerVar("renameIdleTimer") > 0f)
		{
			self.TimerVar("renameIdleTimer",self.TimerVar("renameIdleTimer")-dtime);
		}
		
		if(self.TimerVar("inputTimer")  > 0f)
		{
			self.TimerVar("inputTimer",self.TimerVar("inputTimer")-dtime);
		}

		if(self.TimerVar("flashTimer") > 0f)
		{
			self.TimerVar("flashTimer",self.TimerVar("flashTimer")-dtime);

		}
		
	}

	private void InitVars()
	{
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
		this.BoolVar("gameSaved",true);
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
		this.BoolVar("SaveGame",false);
		this.BoolVar("CanExit",false);
		this.BoolVar("AskExit",false);
		this.BoolVar("lockInputs",true);
		this.BoolVar("tryExit",false);
		// Game timers
		this.TimerVar("lockTimer",0.5f);
		this.TimerVar("idleTimer",0.0f);
		this.TimerVar("doubleClick",0.0f);
		this.TimerVar("renameIdleTimer",0.4f);
		this.TimerVar("inputTimer",0.05f);
		this.TimerVar("flashTimer",0.0f);
		
		// LockZ and locked_z keeps the nodes and edges locked to specific (locked_z) z value
		this.BoolVar("lockZ",true);
		this.FloatVar("locked_z",0);
	}
	#region RVariableStorage implementation

	public System.Collections.Generic.Dictionary<string, bool> BoolVariables {
		get {
			return _boolVariables;
		}
		set {
			_boolVariables = value;
		}
	}

	public System.Collections.Generic.Dictionary<string, float> FloatVariables {
		get {
			return _floatVariables;
		}
		set {
			_floatVariables = value;
		}
	}

	public System.Collections.Generic.Dictionary<string, int> IntVariables {
		get {
			return _intVariables;
		}
		set {
			_intVariables = value;
		}
	}

	public System.Collections.Generic.Dictionary<string, Vector3> Vector3Variables {
		get {
			return _vector3Variables;
		}
		set {
			_vector3Variables = value;
		}
	}

	public System.Collections.Generic.Dictionary<string, float> Timers {
		get {
			return _timers;
		}
		set {
			_timers = value;
		}
	}

	#endregion


}
