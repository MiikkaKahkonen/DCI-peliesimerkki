using UnityEngine;
using System.Collections;

public class EdgeMono : MonoBehaviour
{

	public Material selected;
	public Material unselected;

	EdgeView view = null;
	LineRenderer lr = null;
	Vector3 headPosition;
	Vector3 tailPosition;
	bool changed = false;
	public int id = -1;

	// Use this for initialization
	void Start ()
	{
		transform.parent = Bootstrap.edgeParent.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(changed)
		{
			if(lr == null)
			{
				lr = gameObject.GetComponent<LineRenderer>();
			}
			lr.SetPosition(0,headPosition);
			lr.SetPosition(1,tailPosition);
			changed = false;

			UpdateCollider();
		}
	}
	public void UpdateCollider()
	{
		float distance = Vector3.Distance(headPosition, tailPosition);
		float offset = 3;
		Vector3 center = headPosition;
		center = center + tailPosition;
		center = center/2;
		center.z = 0;
		// uPdate game object
		gameObject.GetComponentInChildren<Transform>().position = center;
		gameObject.GetComponentInChildren<Transform>().LookAt(headPosition);
		gameObject.GetComponentInChildren<CapsuleCollider>().height = distance - offset;
		//(gameObject.collider as CapsuleCollider).transform.LookAt(headPosition);
		
		
	}

	public void Select()
	{
		this.renderer.material = selected;
	}
	public void Unselect()
	{
		this.renderer.material = unselected;
	}

	public void Hide()
	{
		enableRenderer(false);
	}
	public void Show()
	{
		enableRenderer(true);
	}
	private void enableRenderer(bool enabled)
	{
		gameObject.renderer.enabled = enabled;
	}
	//public void SetView(EdgeView view)
	//{
	//	this.view = view;
	//}

	public void UpdatePosition(Vector3 head, Vector3 tail)
	{
		changed = true;
		headPosition = head;
		tailPosition = tail;
	}
	public void initialize(EdgeView view)
	{
		this.view = view;
	}
	public void SetID(int id)
	{
		this.id = id;
	}
	public int GetID()
	{
		return id;
	}
}
