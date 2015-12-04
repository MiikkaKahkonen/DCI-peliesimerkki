using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DCI.Roles
{
	public interface GameNode : Observable
	{
		string Id{get;set;}
		Vector3 Position {get;set;}

		Manager.DCINodeState State {get;set;}
		string Word{get;set;}
		string EmptyWord{get;set;}
	}
}