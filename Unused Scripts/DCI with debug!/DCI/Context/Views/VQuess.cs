using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VQuess: View 
{
	GameObject QuessPanel;
	TextMesh answer;
	VPanelMono mono;

	public VQuess()
	{
		QuessPanel = GameObject.Find("QuessPanel");
		TextMesh[] meshes = QuessPanel.GetComponentsInChildren<TextMesh>();
		mono = QuessPanel.GetComponent<VPanelMono>();

		for(int i = 0; i < meshes.Length; i++)
		{
			if(meshes[i].name == "Answer")
			{	
				answer = meshes[i];
				break;
			}
		}
	}

	public void UpdateText(string text)
	{
		answer.text = text;
	}

	public void ColorFeedback(int state)
	{
		if(state == 0)
		{
			mono.SetColor(Color.green);
		}
		else if (state == 1)
		{
			mono.SetColor(Color.red);
		}
		else if (state == 3)
		{
			mono.SetColor(Color.gray);
		}
		else
		{
			mono.SetColor(new Color(218f/255,218f/255,218f/255,255f/255));
		}
	}
}
