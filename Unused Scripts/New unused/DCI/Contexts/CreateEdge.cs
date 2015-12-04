using UnityEngine;
using System.Collections;
using DCI.Roles;

public class CreateEdge : BaseContext 
{
	GameGraph gameGraph;
	GameNode head;
	GameNode tail;

	public CreateEdge(GameGraph gameGraph, GameNode head, GameNode tail)
	{
		this.gameGraph = gameGraph;
		this.head = head;
		this.tail = tail;
	}

	public override void Execute ()
	{
		base.Execute ();

		GameEdge newEdge = gameGraph.CreateNewEdge();

		newEdge.Head = head;
		newEdge.Tail = tail;

		newEdge.Observe(head);
		newEdge.Observe(tail);

		if((int)head.State < (int)Manager.DCINodeState.Quessable || (int)tail.State < (int)Manager.DCINodeState.Quessable)
			newEdge.State = Manager.DCIEdgeState.Shown;
		else
			newEdge.State = Manager.DCIEdgeState.Hidden;

		gameGraph.AddEdge(newEdge);

	}
}
