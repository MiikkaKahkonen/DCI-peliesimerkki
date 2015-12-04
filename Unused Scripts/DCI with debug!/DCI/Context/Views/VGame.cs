using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VGame: View 
{

	GameObject NodeGO;
	GameObject EdgeGO;

	Dictionary<string, VNodeMono> _nodes;
	Dictionary<string, VEdgeMono> _edges;

	private GameObject nodeParent;
	private GameObject edgeParent;

	public VGame (RGameGraph gameGraph, Dictionary<string, Vector3> _nodePositions)
	{
		NodeGO = GameObject.Find("ViewAssets/NodeAsset");
		EdgeGO = GameObject.Find("ViewAssets/EdgeAsset");

		nodeParent = GameObject.Find("Nodes");
		edgeParent = GameObject.Find("Edges");

		_nodes = new Dictionary<string, VNodeMono>();
		_edges = new Dictionary<string, VEdgeMono>();
		GameObject go;

		foreach(RGameNode gnode in gameGraph.Nodes.Values)
		{
			go = GameObject.Instantiate(NodeGO,_nodePositions[gnode.Id],NodeGO.transform.rotation) as GameObject;
			go.transform.parent = nodeParent.transform;
			go.name = gnode.Id;
			_nodes.Add(gnode.Id,go.GetComponent<VNodeMono>());

			UpdateNode(gnode);
		}

		foreach(RGameEdge gedge in gameGraph.Edges.Values)
		{
			go = GameObject.Instantiate(EdgeGO,Vector3.zero,NodeGO.transform.rotation) as GameObject;
			go.transform.parent = edgeParent.transform;
			go.name = gedge.Id;
			_edges.Add(gedge.Id,go.GetComponent<VEdgeMono>());
			UpdateEdge(gameGraph.Nodes[gedge.HeadId],gameGraph.Nodes[gedge.TailId]);
		}


	}
	public void UpdateNode(RGameNode node)
	{
		if(_nodes.ContainsKey(node.Id))
		{
			VNodeMono vnm = _nodes[node.Id];

			if((int)node.State < 3)
			{
				vnm.EnableRenderer(true);

				if(node.State == 2)
				{
					vnm.SetWord(node.EWord);
				}
				else
				{
					vnm.SetWord(node.Word);
				}
			}
		}
	}
	public void UpdateEdge(RGameNode head, RGameNode tail)
	{
		if(_edges.ContainsKey(head.Id+tail.Id))
		{
			VEdgeMono vem = _edges[head.Id+tail.Id];
			
			if((int)head.State < 2 || (int)tail.State < 2)
			{
				vem.EnableRenderer(true);
			}
			else
			{
				vem.EnableRenderer(false);
			}

			if(_nodes.ContainsKey(head.Id))
				vem.SetStart(_nodes[head.Id].GetPosition());

			if(_nodes.ContainsKey(tail.Id))
				vem.SetEnd(_nodes[tail.Id].GetPosition());
		}
		else if(_edges.ContainsKey(tail.Id+head.Id))
		{
			VEdgeMono vem = _edges[tail.Id+head.Id];
			
			if(head.State < 2 || tail.State < 2)
			{
				vem.EnableRenderer(true);
			}
			else
			{
				vem.EnableRenderer(false);
			}
			
			if(_nodes.ContainsKey(head.Id))
				vem.SetStart(_nodes[tail.Id].GetPosition());
			
			if(_nodes.ContainsKey(tail.Id))
				vem.SetEnd(_nodes[head.Id].GetPosition());
		}
	}
	public void UpdateNodes(List<RGameNode> nodes)
	{
		foreach(RGameNode node in nodes)
		{
			UpdateNode(node);
		}
	}

	public void UpdateEdges(List<NodePair> edges)
	{
		foreach(NodePair edge in edges)
		{
			UpdateEdge(edge.Head,edge.Tail);
		}
	}

	public Dictionary<string,Vector3> GetNodePositions()
	{
		Dictionary<string,Vector3> positions = new Dictionary<string, Vector3>();

		foreach(string key in _nodes.Keys)
		{
			positions.Add(key,_nodes[key].GetPosition());
		}

		return positions;
	}
	
}
