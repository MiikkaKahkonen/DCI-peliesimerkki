using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputSystem : MonoBehaviour {

	public static bool ReturnStrings = false;

	private static InputSystem Instance;

	Queue<IGameInput> inputs = null;
	string inputString = "";

	float time = 0;
	bool command1Down = false;
	bool command2Down = false;

	public KeyCode command1;
	public KeyCode command2;

	public static InputSystem GetInstance()
	{
		return Instance;
	}

	void Awake()
	{
		Instance = this;
	}
	// Use this for initialization
	void Start () {

		inputString = "";
		inputs = new Queue<IGameInput>();

		FormInputs();

		if(command1 == KeyCode.None)
		{
			command1 = KeyCode.LeftControl;
		}
		if(command2 == KeyCode.None)
		{
			command2 = KeyCode.LeftShift;
		}
	}
	
	// Update is called once per frame
	void Update () {

		time += Time.deltaTime;

		//if(time > 0.2f)
			FormInputs();
	}

	void FormInputs()
	{
		//Debug.Log ("i'm alive");
		inputs.Clear();
		time = 0;
		command1Down = false;
		command2Down = false;
		inputString = "";
		// Check regural inputs
		if(Input.anyKey)
		{
			inputString = Input.inputString;
			// Check command keys first
			if(Input.GetKey (command1))
			{
				command1Down = true;
			}
			if(Input.GetKey(command2))
			{
				command2Down = true;
			}

			if(Input.GetKey(KeyCode.A))
			{
				inputs.Enqueue(new KeyDown(KeyCode.A));
			}
			if(Input.GetKey(KeyCode.S))
			{
				inputs.Enqueue(new KeyDown(KeyCode.S));
			}
			if(Input.GetKey(KeyCode.D))
			{
				inputs.Enqueue(new KeyDown(KeyCode.D));
			}
			if(Input.GetKey(KeyCode.Return))
			{
				inputs.Enqueue( new KeyDown(KeyCode.Return));
			}
			if(Input.GetKey(KeyCode.Backspace))
			{
				inputs.Enqueue( new KeyDown(KeyCode.Backspace));
			}
			if(Input.GetKey(KeyCode.Escape))
			{
				inputs.Enqueue(new KeyDown(KeyCode.Escape));
			}
			if(Input.GetKey(KeyCode.F5))
			{
				inputs.Enqueue(new KeyDown(KeyCode.F5));
			}
		}

		// Check mouse inputs
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			inputs.Enqueue(new MouseWheel(true));
		}
		else if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			inputs.Enqueue(new MouseWheel(false));
		}

		if(Input.GetMouseButtonUp(0))
		{
			// Released
			inputs.Enqueue(new MouseUp(Input.mousePosition));
		}
		else if(Input.GetMouseButtonDown(0))
		{
			// Pressed
			inputs.Enqueue(new MouseClick(Input.mousePosition));
		}
		else if(Input.GetMouseButton(0))
		{
			// Held
			inputs.Enqueue(new MouseDown(Input.mousePosition));
		}
		else if(Input.GetMouseButtonUp(1))
		{
			inputs.Enqueue(new MouseRightClick(Input.mousePosition));
		}
	}

	public Queue<IGameInput> GetInputs()
	{
		return inputs;
	}
	public string GetInputString()
	{
		return inputString;
	}
	public bool GetCommand1Down()
	{
		return command1Down;
	}
	public bool GetCommand2Down()
	{
		return command2Down;
	}
	
}
