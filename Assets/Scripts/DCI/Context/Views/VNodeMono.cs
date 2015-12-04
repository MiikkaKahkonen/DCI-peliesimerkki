using UnityEngine;
using System.Collections;

public class VNodeMono : MonoBehaviour
{
	TextMesh _tm;
	bool _tmset = false;
		
	void Start()
	{
		_tm = gameObject.GetComponentInChildren<TextMesh>();
		_tmset = true;
	}

	private TextMesh Tm
	{
		get
		{
			if(!_tmset)
			{
				_tm = gameObject.GetComponentInChildren<TextMesh>();
				_tmset = true;
			}
			return _tm;
		}
	}

	public Vector3 GetPosition()
	{
		return gameObject.transform.position;
	}
	public void SetPosition(Vector3 value)
	{
		gameObject.transform.position = value;
	}

	public void SetWord(string word)
	{
		Tm.text = word;
	}

	public void EnableRenderer(bool enable)
	{
		gameObject.renderer.enabled = enable;
		Tm.renderer.enabled = enable;
	}
}
