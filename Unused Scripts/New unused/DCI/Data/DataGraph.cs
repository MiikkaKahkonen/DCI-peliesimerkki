using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DCI.Roles;

public class DataGraph : Data, DCI.Roles.GameGraph
{
	private GameNodeList _nodes;
	private GameEdgeList _edges;

	private Dictionary<string,Vector3> _nodePos;

	private string _name = "Graph";

	private GameGraph _data;

	private List<DCI.Roles.Observer> _observers;
	private bool _changed;

	public DataGraph()
	{
		_nodes = new GameNodeList();
		_edges = new GameEdgeList();
		_observers = new List<Observer>();
		_nodePos = new Dictionary<string,Vector3>();
		_changed = false;
	}

	#region GameGraph implementation

	public ICollection<DCI.Roles.GameNode> GetNodes()
	{
		return _nodes.Values;
	}

	public ICollection<DCI.Roles.GameEdge> GetEdges()
	{
		return _edges.Values;
	}

	public void AddNode (DCI.Roles.GameNode node)
	{
		_nodes.Add(node);
	}

	public void RemoveNode (DCI.Roles.GameNode node)
	{
		_nodes.Remove(node);
	}

	public void AddEdge (DCI.Roles.GameEdge edge)
	{
		_edges.Add(edge);
	}

	public void RemoveEdge (DCI.Roles.GameEdge edge)
	{
		_edges.Remove(edge);
	}


	public DCI.Roles.GameNode GetNode (string key)
	{
		if(_nodes.ContainsKey(key))
			return _nodes[key];
		return null;
	}

	public DCI.Roles.GameEdge GetEdge (string key)
	{
		if(_edges.ContainsKey(key))
			return _edges[key];
		return null;
	}

	public DCI.Roles.GameNode CreateNewNode ()
	{
		return new DataNode();
	}

	public DCI.Roles.GameEdge CreateNewEdge ()
	{
		return new DataEdge();
	}

	public ICollection<DCI.Roles.GameNode> GetConnectedNodes(DCI.Roles.GameNode node)
	{
		List<DCI.Roles.GameNode> nodes = new List<DCI.Roles.GameNode>();

		foreach(DCI.Roles.GameEdge edge in this.GetEdges())
		{
			if(edge.Head == node)
			{
				nodes.Add(edge.Tail);
			}
			else if(edge.Tail == node)
			{
				nodes.Add(edge.Head);
			}
		}

		return nodes;
	}

	public DCI.Roles.GameNode GetNodeWithWord(string word)
	{
		foreach(DCI.Roles.GameNode node in this.GetNodes())
		{
			if(word == node.Word)
			{
				return node;
			}
		}

		return null;
	}

	public string Name {
		get {
			return _name;
		}
		set {
			_name = value;
		}
	}

	public Dictionary<string, Vector3> NodePositions {
		get {
			return _nodePos;
		}
		set {
			_nodePos = value;
		}
	}

	#endregion

	#region Observable implementation

	public List<Observer> Observers {
		get {
			return _observers;
		}
		set {
			_observers = value;
		}
	}

	public bool Changed {
		get {
			return _changed;
		}
		set {
			_changed = value;
		}
	}

	#endregion



}
