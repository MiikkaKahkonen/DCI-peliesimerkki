using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CQuess : BaseContext
{
	RGameGraph gameGraph;
	VQuess view;
	InputSystem inputs;
	RVariableStorage gameVariables;

	string currentQuess = "...";
	bool flipFlop = true;
	string end = " ";

	public CQuess(RGameGraph gameGraph,InputSystem inputs, RVariableStorage gameVariables)
	{
		this.gameGraph = gameGraph;
		this.inputs = inputs;
		this.gameVariables = gameVariables;
		view = new VQuess();
	}

	public new string Execute()
	{

		string result = "";

		base.Execute();

		Queue<IGameInput> inputQueue = inputs.GetInputs();
		string inputString = inputs.GetInputString();

		if(currentQuess == "...")
		{
			currentQuess = "";
		}

		while(inputQueue.Count > 0)
		{
			IGameInput ip = inputQueue.Dequeue();
			
			if(ip is KeyDown)
			{
				if((ip as KeyDown).key == KeyCode.Return)
				{
					if(currentQuess != "")
					{
						result = gameGraph.Quess(currentQuess);
						Manager.Message("Debug",result);
						if(result != "")
						{
							currentQuess = "";
							view.ColorFeedback(0);
						}
						else
						{
							view.ColorFeedback(1);
						}
					}

					gameVariables.TimerVar("inputTimer",0.08f);
				}
				else if((ip as KeyDown).key == KeyCode.Backspace)
				{
					if(currentQuess != "")
						currentQuess = currentQuess.Substring(0,currentQuess.Length-1);

					gameVariables.TimerVar("inputTimer",0.08f);
				}
				else if((ip as KeyDown).key == KeyCode.Escape)
				{
					gameVariables.TimerVar("inputTimer",0.08f);
					gameVariables.BoolVar("quessing",false);
					currentQuess = "...";
					result = "exit";
					view.ColorFeedback(4);
				}
			}
		}

		if(inputString != "")
		{
			currentQuess += inputString;
			gameVariables.TimerVar("inputTimer",0.03f);
			end = " ";
			gameVariables.TimerVar("flashTimer",0.8f);
		}

		if(gameVariables.TimerVar("flashTimer") <= 0)
		{
			gameVariables.TimerVar("flashTimer",0.5f);
			flipFlop = true;
		}

		if(flipFlop)
		{
			if(end == " ")
			{
				end = "_";
			}
			else
			{
				end = " ";
			}
			flipFlop = false;
		}

		if(result != "exit")
		{
			view.UpdateText(currentQuess+end);
		}
		else
		{
			view.UpdateText(currentQuess);

		}

		base.Done();
		return result;
	}
}