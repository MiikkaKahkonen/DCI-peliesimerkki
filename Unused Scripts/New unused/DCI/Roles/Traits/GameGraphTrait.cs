using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DCI.Roles
{
	public static class GameGraphTrait	
	{
		public static GameNode CheckAnswer(this GameGraph self, string word)
		{
			GameNode node = self.GetNodeWithWord(word);

			if(node == null)
				return null;

			if(node.State == Manager.DCINodeState.Quessable)
			{
				return node;
			}
			else
			{
				return null;
			}
		}
		public static ICollection<DCI.Roles.GameNode> Reveal(this GameGraph self, GameNode nodeToReveal)
		{
			nodeToReveal.Reveal();
			return self.GetConnectedNodes(nodeToReveal);
		}
		public static string GraphToText(this GameGraph self)
		{
			string edgeStart = "edge";
			string nodeStart = "node";
			
			string attSep = ""+BaseFileSystem.ATTSEP;
			string valSep = ""+BaseFileSystem.VALSEP;
			string axisSep = ""+BaseFileSystem.AXISSEP;
			
			string text = "";
			
			foreach(DCI.Roles.GameNode node in self.GetNodes())
			{
				text += nodeStart+attSep+
					"position"+valSep+node.Position.x.ToString("f2")+axisSep+
						node.Position.y.ToString("f2")+axisSep+
						node.Position.z.ToString("f2")+axisSep+attSep+
						"id"+valSep+node.Id+attSep+
						"word"+valSep+node.Word+attSep+
						"state"+valSep+(int)node.State+"\n";
			}
			
			foreach(DCI.Roles.GameEdge edge in self.GetEdges())
			{
				text += edgeStart+attSep+
					"head"+valSep+edge.Head.Id+attSep+
					"tail"+valSep+edge.Tail.Id+attSep+
					"state"+valSep+(int)edge.State+"\n";
			}

			return text;
		}
		public static GameGraph TextToGraph(this GameGraph self, string text)
		{
			List<string> list = new List<string>(text.Split('\n'));
			int c = 0;
			
			int isNode = 0;
			Vector3 position = Vector3.zero;
			string id = "";
			string word = "";
			Manager.DCINodeState state = Manager.DCINodeState.Hidden;

			
			string headId = "";
			string tailId = "";

			foreach(string realLine in list)
			{
				// Set defaults
				id = "-1";
				word = "UNSET";
				state = Manager.DCINodeState.Hidden;
				
				
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
					case "state" : state = (Manager.DCINodeState)(int.Parse(variableValue[1]));
						break;
					}
				}
				
				if(isNode == 1)
				{
					// Create Node
					Context CreateNode = new CreateNode(self,id,word,state);
					CreateNode.Execute();
				}
				else if(isNode == 0)
				{
					// Create Edge
					Context CreateEdge = new CreateEdge(self,self.GetNode(headId),self.GetNode(tailId));
					CreateEdge.Execute();
				}
				else if (isNode == -1)
				{
					self.NodePositions[id] = position;
				}
			}

			foreach(GameNode node in self.GetNodes())
			{
				if(!self.NodePositions.ContainsKey(node.Id))
				{
					self.NodePositions[node.Id] = self.GetNextNodePosition();
				}
			}
			return self;
		}

		public static void RemoveNullEdges(this GameGraph self)
		{
			foreach(GameEdge edge in self.GetEdges().Reverse<GameEdge>())
			{
				if(edge.Head == null || edge.Tail == null)
				{
					self.RemoveEdge(edge,"delete");
				}
			}
		}
		public static void RemoveNode(this GameGraph self,GameNode node, string message)
		{
			node.UpdateObservers(message);
			self.RemoveNode(node);
		}
		public static void RemoveEdge(this GameGraph self, GameEdge edge, string message)
		{
			edge.UpdateObservers(message);
			self.RemoveEdge(edge);
		}
		public static void ClearGraph(this GameGraph self)
		{
			foreach(GameNode node in self.GetNodes().Reverse<GameNode>())
			{
				self.RemoveNode(node,"delete");
			}
			self.RemoveNullEdges();
		}
		public static string Report(this GameGraph self)
		{
			string report = "Reporting : "+self.Name;
			if(self.GetNodes() != null)
			{
				report += "\nNodes : "+self.GetNodes().Count;
				foreach(GameNode node in self.GetNodes())
				{
					string temp = node.Report();
					temp = temp.Replace("\n","\n    ");
					report += "\n  "+temp;
				}
			}
			else
				report += "\nNodes : null";

			if(self.GetEdges() != null)
			{
				report += "\nEdges : "+self.GetEdges().Count;
				if(self.GetEdges().Count > 0 )
					report += "  ";

				foreach(GameEdge edge in self.GetEdges())
				{
					string temp = edge.Report();
					temp = temp.Replace("\n","\n    ");
					report += "\n  "+temp;
				}
			}
			else
				report += "\nEdges : null";

			report += "\nClosing report on :"+self.Name;

			return report;
		}
		public static Vector4 UpdateBounds(this GameGraph self, Vector4 bounds, GameNode node)
		{

			if(node.Position.x < bounds.x)
			{
				bounds.x = node.Position.x;
			}
			if(node.Position.x > bounds.y)
			{
				bounds.y = node.Position.x;
			}
			if(node.Position.y < bounds.z)
			{
				bounds.z = node.Position.y;
			}
			if(node.Position.y > bounds.w)
			{
				bounds.w = node.Position.y;
			}

			return bounds;
		}
		public static Vector4 GetBounds(this GameGraph self)
		{
			Vector4 bounds = Vector4.zero;
			foreach(GameNode node in self.GetNodes())
			{
				bounds = self.UpdateBounds(bounds,node);
			}
			return bounds;
		}


		public static Vector3 GetNodePosition(this GameGraph self,string id)
		{
			if(self.NodePositions.ContainsKey(id))
				return self.NodePositions[id];
			else
				return self.GetNextNodePosition();
		}
		public static Vector3 GetNextNodePosition(this GameGraph self)
		{
			Vector3 newvec = Vector3.zero;
			newvec.x = Random.Range(0,100);
			newvec.y = Random.Range(0,100);
			newvec.z = 2;
			return newvec;
		}
		// Update all game timers
		public static void UpdateTimers(this VariableStorage self)
		{
			// Input lock timer
			float dtime = self.TimerVar ("dtime");
			if(self.TimerVar("lockTimer") <= 0f)
			{
				self.BoolVar("lockInputs",false);
				self.TimerVar("lockTimer",0f);
			}
			else
			{
				self.TimerVar("lockTimer", self.TimerVar("lockTimer")- dtime);
			}
			
			// Idle timer
			if(self.BoolVar("idled"))
			{
				self.TimerVar("idleTimer", self.TimerVar("idleTimer")+ dtime);
			}
			else
			{
				self.TimerVar("idleTimer", 0f);
			}
			
			// dclick
			if(self.TimerVar("doubleClick") > 0f)
			{
				self.TimerVar("doubleClick",self.TimerVar("doubleClick")- dtime);
			}
			
			if(self.TimerVar("renameIdleTimer") > 0f)
			{
				self.TimerVar("renameIdleTimer",self.TimerVar("renameIdleTimer")-dtime);
			}
			
			if(self.TimerVar("inputTimer")  > 0f)
			{
				self.TimerVar("inputTimer",self.TimerVar("inputTimer")-dtime);
			}
			
		}
	}
}
