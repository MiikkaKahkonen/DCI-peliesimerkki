using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CQuit : BaseContext
{
	InputSystem inputs;
	RGameGraph gameGraph;
	Dictionary<string,Vector3>  nodePositions;

	public CQuit(InputSystem inputs)
	{
		this.inputs = inputs;

		Renderer[] rends = GameObject.Find("Saving").GetComponentsInChildren<Renderer>();
		foreach(Renderer rend in rends)
		{
			rend.enabled = true;
		}

		Collider[] cols = GameObject.Find("Saving").GetComponentsInChildren<Collider>();
		foreach(Collider col in cols)
		{
			if(col.gameObject.name == "Yes" || col.gameObject.name == "No" || col.gameObject.name == "Cancel")
			{
				col.enabled = true;
			}
		}
	}

	public new int Execute()
	{
		base.Execute();
		int result = 0;
		Queue<IGameInput> inputQueue = inputs.GetInputs();
		string inputString = inputs.GetInputString();

		while(inputQueue.Count > 0)
		{
			IGameInput ip = inputQueue.Dequeue();

			if(ip is MouseClick)
			{
				GameObject ob = Manager.CheckRay((ip as MouseClick).pos,new string[] {"Yes","No","Cancel"});

				if(ob != null)
				{
					if(ob.tag == "Yes")
					{
						result = 1;
					}
					else if(ob.tag == "No")
					{
						result = 2;
					}
					else
					{
						Renderer[] rends = GameObject.Find("Saving").GetComponentsInChildren<Renderer>();
						foreach(Renderer rend in rends)
						{
							rend.enabled = false;
						}
						Collider[] cols = GameObject.Find("Saving").GetComponentsInChildren<Collider>();
						foreach(Collider col in cols)
						{
							if(col.gameObject.name == "Yes" || col.gameObject.name == "No" || col.gameObject.name == "Cancel")
							{
								col.enabled = false;
							}
						}
						result = 3;
					}
				}
			}

		}


		base.Done();
		return result;
	}
}