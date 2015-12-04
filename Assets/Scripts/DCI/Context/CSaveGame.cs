using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSaveGame : BaseContext
{
	RGameGraph gameGraph;
	BaseFileSystem bfs;
	Dictionary<string, Vector3> nodePositions;

	public CSaveGame(RGameGraph gameGraph,BaseFileSystem bfs, Dictionary<string,Vector3> nodePositions)
	{
		this.gameGraph = gameGraph;
		this.bfs = bfs;
		this.nodePositions = nodePositions;
	}

	public new void Execute()
	{
		base.Execute();

		List<string> output = new List<string>();
		foreach(RGameNode node in gameGraph.Nodes.Values)
		{
			output.Add(node.ToTexT());
		}
		foreach(RGameEdge edge in gameGraph.Edges.Values)
		{
			output.Add(edge.ToTexT());
		}
		foreach(string key in nodePositions.Keys)
		{
			string result = "";
			result += "pos"+BaseFileSystem.ATTSEP;
			result += "position"+BaseFileSystem.VALSEP;
			result += nodePositions[key].x.ToString()+BaseFileSystem.AXISSEP;
			result += nodePositions[key].y.ToString()+BaseFileSystem.AXISSEP;
			result += nodePositions[key].z.ToString();
			result += BaseFileSystem.ATTSEP;
			result += "id"+BaseFileSystem.VALSEP;
			result += key;
			output.Add(result);
		}
		bfs.SetRelativePath(Manager.DEFAULT_SAVE_NAME);
		bfs.SaveTextFile(output);

		base.Done();
	}
}