using UnityEngine;
using System.Collections;

public class NodeMono : MonoBehaviour {

	public Material selected;
	public Material unselected;
	public Material unselectedEdit;

	private Material materialToUse;

	NodeView view = null;
	bool changed = false;
	public int id = -1;
	Vector3 newPosition;


	void Start ()
	{
		transform.parent = Bootstrap.nodeParent.transform;
	}
	// Update is called once per frame
	void Update () {

		if(changed)
		{
			transform.position = newPosition;
			changed = false;
		}

	}


	//public void SetView(NodeView view)
	//{
	//	this.view = view;
	//}

	public void Hide()
	{
		enableRenderers(false);
	}
	public void Show()
	{
		enableRenderers(true);
	}

	public void Select()
	{
		this.renderer.material = selected;
	}
	public void Unselect()
	{
		this.renderer.material = materialToUse;
	}

	private void enableRenderers(bool enable)
	{
		foreach(Renderer r in gameObject.GetComponentsInChildren<Renderer>())
		{
			r.enabled = enable;
		}
	}

	public void UpdatePosition(Vector3 position)
	{
		changed = true;
		newPosition = position;
	}
	public void initialize(NodeView view)
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
	public void ShownFromStart(bool mode)
	{
		if(mode)
			materialToUse = unselectedEdit;
		else
			materialToUse = unselected;

		this.renderer.material = materialToUse;
	}
}
