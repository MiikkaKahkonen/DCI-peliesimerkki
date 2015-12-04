using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CLoadGame : BaseContext
{
	RGameGraph gameGraph;
	Dictionary<string, Vector3> nodePositions;
	BaseFileSystem bfs;

	public CLoadGame (RGameGraph gameGraph, Dictionary<string, Vector3> nodePositions,BaseFileSystem bfs)
	{
		this.gameGraph = gameGraph;
		this.nodePositions = nodePositions;
		this.bfs = bfs;
	}

	public new void Execute()
	{
		base.Execute();

		//string filename = Manager.DEFAULT_LOAD_NAME;
		//bfs.SetRelativePath(filename);

		if(!bfs.fileExists())
		{
			Manager.Message("Setup","Loading failed, file does not exist: "+bfs.GetPath());
		}

		List<string> lines = bfs.LoadTextFile();

		if(gameGraph.CreateGraph(lines,nodePositions))
		{
			Manager.Message("Setup","Game loaded successfully from file: "+ bfs.GetPath());
		}
		else
		{
			Manager.Message("Setup","Loading failed, game was NOT loaded.");
		}

		base.Done();
	}
}