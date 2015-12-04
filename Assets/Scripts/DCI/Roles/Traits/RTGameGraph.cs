using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class RTGameGraph
{
	public static bool CreateGraph(this RGameGraph self, List<string> lines,Dictionary<string,Vector3> nodePositions)
	{
		try{
			int c = 0;
			int isNode = 0;
			Vector3 position = Vector3.zero;
			string id = "";
			string word = "";

			CGame.DCINodeState state = CGame.DCINodeState.Hidden;
			
			
			string headId = "";
			string tailId = "";
			
			foreach(string realLine in lines)
			{
				// Set defaults
				id = "-1";
				word = "UNSET";
				state = CGame.DCINodeState.Hidden;
				
				
				headId = "-1";
				tailId = "-1";
				
				
				string line = realLine;
				line = line.Trim();
				// print text
				Manager.Message("Debug","Line "+c.ToString()+": "+line);
				c++;
				
				string[] variables = line.Split(BaseFileSystem.ATTSEP);
				foreach(string variable in variables)
				{
					string[] variableValue = variable.Split(BaseFileSystem.VALSEP);
					
					switch(variableValue[0])
					{
					case "node" : isNode = 1;
						break;
					case "edge" : isNode = 0;
						break;
					case "pos" : isNode = -1;
						break;
					case "position" : string[] positionValues = variableValue[1].Split(',');
						float x = float.Parse(positionValues[0]);
						float y = float.Parse(positionValues[1]);
						float z = float.Parse(positionValues[2]);
						position = new Vector3(x,y,z);
						break;
					case "id" : id = variableValue[1];
						break;
					case "word" : word = variableValue[1]; 
						break;
					case "head" : headId = variableValue[1];
						break;
					case "tail" : tailId = variableValue[1];
						break;
					case "state" : state = (CGame.DCINodeState)(int.Parse(variableValue[1]));
						break;
					}
				}
				
				if(isNode == 1)
				{
					// Create Node
					self.Nodes.Add(self.CreateNode(id, word,state));

				}
				else if(isNode == 0)
				{
					// Create Edge
					self.Edges.Add(self.CreateEdge(headId,tailId));
				}
				else if (isNode == -1)
				{
					nodePositions[id] = position;
				}
			}
			
			foreach(RGameNode node in self.Nodes.Values)
			{
				if(!nodePositions.ContainsKey(node.Id))
				{
					nodePositions[node.Id] = CGame.GetRandomNodePosition();
				}
			}
		}
		catch(System.SystemException e)
		{
			Manager.Message("Exceptions",e.ToString());
			return false;
		}
		return true;
	}
	public static RGameNode CreateNode(this RGameGraph self,string id,string word, CGame.DCINodeState state)
	{
		RGameNode newNode = self.NewNode;
		newNode.Id = id;
		newNode.Word = word;
		newNode.State = (int)state;
		newNode.SetEWord();

		return newNode;
	}
	public static RGameEdge CreateEdge(this RGameGraph self, string headId, string tailId)
	{
		RGameEdge newEdge = self.NewEdge;
		newEdge.Id = headId+tailId;
		newEdge.HeadId = headId;
		newEdge.TailId = tailId;

		return newEdge;
	}
	public static Vector3 GetNextNodePosition(this RGameGraph self)
	{
		Vector3 newvec = Vector3.zero;
		newvec.x = Random.Range(0,100);
		newvec.y = Random.Range(0,100);
		newvec.z = 2;
		return newvec;
	}
	public static string Quess(this RGameGraph self, string quess)
	{
		foreach(RGameNode node in self.Nodes.Values)
		{
			if(node.State == 2)
			{
				if(node.CompaireQuess(quess))
				{
					node.State = 1;
					return node.Id;
				}

			}
		}

		return "";
	}
	public static List<RGameNode> UpdateNeighbours(this RGameGraph self, string id)
	{
		List<RGameNode> list = new List<RGameNode>();

		if(self.Nodes.ContainsKey(id))
		{
			CGame.DCINodeState newStateAtMost = CGame.DCINodeState.Hidden;

			if(self.Nodes[id].State == 1)
				newStateAtMost = CGame.DCINodeState.Quessable;

			foreach(RGameEdge edge in self.Edges.Values)
			{
				if(edge.HeadId == id)
				{
					if(self.Nodes[edge.TailId].State > (int)newStateAtMost)
						self.Nodes[edge.TailId].State = (int)newStateAtMost;

					//list.Add(new NodePair(self.Nodes[edge.HeadId],self.Nodes[edge.TailId]));
					list.Add(self.Nodes[edge.TailId]);
				}
				else if(edge.TailId == id)
				{
					if(self.Nodes[edge.HeadId].State > (int)newStateAtMost)
						self.Nodes[edge.HeadId].State = (int)newStateAtMost;

					list.Add(self.Nodes[edge.HeadId]);
					//list.Add(new NodePair(self.Nodes[edge.HeadId],self.Nodes[edge.TailId]));
				}
			}
		}

		return list;
	}
}