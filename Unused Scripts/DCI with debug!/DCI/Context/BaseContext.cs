using UnityEngine;
using System.Collections;

public abstract class BaseContext 
{
	protected void Execute()
	{
		Manager.Message("Debug","Executing "+this.GetType().ToString()+"...");
	}

	protected void Done()
	{
		Manager.Message("Debug","Execution of "+this.GetType().ToString()+" done.");
	}
}
