using UnityEngine;
using System.Collections;
using DCI;

namespace DCI.Roles
{
	public interface GraphStorage
	{
		GameGraph Data {get;set;}
		string Name {get;set;}
	}
}
