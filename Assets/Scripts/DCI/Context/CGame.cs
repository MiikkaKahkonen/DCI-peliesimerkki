using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGame : BaseContext, RVariableStorage
{
	public enum DCINodeState {AlwaysShown = 0,Quessed = 1,Quessable = 2, Hidden = 3}
	public enum DCIEdgeState {Shown = 0,Hidden = 1}

	RGameGraph gameGraph;
    RVariableStorage varStorage;

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
        varStorage = this;
        InitVars();
	}
    private void InitVars()
    {
        // Camera Movement values
        varStorage.FloatVar("dragSpeed", 10);
        varStorage.FloatVar("slowDownTimerBase", 1);
        varStorage.FloatVar("maxXSpeed", 2);
        varStorage.FloatVar("maxYSpeed", 2);

        // Camera state values
        varStorage.BoolVar("moveCamera", false);
        varStorage.BoolVar("updateCameraToPointer", false);
        varStorage.BoolVar("moveCameraWithDrag", false);
        varStorage.BoolVar("slowCameraMovement", false);
        varStorage.BoolVar("gameSaved", true);
        // Values for making a smooth ending to camera movement when camera is moved
        varStorage.TimerVar("slowDownTimer", 0);
        varStorage.FloatVar("slowDragSpeed", 0);
        varStorage.Vector3Var("lastMousePosition", Vector3.zero);

        // Values for camera "zooming"
        varStorage.BoolVar("orthChange", false);
        varStorage.BoolVar("orthRise", false);
        varStorage.IntVar("orthographicSizeMin", 15);
        varStorage.IntVar("orthographicSizeMax", 15);

        // Values requried for drag and drop
        varStorage.Vector3Var("dragPosition", Vector3.zero);
        varStorage.Vector3Var("mousePosition", Vector3.zero);
        varStorage.Vector3Var("clickOriginalPosition", Vector3.zero);

        //bool startDrag = false;
        varStorage.BoolVar("moveSelected", false);
        varStorage.BoolVar("updateSelectedToPointer", false);

        // Game state flags
        varStorage.BoolVar("init", false);
        varStorage.BoolVar("idled", false);
        varStorage.BoolVar("dclick", false);
        varStorage.BoolVar("rename", false);
        varStorage.BoolVar("renameStarted", false);
        varStorage.BoolVar("quessing", false);
        varStorage.BoolVar("idleShow", false);
        varStorage.BoolVar("trimIdle", false);
        varStorage.BoolVar("SaveGame", false);
        varStorage.BoolVar("CanExit", false);
        varStorage.BoolVar("AskExit", false);
        varStorage.BoolVar("lockInputs", true);
        varStorage.BoolVar("tryExit", false);

        // Game timers
        varStorage.TimerVar("lockTimer", 0.5f);
        varStorage.TimerVar("idleTimer", 0.0f);
        varStorage.TimerVar("doubleClick", 0.0f);
        varStorage.TimerVar("renameIdleTimer", 0.4f);
        varStorage.TimerVar("inputTimer", 0.05f);
        varStorage.TimerVar("flashTimer", 0.0f);

        // LockZ and locked_z keeps the nodes and edges locked to specific (locked_z) z value
        varStorage.BoolVar("lockZ", true);
        varStorage.FloatVar("locked_z", 0);
    }

    public new void Execute()
	{
        //base.Execute();
        varStorage.TimerVar ("dtime",Time.deltaTime);
		UpdateTimers();
        
		if(varStorage.BoolVar("quessing") && varStorage.TimerVar("inputTimer") <= 0)
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
		else if(varStorage.BoolVar("moveCamera") && varStorage.TimerVar("inputTimer") <= 0)
		{
			Queue<IGameInput> inputQueue = inputSystem.GetInputs();

			while(inputQueue.Count > 0)
			{
				IGameInput ip = inputQueue.Dequeue();

				if(ip is MouseUp)
				{
                    varStorage.BoolVar("moveCamera",false);
				}
				else if(ip is MouseDown)
				{
                    varStorage.Vector3Var("mousePosition",(ip as MouseDown).pos);
				}
			}
		}
		else if(varStorage.BoolVar("tryExit") && varStorage.TimerVar("inputTimer") <= 0)
		{
            quitContext = new CQuit(inputSystem);

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
                    varStorage.BoolVar("tryExit",false);
				}
			}
		}
        else if (varStorage.TimerVar("inputTimer") <= 0)
		{
			Queue<IGameInput> inputQueue = inputSystem.GetInputs();
			string inputString = inputSystem.GetInputString();

			while(inputQueue.Count > 0)
			{
				IGameInput ip = inputQueue.Dequeue();

				if(ip is KeyDown)
				{
                    varStorage.TimerVar("inputTimer",1f);

					if((ip as KeyDown).key == KeyCode.F5)
					{
                        varStorage.BoolVar("SaveGame",true);
                        varStorage.TimerVar("inputTimer",0.5f);
					}
				}

				if(ip is MouseClick)
				{
					GameObject ob = Manager.CheckRay((ip as MouseClick).pos);

					if(ob != null)
					{
						if(ob.tag == "Quess")
						{
                            varStorage.BoolVar("quessing",true);
                            varStorage.TimerVar("inputTimer",0.2f);
						}
						else if(ob.tag == "Exit")
						{
							_nodePositions = gameView.GetNodePositions();
                            varStorage.TimerVar("inputTimer",0.2f);
                            varStorage.BoolVar("tryExit",true);
						}
					}
				}

				if(ip is MouseDown)
				{
					GameObject ob = Manager.CheckRay((ip as MouseDown).pos);

					if(ob == null)
					{
                        varStorage.BoolVar("moveCamera",true);
                        varStorage.Vector3Var("mousePosition",(ip as MouseDown).pos);
					}
				}
			}
		}

		if(varStorage.BoolVar ("SaveGame"))
		{
            varStorage.BoolVar("SaveGame",false);
            varStorage.BoolVar ("gameSaved",true);
            varStorage.TimerVar("inputTimer",1f);
			_nodePositions = gameView.GetNodePositions();
			CSaveGame saveGame = new CSaveGame(gameGraph,BaseFileSystem.GetInstance(),_nodePositions);
			saveGame.Execute();
		}

		if(varStorage.BoolVar("moveCamera"))
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
		float dtime = varStorage.TimerVar ("dtime");

		// Input lock timer
		if(varStorage.TimerVar("lockTimer") <= 0f)
		{
			varStorage.BoolVar("lockInputs",false);
			varStorage.TimerVar("lockTimer",0f);
		}
		else
		{
			varStorage.TimerVar("lockTimer", varStorage.TimerVar("lockTimer")- dtime);
		}
		
		// Idle timer
		if(varStorage.BoolVar("idled"))
		{
			varStorage.TimerVar("idleTimer", varStorage.TimerVar("idleTimer")+ dtime);
		}
		else
		{
			varStorage.TimerVar("idleTimer", 0f);
		}
		
		// dclick
		if(varStorage.TimerVar("doubleClick") > 0f)
		{
			varStorage.TimerVar("doubleClick",varStorage.TimerVar("doubleClick")- dtime);
		}
		
		if(varStorage.TimerVar("renameIdleTimer") > 0f)
		{
			varStorage.TimerVar("renameIdleTimer",varStorage.TimerVar("renameIdleTimer")-dtime);
		}
		
		if(varStorage.TimerVar("inputTimer")  > 0f)
		{
			varStorage.TimerVar("inputTimer",varStorage.TimerVar("inputTimer")-dtime);
		}

		if(varStorage.TimerVar("flashTimer") > 0f)
		{
			varStorage.TimerVar("flashTimer",varStorage.TimerVar("flashTimer")-dtime);

		}
		
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
