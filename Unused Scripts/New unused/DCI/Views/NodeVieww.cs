using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DCI.Roles;

public class NodeVieww : View 
{

	DCI.Roles.GameNode gameNode;
	Vector3 position;
	GameObject viewObject;
	Manager.DCINodeState state;

	public static GameObject nodeObject;
	public static Material selected;
	public static Material unSelected;

	private Transform parent;
	private bool renderersEnabled;

	public NodeVieww(DCI.Roles.GameNode gameNode,Vector3 position, Transform parent)
	{
		this.parent = parent;
		this.position = position;
		CreateSelf(gameNode);
	}

	public NodeVieww(DCI.Roles.GameNode gameNode)
	{
		CreateSelf(gameNode);
		position = Vector3.zero;
	}
	private void CreateSelf(DCI.Roles.GameNode gameNode)
	{
		renderersEnabled = false;
		this.gameNode = gameNode;
//		this.Observe(this.gameNode);

		GameObject initializer;
		if(nodeObject == null)
		{
			initializer = GameObject.Find("ViewAssets/NodeAsset");
			Manager.Message("Debug","Asset Found! : "+initializer.name);
			nodeObject = initializer;

			unSelected = nodeObject.renderer.materials[0];
			selected = Resources.Load<Material>("Selected");
			Manager.Message("Debug","Loaded Resource: "+"Material-"+"Selected.");
		}
		else
			initializer = nodeObject;
		
		viewObject = GameObject.Instantiate(nodeObject,this.position,nodeObject.transform.rotation) as GameObject;
		viewObject.name = "Node: "+this.gameNode.Id;
		viewObject.GetComponent<NodeMonoView>().Id = this.gameNode.Id;

		if(parent != null)
			viewObject.transform.parent = parent;

		state = Manager.DCINodeState.Hidden;


		viewObject.transform.position = position;

		if(gameNode.State == Manager.DCINodeState.Quessable)
		{
			viewObject.GetComponentInChildren<TextMesh>().text = gameNode.EmptyWord;
		}
		else if( (int)gameNode.State < (int)Manager.DCINodeState.Quessable)
		{
			viewObject.GetComponentInChildren<TextMesh>().text = gameNode.Word;
		}
		
		if(state == Manager.DCINodeState.Hidden)
		{
			renderersEnabled = true;
			viewObject.renderer.enabled = true;
			
			foreach(Renderer r in viewObject.GetComponentsInChildren<Renderer>())
			{
				r.enabled = true;
			}
		}
		
		state = gameNode.State;

	}

	#region implemented abstract members of View
	/*
	public override void Strip ()
	{
		this.QuitObserving(gameNode);
		gameNode = null;
		GameObject.Destroy(viewObject);
	}

	public void Update (string message)
	{
		if(message == "delete")
		{
			//this.Strip();
			return;
		}

		if(viewObject.transform.position != gameNode.Position)
			viewObject.transform.position = gameNode.Position;

		if(state != gameNode.State)
		{
			if(gameNode.State == Manager.DCINodeState.Quessable)
			{
				viewObject.GetComponentInChildren<TextMesh>().text = gameNode.EmptyWord;
			}
			else if( (int)gameNode.State < (int)Manager.DCINodeState.Quessable)
			{
				viewObject.GetComponentInChildren<TextMesh>().text = gameNode.Word;
			}

			if(state == Manager.DCINodeState.Hidden)
			{
				renderersEnabled = true;
				viewObject.renderer.enabled = true;

				foreach(Renderer r in viewObject.GetComponentsInChildren<Renderer>())
				{
					r.enabled = true;
				}
			}

			state = gameNode.State;
		}
	}

	public override void Select ()
	{
		viewObject.renderer.material = selected;
	}
	
	public override void Unselect ()
	{
		viewObject.renderer.material = unSelected;
	}*/
	#endregion

	#region implemented abstract members of View

	public override List<Observer> Observers {
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
