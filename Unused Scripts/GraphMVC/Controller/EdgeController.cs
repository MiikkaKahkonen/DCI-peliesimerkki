using UnityEngine;
using System.Collections;

public class EdgeController : IGraphController
{
	EdgeModel model;
	EdgeView view;

	public void ConnectEdge(NodeModel head, NodeModel tail)
	{
		model = new EdgeModel(head,tail);

		head.AddListener(this);
		tail.AddListener(this);
	}

	public string ToStringLine ()
	{
		return "edge:"+model.ToStringLine();
	}

	public EdgeController Die()
	{
		model.RemoveNodeListener(this);
		view.Die();
		model.Die();
		return this;
	}

	#region IGraphController implementation

	public void MyUpdate (IMyEvent e)
	{
		if(e == null)
		{
			if(model.Hidden)
			{
				view.Hide();
			}
			else
			{
				view.Show();
			}
		}
		else
		{
			if(e.MyName == "Moved")
			{
				view.UpdatePosition(model.GetHeadPosition(),model.GetTailPosition());
			}
			if(e.MyName == "Killed")
			{

				model.RemoveNodeListener(this);
				model.Die();
				view.Die();
				model = null;
				view = null;
			}
			/*if(e.MyName == "State")
			{
				if(model.Hidden)
				{
					view.Hide();
				}
				else
				{
					view.Show();
				}
			}*/
		}
	}

	public IGraphModel Model {
		get {
			return model as IGraphModel;
		}
	}

	public IGraphView View {
		get {
			return view as IGraphView;
		}
	}
	
	public EdgeView CreateView (GameObject ViewObject)
	{
		view = new EdgeView(ViewObject.GetComponent<EdgeMono>(),model.Id);
		return view;
	}

	public Vector3 Initialize()
	{
		view.UpdatePosition(model.GetHeadPosition(),model.GetTailPosition());
		if(model.Hidden)
		{
			view.Hide();
		}
		else
		{
			view.Show();
		}

		return (model.GetHeadPosition() + model.GetTailPosition()/2);
	}

	public IGraphController Select ()
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
		return;
	}
	#endregion
}
