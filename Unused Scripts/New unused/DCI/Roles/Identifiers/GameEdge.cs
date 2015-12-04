using UnityEngine;
using System.Collections;

namespace DCI.Roles
{
	public interface GameEdge : Observer, Observable
	{
		DCI.Roles.GameNode Head{get;set;}
		DCI.Roles.GameNode Tail{get;set;}

		string Id{get;set;}
		Manager.DCIEdgeState State{get;set;}


	}
}