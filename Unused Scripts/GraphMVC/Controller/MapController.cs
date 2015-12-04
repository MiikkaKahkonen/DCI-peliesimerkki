using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapController
{

	MapModel model;
	MapView view;

	IGraphController selectedObject;

	public MapController()
	{
		model = new MapModel();
	}

	public MapController( GameObject node, GameObject edge)
	{
		model = new MapModel();
		view = new MapView(node,edge);
	}

	public MapController(List<string> mapdata)
	{
		model = new MapModel(mapdata);
	}

	public MapController(List<string> mapdata, GameObject node, GameObject edge)
	{
		model = new MapModel(mapdata);
		view = new MapView(node,edge);
		view.CreateNodeViews(model.GetNodes());
		view.CreateEdgeViews(model.GetEdges());
		model.Initialize();
	}

	public void SetModel(MapModel model)
	{
		KillModel();
		this.model = model;
	}

	public void SetModel(List<string> mapdata)
	{
		KillModel();
		this.model = new MapModel(mapdata);
	}

	public void SetView( GameObject node, GameObject edge)
	{
		KillView();
		view = new MapView(node,edge);
	}

	public void StartView()
	{
		if(model != null && view != null)
		{
			view.CreateNodeViews(model.GetNodes());
			view.CreateEdgeViews(model.GetEdges());
		}
	}

	public void ClearData()
	{
		KillModel();
		selectedObject = null;
		model = new MapModel();
	}
	public bool Initialize()
	{
		if(view != null && model != null)
		{
			model.Initialize();
			return true;
		}
		return false;
	}
	private void KillView()
	{
		if(view != null)
		{
			view.Die();
			view = null;
		}
	}
	private void KillModel()
	{
		if(model != null)
		{
			model.Die();
			model = null;
		}

	}

	public void Quess (string word)
	{
		model.Quess(word);
	}

	public List<string> GetAsStringList ()
	{
		return model.ToListString();
	}

	public Vector4 GetBounds()
	{
		return model.GetBounds();
	}
	public void Unselect()
	{
		if(selectedObject != null)
			selectedObject.Unselect();
		this.ClearSelected();
	}
	public void Select(object ob)
	{
		Unselect();

		if(ob is NodeMono)
		{
			selectedObject = model.GetNodes().GetNode((ob as NodeMono).GetID()).Select();
			//selectedObject = model.GetNodes()[0].Select();
		}
		else if (ob is EdgeMono)
		{
			selectedObject = model.GetEdges().GetEdge((ob as EdgeMono).GetID()).Select();
			//selectedObject = model.GetEdges()[0].Select();
		}
	}
	public void ClearSelected()
	{
		selectedObject = null;
	}

	public object GetSelected ()
	{
		if(selectedObject != null)
			return selectedObject.View.Go;
		return null;
	}

	public void MoveSelected(Vector3 pos)
	{		
		if(selectedObject != null)
		{
			selectedObject.Move(pos);
			GetNewBounds();
		}
	}

	public void Kill(object ob)
	{
		if(ob is NodeMono)
		{
			int nid = (ob as NodeMono).GetID();
			model.GetEdges().RemoveAssosiates(nid);
			NodeController ncon = model.GetNodes().GetNode(nid).Die();
			model.GetNodes().Remove(ncon);
			ncon = null;
		}
		else if (ob is EdgeMono)
		{
			EdgeController vcon = model.GetEdges().GetEdge((ob as EdgeMono).GetID()).Die();
			model.GetEdges().Remove(vcon);
			vcon = null;
			//selectedObject = model.GetEdges()[0].Select();
		}

		GetNewBounds();
	}

	public bool IsConnected(NodeMono n1, NodeMono n2)
	{
		if(n1 == n2)
			return true;

		EdgeController e = model.IsConnected(n1.GetID(),n2.GetID());

		if(e != null)
			return true;
		else 
			return false;
	}

	public void CreateEdge (NodeMono head, NodeMono tail)
	{
		EdgeController e = model.CreateEdge(model.GetNodes().GetNode(head.GetID()).Model as NodeModel,model.GetNodes().GetNode(tail.GetID()).Model as NodeModel);
		view.CreateEdgeView(e);
		e.Initialize();
	}

	public void CreateNode (Vector3 position)
	{
		NodeController n = model.CreateNode(position,"New Node",NodeModel.NodeState.Revealed,NodeController.GetNewID());
		view.CreateNodeView(n);
		model.AddBounds(n.Initialize());
		model.GetNewBounds();
	}

	private void GetNewBounds()
	{
		model.GetNewBounds();
	}

	public void AddWords (string com)
	{
		(selectedObject as NodeController).AddWord(com);
	}

	public void RemoveLastCharOfWord ()
	{
		(selectedObject as NodeController).RemoveLastCharOfWord();
	}
	public void CycleState()
	{
		(selectedObject as NodeController).CycleState();
	}
}
