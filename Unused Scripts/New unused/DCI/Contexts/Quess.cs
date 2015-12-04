using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DCI.Roles;

public class Quess : BaseContext 
{
	GameGraph gameGraph;
	string quess;
	bool result;

	public Quess(GameGraph gameGraph, string quess)
	{
		this.gameGraph = gameGraph;
		this.quess = quess.ToLower();
	}

	public override void Execute ()
	{
		base.Execute ();

		DCI.Roles.GameNode node = gameGraph.CheckAnswer(quess);

		if(node != null)
		{
			ICollection<DCI.Roles.GameNode> nodes = gameGraph.Reveal(node);

			foreach(DCI.Roles.GameNode nodeRelative in nodes)
			{
				nodeRelative.MakeQuessable();
			}

			quess = char.ToUpper(quess[0]) + quess.Substring(1);
			Manager.Message("Game",quess+" was correct!");
			result = true;
		}
		else
		{
			quess = char.ToUpper(quess[0]) + quess.Substring(1);
			Manager.Message("Game",quess+" was wrong.");
			result = false;
		}

	}
	public bool Result()
	{
		return result;
	}
}
