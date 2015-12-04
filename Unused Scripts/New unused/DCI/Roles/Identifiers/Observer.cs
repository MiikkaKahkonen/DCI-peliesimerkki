using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace DCI.Roles
{
	public interface Observer
	{
		void Update(string message);
	}
}