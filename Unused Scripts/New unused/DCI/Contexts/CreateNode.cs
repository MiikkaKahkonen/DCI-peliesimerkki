using UnityEngine;
using System.Collections;
using DCI.Roles;

public class CreateNode : BaseContext
{
	GameGraph gameGraph;

	string id;
	//Vector3 position;
	string word;
	Manager.DCINodeState state;


	public CreateNode(GameGraph gameGraph, string id, string word,Manager.DCINodeState state)
	{
		this.gameGraph = gameGraph;
		this.id = id;
		//this.position = position;
		this.word = word;
		this.state = state;
	}


	public override void Execute ()
	{
		base.Execute ();

		GameNode newNode = gameGraph.CreateNewNode();
		
		newNode.Id = id;
		//newNode.Position = position;
		newNode.Word = word;
		newNode.CreateEmptyWord();
		newNode.State = state;
		
		
		gameGraph.AddNode(newNode);

	}
}
