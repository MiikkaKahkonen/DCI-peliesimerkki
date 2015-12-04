using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DCI.Roles
{
	public interface Observable
	{
		List<DCI.Roles.Observer> Observers{get;set;}
		bool Changed {get;set;}
	}
}
