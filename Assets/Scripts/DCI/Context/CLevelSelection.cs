using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CLevelSelection : BaseContext
{
    InputSystem inputs;
    VLevelSelection view;
    DLevels levels;

    public CLevelSelection(InputSystem inputs,DLevels levels)
	{
        this.levels = levels;
        this.inputs = inputs;
        view = new VLevelSelection();
        this.levels.LoadLevels();
        view.SetLevels(this.levels.GetLevelNames());
    }

	public new string Execute()
	{
		base.Execute();
        string result = null;
        Queue<IGameInput> inputQueue = inputs.GetInputs();
        string inputString = inputs.GetInputString();

        while (inputQueue.Count > 0)
        {
            IGameInput ip = inputQueue.Dequeue();

            if (ip is MouseClick)
            {
                GameObject ob = Manager.CheckRay((ip as MouseClick).pos, new string[] { "Up", "Down", "Load","Quit","Level"});

                if (ob != null)
                {
                    Manager.Message(ob.tag, "levelselect");
                    if (ob.tag == "Up")
                    {
                        view.Up();
                    }
                    else if (ob.tag == "Down")
                    {
                        view.Down();
                    }
                    else if (ob.tag == "Load")
                    {
                        result = view.GetSelected();
                        if(result != null)
                        {
                            result += ".txt";
                        }
                    }
                    else if (ob.tag == "Quit")
                    {
#if UNITY_EDITOR

                        UnityEditor.EditorApplication.isPlaying = false;
#else
					Application.Quit();
#endif
                    }
                    else if (ob.tag == "Level")
                    {
                        try
                        {
                            view.SelectLevel(int.Parse(ob.name.Substring(ob.name.Length-1)));
                        }
                        catch(UnityException e)
                        {
                        
                        }
                        
                    }
                }
            }

        }

        base.Done();
        return result;
	}
}