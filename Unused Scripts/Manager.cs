using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour 
{
	
	public const string DEFAULT_SAVE_NAME = "default.txt",
						DEFAULT_LOAD_NAME = "default_load.txt",
						GAME_LOAD_FAILED = "FAILED",
						GRAPH_STORAGE_ERROR = "FAILED";



	//private BaseFileSystem fileSystem = BaseFileSystem.GetInstance();
	//private ITextFileSystem fileSystem = BaseFileSystem.GetInstance();

	private static Manager instance;
	private int messageNumber = 1;

	private bool messageRecieved = false;
	private Dictionary<string,string> messages = new Dictionary<string, string>();
	private Dictionary<string,string> messageHistory = new Dictionary<string, string>();

	private CGame gameContext = null;
	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 30;
		instance = this;

		DGraph dg = new DGraph();
		Dictionary<string,Vector3> nodePositions = new Dictionary<string, Vector3>();

		CLoadGame lgc = new CLoadGame(dg,nodePositions,BaseFileSystem.GetInstance());
		lgc.Execute();

		gameContext = new CGame(dg,nodePositions,InputSystem.GetInstance());
	}


	void Update()
	{
		gameContext.Execute();
		printMessages();
	}

	public static void Message(string topic,string message)
	{

		Manager.instance.messageRecieved = true;
		if(Manager.instance.messages.ContainsKey(topic))
		{
			Manager.instance.messages[topic] += Manager.instance.messageNumber +": " + message + "\n";
		}
		else
		{
			Manager.instance.messages[topic] = Manager.instance.messageNumber +": " + message + "\n";
		}
		Manager.instance.messageNumber++;
	}

	private void printMessages()
	{
		if(messageRecieved)
		{
			foreach(string key in messages.Keys)
			{
				string output = "Topic: "+key+":\n";
				output += messages[key];
				output += "End Topic "+key+".";	
				print(output);
				if(messageHistory.ContainsKey(key))
				{
					messageHistory[key] += messages[key];
				}
				else
				{
					messageHistory.Add(key,messages[key]);
				}
			}
			messages.Clear();
			messageRecieved = false;
		}
	}
	public static GameObject CheckRay(Vector3 position,string[] tags)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(position);
		
		if (Physics.Raycast (ray, out hit))
		{
			for(int i = 0; i < tags.Length;i++)
			{
				if(hit.collider.tag == tags[i])
				{

					return hit.collider.gameObject;
				}
			}
		}
		return null;
	}
	public static GameObject CheckRay(Vector3 position)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(position);
		
		if (Physics.Raycast (ray, out hit))
		{
			if(hit.collider.tag == "Node" || hit.collider.tag == "Edge" || hit.collider.tag == "Quess" || hit.collider.tag == "Exit")
			{
				if(hit.collider.transform.parent.renderer.enabled)
				{
					return hit.collider.transform.parent.gameObject;
				}
			}
		}
		return null;
	}

	public static RGameCamera GetCamera ()
	{
		DCamera dCam = new DCamera(Camera.main);
		return dCam;
	}

	void OnApplicationQuit()
	{

		
		/*
		if(!gameContext.BoolVar("CanExit"))
		{
			Application.CancelQuit();
			gameContext.BoolVar("AskExit",true);
		}
		*/
	}
}
