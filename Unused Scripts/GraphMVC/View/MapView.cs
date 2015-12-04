using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapView
{
	private GameObject node;
	private GameObject edge;

	List<NodeView> nodeViews;
	List<EdgeView> edgeViews;

	public MapView(GameObject node, GameObject edge)
	{
		this.node = node;
		this.edge = edge;
		nodeViews = new List<NodeView>();
		edgeViews = new List<EdgeView>();
	}
	public void CreateNodeView(NodeController node)
	{
		nodeViews.Add(node.CreateView(GameObject.Instantiate(this.node,Vector3.zero,this.node.transform.rotation) as GameObject));
	}
	public void CreateNodeViews(NodeList nodes)
	{
		foreach( NodeController node in nodes)
		{
			CreateNodeView(node);
		}
	}
	public void CreateEdgeViews(EdgeList edges)
	{
		foreach( EdgeController edge in edges)
		{
			CreateEdgeView(edge);
		}
	}

	public void CreateEdgeView(EdgeController edge)
	{
		edgeViews.Add(edge.CreateView(GameObject.Instantiate(this.edge,Vector3.zero,Quaternion.identity) as GameObject));
	}

	public void Die()
	{
		nodeViews.Clear ();
		edgeViews.Clear();
	}
}
