using UnityEngine;
using System.Collections;
using DCI.Roles;

public class LoadGameFromStorage : BaseContext {

	DCI.Roles.GameGraph gameGraph;
	DCI.Roles.GraphStorage graphStorage;
	string name;

	public LoadGameFromStorage(DCI.Roles.GameGraph gameGraph,DCI.Roles.GraphStorage graphStorage,string name)
	{
		this.gameGraph = gameGraph;
		this.graphStorage = graphStorage;
		this.name = name;
	}

	public override void Execute ()
	{
		base.Execute ();

		DCI.Roles.GameGraph tempGraph = graphStorage.StorageToGraph(name);

		if(gameGraph != null)
		{
			if(gameGraph != null)
			{
				//gameGraph.ClearGraph();
			}
			gameGraph = tempGraph;
			Manager.Message("Setup","Game "+gameGraph.Name+" loaded.");
		}
		else
		{
			Manager.Message("Setup","No game with name "+name+".");
		}
	}
}
