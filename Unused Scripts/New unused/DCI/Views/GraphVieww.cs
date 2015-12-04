using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DCI.Roles;

public class GraphVieww : View
{
	DCI.Roles.GameGraph gameGraph;
	GameObject viewObject;
	Dictionary<string,View> nodes;
	Dictionary<string,View> edges;
	List<Observer> _observers;
	bool _changed;
	//List<View> nodes;
	//List<View> edges;

	public GraphVieww(DCI.Roles.GameGraph gameGraph, Dictionary<string,Vector3> nodePos)
	{
		this.gameGraph = gameGraph;
		this.nodes = new Dictionary<string,View>();
		this.edges = new Dictionary<string,View>();

		viewObject = new GameObject(this.gameGraph.Name);

		foreach(DCI.Roles.GameNode node in this.gameGraph.GetNodes())
		{
			nodes.Add(node.Id,new NodeVieww(node,nodePos[node.Id],viewObject.transform));
		}
		foreach(DCI.Roles.GameEdge edge in this.gameGraph.GetEdges())
		{
			edges.Add(edge.Head.Id+edge.Tail.Id,new EdgeVieww(edge,viewObject.transform));
		}
	}

	public void Select(string id, bool isNode)
	{
		if(isNode)
		{
			//nodes[id].Select();
		}
		else
		{
			//edges[id].Select();
		}
	}
	public void Unselect(string id, bool isNode)
	{
		if(isNode)
		{
			//nodes[id].Unselect();
		}
		else
		{
			//edges[id].Unselect();
		}
	}

	#region implemented abstract members of View
	/*
	public override void Strip ()
	{
		//foreach(View view in nodes)
		//{
		//	view.Strip();
		//}

		this.QuitObserving(gameGraph);
		gameGraph = null;
		GameObject.Destroy(viewObject);
		viewObject = null;

		foreach(View view in edges.Values)
		{
			view.Strip();
		}
		foreach(View view in nodes.Values)
		{
			view.Strip();
		}

		nodes = null;
		edges = null;
	}

	public override void Update (string message)
	{
		if(message == "delete")
		{
			this.Strip();
			return;
		}
	}
	*/
	public bool UpdateNodes(List<string> ids)
	{
		bool ret = false;
		while(ids.Count > 0)
		{
			string id = ids[0];
			ids.RemoveAt(0);

			if(nodes.ContainsKey(id))
			{
				ret = true;
				///nodes[id].Update();
			}
		}
		return ret;
	}
	public bool UpdateEdges(List<string> ids)
	{
		bool ret = false;
		while(ids.Count > 0)
		{
			string id = ids[0];
			ids.RemoveAt(0);
			
			if(edges.ContainsKey(id))
			{
				ret = true;
				///edges[id].Update();
			}
		}
		return ret;
	}
	/*
	public override void Select ()
	{
		throw new System.NotImplementedException ();
	}
	
	public override void Unselect ()
	{
		throw new System.NotImplementedException ();
	}*/
	#endregion

	#region implemented abstract members of View

	public override List<Observer> Observers {
		get {
			return _observers;
		}
		set {
			_observers = value;
		}
	}

	public override bool Changed {
		get {
			return _changed;
		}
		set {
			_changed = value;
		}
	}

	#endregion
}
