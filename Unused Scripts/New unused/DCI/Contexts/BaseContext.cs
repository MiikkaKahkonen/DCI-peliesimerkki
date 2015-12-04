using UnityEngine;
using System.Collections;

public abstract class BaseContext : Context
{
	public virtual void Execute ()
	{
		Manager.Message("Debug","Executing "+this.GetType().ToString()+"...");
	}
}
