using UnityEngine;
using System.Collections.Generic;

namespace DCI.Roles
{
	public interface GameGraph : Observable
	{

		string Name {get;set;}

		void AddNode(GameNode node);
		void RemoveNode(GameNode node);
		void AddEdge(GameEdge edge);
		void RemoveEdge(GameEdge edge);


		GameNode GetNodeWithWord(string word);	

		ICollection<GameNode> GetNodes();
		ICollection<GameEdge> GetEdges();
		ICollection<GameNode> GetConnectedNodes(GameNode node);

		GameNode GetNode(string key);
		GameEdge GetEdge(string key);
		GameNode CreateNewNode();
		GameEdge CreateNewEdge();

		Dictionary<string,Vector3> NodePositions{get;set;}
	}
}
