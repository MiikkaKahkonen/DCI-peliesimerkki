using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DCI.Roles
{
	public interface VariableStorage
	{
		Dictionary<string,bool> BoolVariables {get;set;}
		Dictionary<string,float> FloatVariables {get;set;}
		Dictionary<string,int> IntVariables {get;set;}
		Dictionary<string,Vector3> Vector3Variables {get;set;}
		Dictionary<string,float> Timers {get;set;}

	}
}