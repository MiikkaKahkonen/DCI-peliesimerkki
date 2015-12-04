using UnityEngine;
using System.Collections;
using System;
using DCI.Roles;

public class SaveGameToTextFile : BaseContext
{
	private DCI.Roles.GraphStorage graphStorage;
	private DCI.Roles.TextFileStorage textFileStorage;
	private DCI.Roles.GameGraph gameGraph;
	private Options options;
	
	public SaveGameToTextFile(DCI.Roles.TextFileStorage textFileStorage,
	                          DCI.Roles.GraphStorage graphStorage,
	                          DCI.Roles.GameGraph gameGraph,
	                          Options options)
	{
		this.textFileStorage = textFileStorage;
		this.graphStorage = graphStorage;
		this.gameGraph = gameGraph;

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

		string textToStore = gameGraph.GraphToText();

		if(textFileStorage.TextToStorage(textToStore))
		{
			Manager.Message("Setup","Game saved to : "+options.fullpath);
		}
		else
		{
			Manager.Message("Setup","Saving failed, game was NOT saved.");
		}
	}
	
	#endregion
}
