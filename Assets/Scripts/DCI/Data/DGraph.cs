using UnityEngine;
using System.Collections;

public class DGraph : Data, RGameGraph
{
	GameNodeList _nodes;
	GameEdgeList _edges;

	public DGraph() : base()
    {
		_nodes = new GameNodeList();
		_edges = new GameEdgeList();
	}
	#region RGameGraph implementation
	public GameNodeList Nodes {
		get {
			return _nodes;
		}
		set {
			_nodes = value;
		}
	}
	public GameEdgeList Edges {
		get {
			return _edges;
		}
		set {
			_edges = value;
		}
	}
	public RGameNode NewNode {
		get {
			return new DNode();
		}
	}

	public RGameEdge NewEdge {
		get {
			return new DEdge();
		}
	}

	#endregion
}
