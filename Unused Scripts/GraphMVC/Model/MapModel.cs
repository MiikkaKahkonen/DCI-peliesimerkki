using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapModel
{

	private NodeList nodes;
	private EdgeList edges;
	private Vector4 bounds;
	//> For testing
	List<Vector3> positions;
	List<string> words;

	//<NodeController> nodes;
	//List<EdgeController> edges;
	
	//List<NodeController> nodesToUse;

	//<

	public MapModel()
	{
		nodes = new NodeList();
		edges = new EdgeList();
		bounds = Vector4.zero;
	}
	
	public MapModel(List<string> mapdata)
	{
		nodes = new NodeList();
		edges = new EdgeList();
		CreateMap(mapdata);
		bounds = Vector4.zero;

	}
	public NodeController CreateNode(Vector3 position, string word, NodeModel.NodeState state,int id)
	{
		if(nodes == null)
			nodes = new NodeList();

		NodeController ncon = new NodeController();
		ncon.CreateNode(position,word,state,id);
		nodes.Add(ncon);

		return ncon;
	}
	public EdgeController CreateEdge(NodeModel head,NodeModel tail)
	{
		if(edges == null)
			edges = new EdgeList();

		EdgeController vcon = new EdgeController();
		vcon.ConnectEdge(head,tail);
		edges.Add(vcon);
		return vcon;
	}
	private void CreateMap(List<string> mapdata)
	{
		string[] attributes;
		string[] attributesValues;
		string[] positionAsStrings = null;
		
		int id = -1;
		int headid = -1;
		int tailid = -1;
		string word = "";
		NodeModel.NodeState state = NodeModel.NodeState.Hidden;
		Vector3 position = Vector3.zero;
		
		foreach(string line in mapdata)
		{
			attributes = line.Split(':');
			
			if(attributes[0] == "node")
			{
				for(int i = 1; i < attributes.Length; i++)
				{
					attributesValues = attributes[i].Split('=');
					
					switch(attributesValues[0])
					{
					case "position" :	positionAsStrings = attributesValues[1].Split(',');
						position = new Vector3(
							int.Parse(positionAsStrings[0]),
							int.Parse(positionAsStrings[1]),
							int.Parse(positionAsStrings[2]));											
						break;
						
					case "id" : 		id = int.Parse(attributesValues[1]);
						break;
						
					case "word" : 		word = attributesValues[1];
						break;
						
					case "state" : 		state = (NodeModel.NodeState)int.Parse(attributesValues[1]);
						break;
					}
				}
				CreateNode(position,word,state,id);

			}
			else if(attributes[0] == "edge")
			{
				for(int i = 1; i < attributes.Length; i++)
				{
					attributesValues = attributes[i].Split('=');
					
					switch(attributesValues[0])
					{
					case "headid" :	headid = int.Parse(attributesValues[1]);											
						break;
						
					case "tailid" :	tailid = int.Parse(attributesValues[1]);
						break;
					}
				}

				CreateEdge((nodes.GetNode(headid).Model as NodeModel),(nodes.GetNode(tailid).Model as NodeModel));
			}
			
		}
	}
	public void Initialize()
	{
		foreach(NodeController controller in nodes)
		{
			AddBounds(controller.Initialize());
		}
		foreach(EdgeController controller in edges)
		{
			controller.Initialize();
		}
	}
	public NodeList GetNodes()
	{
		return nodes;
	}
	public void Quess (string word)
	{
		nodes.Quess(word);
	}

	public EdgeList GetEdges()
	{
		return edges;
	}

	public List<string> ToListString ()
	{
		List<string> data = nodes.ToListString();
		data.AddRange(edges.ToListString());
		return data;
	}



	public void Die()
	{
		foreach(NodeController controller in nodes)
		{
			controller.Die();
		}
		//foreach(EdgeController controller in edges)
		//{
		//	controller.Die();
		//}
		nodes.Clear();
		edges.Clear ();
	}

	/*public void GenerateNodes(List<Vector3> positions, List<string> words)
	{
		NodeModel.NodeState state = NodeModel.NodeState.Hidden;
		
		for(int i = 0; i < positions.Count; i++)
		{
			if(i == 0 || i == 4)
				state = NodeModel.NodeState.Revealed;
			else
				state = NodeModel.NodeState.Hidden;
			
			NodeController ncon = new NodeController();
			ncon.CreateNode(positions[i],words[i],state, -1);
			//ncon.CreateView(GameObject.Instantiate(node,Vector3.zero,node.transform.rotation) as GameObject);
			//ncon.Initialize();
			nodes.Add(ncon);
		}
		
		nodesToUse.Add(nodes[0]);
		nodesToUse.Add(nodes[1]);
		
		nodesToUse.Add(nodes[0]);
		nodesToUse.Add(nodes[2]);
		
		nodesToUse.Add(nodes[2]);
		nodesToUse.Add(nodes[3]);
		
		nodesToUse.Add(nodes[3]);
		nodesToUse.Add(nodes[4]);
		
		nodesToUse.Add(nodes[2]);
		nodesToUse.Add(nodes[5]);
		
		for(int i = 0; i < nodesToUse.Count; i = i+2)
		{
			EdgeController vcon = new EdgeController();
			vcon.ConnectEdge(nodesToUse[i].Model as NodeModel,nodesToUse[i+1].Model as NodeModel);
			//vcon.CreateView(GameObject.Instantiate(edge,Vector3.zero,Quaternion.identity) as GameObject);
			//vcon.Initialize();
			
			edges.Add(vcon);
		}
	}*/
	/*void createTestData ()
	{
		positions = new List<Vector3>();
		words = new List<string>();

		positions.Add(new Vector3(0,0,100));
		positions.Add(new Vector3(10,0,100));
		positions.Add(new Vector3(0,10,100));
		positions.Add(new Vector3(10,10,100));
		positions.Add(new Vector3(20,10,100));
		positions.Add(new Vector3(10,20,100));
		
		words.Add("Start");
		words.Add("word1");
		words.Add("word2");
		words.Add("word3");
		words.Add("Five");
		words.Add("Six");
		
		GenerateNodes(positions, words);
	}*/
	public Vector4 GetBounds()
	{
		return bounds;
	}
	public void AddBounds(Vector3 bound)
	{

		if(bound.x < bounds.x)
			bounds.x = bound.x;
		if(bound.x > bounds.y)
			bounds.y = bound.x;
		if(bound.y < bounds.z)
			bounds.z = bound.y;
		if(bound.y > bounds.w)
			bounds.w = bound.y;
	}

	public EdgeController IsConnected (int node1, int node2)
	{
		return edges.ContainsSym(node1,node2);
	}

	public void GetNewBounds ()
	{
		bounds = Vector4.zero;
		foreach(NodeController node in nodes)
		{
			AddBounds(node.GetBounds());
		}
	}
}
