using UnityEngine;
using System.Collections;
using DCI.Roles;

public class LoadGameFromTextFile : BaseContext {

	DCI.Roles.TextFileStorage textFileStorage = textFileStorage;
	DCI.Roles.GameGraph gameGraph = gameGraph;
	DCI.Roles.GraphStorage graphStorage = graphStorage;
	Options options;

	public LoadGameFromTextFile(TextFileStorage textFileStorage,
	                            GameGraph gameGraph,
	                            GraphStorage graphStorage,
	                            Options options)
	{
		this.textFileStorage = textFileStorage;
		this.gameGraph = gameGraph;
		this.graphStorage = graphStorage;
		
		if(options == null)
			options = new Options();
		this.options = options;
	}

	#region Context implementation
	
	public override void Execute ()
	{
		base.Execute();

		if(options.filename == "")
		{
			if(graphStorage.Name != "")
				options.filename = graphStorage.Name;
			else
				options.filename = Manager.DEFAULT_SAVE_NAME;
		}

		options.fullpath = textFileStorage.SetPath(options);

		string data = textFileStorage.StorageAsText();

		if(data == Manager.GAME_LOAD_FAILED)
		{
			Manager.Message("Setup","Loading failed, game was NOT loaded.");
			return;
		}

		gameGraph.Name = options.filename;

		gameGraph.TextToGraph(data);

		graphStorage.GraphToStorage(gameGraph);

		Manager.SetActiveGraph(gameGraph);

	}

	#endregion
}
