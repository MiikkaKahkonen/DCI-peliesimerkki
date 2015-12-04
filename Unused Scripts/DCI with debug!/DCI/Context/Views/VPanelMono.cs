using UnityEngine;
using System.Collections;

public class VPanelMono : MonoBehaviour
{
	float timer = 0f;

	void Update()
	{
		if(timer > 0f)
		{
			timer -= Time.deltaTime;
			if(timer <= 0f)
			{
				SetColor(new Color(218f/255,218f/255,218f/255,255f/255));
			}
		}
	}

	public void SetColor(Color color)
	{
		gameObject.GetComponent<MeshRenderer>().material.color = color;
		if(color != new Color(218f/255,218f/255,218f/255,255f/255))
			timer = 1.5f;
	}

}

