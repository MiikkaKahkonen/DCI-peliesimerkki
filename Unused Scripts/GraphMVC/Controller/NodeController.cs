using UnityEngine;
using System.Collections;

public class NodeController : IGraphController
{
	public static int uid = 0;
	NodeModel model = null;
	NodeView view = null;

	public static int GetNewID()
	{
		uid++;
		return uid-1;
	}
	public static void UpdateID(int id)
	{
		if(uid <= id)
		{
			uid = id;
			uid++;
		}
	}

	public void QuessAnswer(string word)
	{
		if(model.State == NodeModel.NodeState.Shown)
		{
			if(model.checkWord(word))
			{
				model.State = NodeModel.NodeState.Revealed;
			}
		}
	}

	public NodeController CreateNode(Vector3 position, string word,NodeModel.NodeState state, int id)
	{
		model = new NodeModel(position,word,state,id);
		model.AddListener(this);
		return this;
	}

	public string ToStringLine ()
	{
		return "node:"+model.ToStringLine();
	}

	public NodeController Die()
	{
		model.Die();
		model = null;
		return this;
	}
	#region IGraphController implementation

	public void MyUpdate (IMyEvent e)
	{
		if(e == null)
		{
			if(model.State == NodeModel.NodeState.Hidden && Game.mode != "edit")
			{
				view.Hide();
			}
			else
			{
				view.Show(model.GetWord());
			}
		}
		else
		{
			if(e.MyName == "Moved")
			{
				view.UpdatePosition(model.GetPosition());
			}
			else if(e.MyName == "Killed")
			{
				view.Die();
				view = null;
			}
			else if(e.MyName == "Renamed")
			{
				view.UpdateName(model.GetWord());
			}
			else if(e.MyName == "State")
			{
				if(model.State == NodeModel.NodeState.Hidden && Game.mode != "edit")
				{
					view.Hide();
				}
				else
				{
					view.Show(model.GetWord());
				}
				view.UpdateState(model.State);
			}
		}


	}
	public Vector3 GetBounds()
	{
		return model.GetPosition();
	}
	public IGraphModel Model {
		get {
			return (model as IGraphModel);
		}
	}

	public IGraphView View {
		get {
			return (view as IGraphView);
		}
	}
	
	public NodeView CreateView (GameObject ViewObject)
	{
		view = new NodeView(ViewObject.GetComponent<NodeMono>(), model.Id,model.State);
		return view;
	}

	public Vector3 Initialize()
	{
		view.UpdatePosition(model.GetPosition());
		MyUpdate(null);
		return GetBounds();
	}

	public IGraphController Select()
	{
		view.Go.SendMessage("Select");
		return this;
	}

	public IGraphController Unselect ()
	{
		view.Go.SendMessage("Unselect");
		return this;
	}

	public void Move(Vector3 to)
	{
		model.SetPosition(to);
	}
	#endregion

	public void AddWord (string com)
	{
		model.SetWord(model.GetWord()+com);
	}

	public void RemoveLastCharOfWord ()
	{
		if(model.GetWord().Length > 0)
			model.SetWord(model.GetWord().Remove(model.GetWord().Length-1));
	}

	public void CycleState()
	{
		if(model.State == NodeModel.NodeState.Hidden)
			model.State = NodeModel.NodeState.ShownFromStart;
		else
			model.State = NodeModel.NodeState.Hidden;
	}
}
