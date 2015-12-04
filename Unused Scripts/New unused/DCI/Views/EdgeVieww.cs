using UnityEngine;
using System.Collections;
using DCI.Roles;

public class EdgeVieww : View
{
	public static GameObject edgeObject;

	private Transform parent;
	private GameObject viewObject;
	public static Material selected;
	public static Material unSelected;

	private Manager.DCIEdgeState state;
	private DCI.Roles.GameEdge gameEdge;
	private Vector3 headPosition;
	private Vector3 tailPosition;

	public EdgeVieww(DCI.Roles.GameEdge gameEdge, Transform parent)
	{
		this.parent = parent;
		this.gameEdge = gameEdge;
		//this.Observe(gameEdge.Head);
		//this.Observe(gameEdge.Tail);

		headPosition = Vector3.zero;
		tailPosition = Vector3.zero;
		GameObject initializer;

		if(edgeObject == null)
		{
			initializer = GameObject.Find("ViewAssets/EdgeAsset");
			Manager.Message("Debug","Asset Found! : "+initializer.name);
			edgeObject = initializer;

			unSelected = edgeObject.renderer.materials[0];
			selected = Resources.Load<Material>("SelectedEdge");
			Manager.Message("Debug","Loaded Resource: "+"Material-"+"Selected.");	
		}
		else
			initializer = edgeObject;

		viewObject = GameObject.Instantiate(edgeObject,edgeObject.transform.position,edgeObject.transform.rotation) as GameObject;
		viewObject.name = "Edge: "+this.gameEdge.Id;
		viewObject.GetComponent<EdgeMonoView>().Head =this.gameEdge.Head.Id;
		viewObject.GetComponent<EdgeMonoView>().Tail =this.gameEdge.Tail.Id;

		if(parent != null)
			viewObject.transform.parent = parent;
		
		state = Manager.DCIEdgeState.Hidden;

		headPosition = gameEdge.Head.Position;
		tailPosition = gameEdge.Tail.Position;
		
		viewObject.GetComponent<LineRenderer>().SetPosition(0,headPosition);
		viewObject.GetComponent<LineRenderer>().SetPosition(1,tailPosition);
		
		UpdateCollider();

		if(gameEdge.State == Manager.DCIEdgeState.Shown)
			viewObject.renderer.enabled = true;
		else
			viewObject.renderer.enabled = false;
		
		state = gameEdge.State;

		//this.Update("Init");
	}

	#region implemented abstract members of View
	/*
	public override void Strip ()
	{
		//this.QuitObserving(gameEdge.Head);
		//this.QuitObserving(gameEdge.Tail);
		gameEdge = null;
		GameObject.Destroy(viewObject);
	}

	public override void Update (string message)
	{
		if(message == "delete")
		{
			this.Strip();
			return;
		}


	}

	public override void Select ()
	{
		viewObject.renderer.material = selected;
	}
	
	public override void Unselect ()
	{
		viewObject.renderer.material = unSelected;
	}
*/
	#endregion

	private void UpdateCollider()
	{
		float distance = Vector3.Distance(headPosition, tailPosition);
		float offset = 3;
		Vector3 center = headPosition;
		center = center + tailPosition;
		center = center/2;
		center.z = 0;
		// Update game object 
		viewObject.GetComponentInChildren<Transform>().position = center;
		viewObject.GetComponentInChildren<Transform>().LookAt(headPosition);
		viewObject.GetComponentInChildren<CapsuleCollider>().height = distance - offset;
	}

	#region implemented abstract members of View

	public override System.Collections.Generic.List<Observer> Observers {
		get {
			throw new System.NotImplementedException ();
		}
		set {
			throw new System.NotImplementedException ();
		}
	}

	public override bool Changed {
		get {
			throw new System.NotImplementedException ();
		}
		set {
			throw new System.NotImplementedException ();
		}
	}

	#endregion
}
