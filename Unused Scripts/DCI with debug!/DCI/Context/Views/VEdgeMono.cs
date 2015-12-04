using UnityEngine;
using System.Collections;

public class VEdgeMono : MonoBehaviour 
{
	LineRenderer _lr;
	bool _lrset = false;
	Vector3 startPos;
	Vector3 endPos;

	void Start()
	{
		_lr = gameObject.GetComponent<LineRenderer>();
		_lrset = true;
	}

	private LineRenderer Lr {
		get {
			if(!_lrset)
			{
				_lr = gameObject.GetComponent<LineRenderer>();
				_lrset = true;
			}
			return _lr;
		}
	}

	public void EnableRenderer(bool enable)
	{
		Lr.enabled = enable;
	}
	public Vector3 GetStart()
	{
		return startPos;
	}
	public void SetStart(Vector3 value)
	{
		startPos = value;
		Lr.SetPosition(0,value);
	}
	public Vector3 GetEnd()
	{
		return endPos;
	}
	public void SetEnd(Vector3 value)
	{
		endPos = value;
		Lr.SetPosition(1,value);
	}
}
