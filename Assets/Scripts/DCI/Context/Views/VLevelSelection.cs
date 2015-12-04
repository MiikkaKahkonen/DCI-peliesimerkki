using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VLevelSelection: View 
{
	VLevelSelectionMono ui;
    string[] levels;
    int start = 0;
	public VLevelSelection()
	{
        GameObject o = GameObject.Find("Main Camera");
        ui = o.GetComponentInChildren<VLevelSelectionMono>();
    }
    public string GetSelected()
    {
        int index = ui.GetSelected();
        if(index >= 0)
        {
            return levels[index + start];
        }
        else
        {
            return null;
        }
            
    }
    public void SetLevels(string[] levels)
    {
        this.levels = levels;
        updateSelecton();
    }
    public void SelectLevel(int i)
    {
        ui.SelectItem(i);
    }

    public void Up()
    {
        if (start - 1 >= 0)
        {
            start -= 1;
            updateSelecton();
        }
        
    }

    public void Down()
    {
        if (ui.maxLevels + start + 1 < levels.Length)
        {
            start += 1;
            updateSelecton();
        }
       
    }

    private void updateSelecton()
    {
        int size = ui.maxLevels < levels.Length ? ui.maxLevels : levels.Length;
        string[] arr = new string[size];

        for(int i = 0; i < size; i++)
        {
            arr[i] = levels[i + start];
        }

        ui.RenderLevels(arr);
    }

}
