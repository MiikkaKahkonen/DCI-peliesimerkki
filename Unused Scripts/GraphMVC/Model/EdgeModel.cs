using UnityEngine;
using System.Collections;

public class EdgeModel : IGraphModel
{
	NodeModel head;
	NodeModel tail;
	int id;
	bool hidden = true;
	System.Collections.Generic.List<IGraphController> listeners;

	public EdgeModel(NodeModel head,NodeModel tail)
	{
		id = NodeController.GetNewID();
		this.SetHead(head);
		this.SetTail(tail);
		listeners = new System.Collections.Generic.List<IGraphController>();
		id = NodeController.GetNewID();
	}
	public Vector3 GetHeadPosition()
	{
		return head.GetPosition();
	}

	public Vector3 GetTailPosition()
	{
		return tail.GetPosition();
	}

	public void SetHead(NodeModel node)
	{
		head = node;
	}
	public void SetTail(NodeModel node)
	{
		tail = node;
	}
	public bool IsHead(int id)
	{
		if(head.Id == id)
			return true;
		return false;
	}
	public bool IsTail(int id)
	{
		if(tail.Id == id)
			return true;
		return false;
	}

	public bool IsAssossiate(int id)
	{
		if(IsHead (id))
			return true;
		else
			return IsTail (id);
	}
	public string ToStringLine()
	{
		return "headid="+head.Id+":tailid="+tail.Id;
	}
	public void RemoveNodeListener(IGraphController controller)
	{
		head.RemoveListener(controller);
		tail.RemoveListener(controller);
	}
	#region IGraphModel implementation

	public void HasChanged (IMyEvent e)
	{
		throw new System.NotImplementedException ();
	}

	public void AddListener (IGraphController controller)
	{
		throw new System.NotImplementedException ();
	}
	public void RemoveListener (IGraphController controller)
	{
		throw new System.NotImplementedException ();
	}

	public int Id {
		get {
			return id;
		}
	}

	public bool Hidden {
		get {
			if(head.State == NodeModel.NodeState.Revealed || 
			   head.State == NodeModel.NodeState.ShownFromStart ||
			   tail.State == NodeModel.NodeState.Revealed ||
			   tail.State == NodeModel.NodeState.ShownFromStart)
			{

				Bootstrap.PrintMessage(this.ToStringLine());

				if((head.State == NodeModel.NodeState.Revealed || head.State == NodeModel.NodeState.ShownFromStart) && tail.State == NodeModel.NodeState.Hidden)
				{
					tail.State = NodeModel.NodeState.Shown;
					Bootstrap.PrintMessage("Tail state to shown");
				}
				else if(head.State == NodeModel.NodeState.Hidden && (tail.State == NodeModel.NodeState.Revealed || tail.State == NodeModel.NodeState.ShownFromStart))
				{
					head.State = NodeModel.NodeState.Shown;
					Bootstrap.PrintMessage("Head state to shown");
				}

				hidden = false;
			}
			else
			{
				hidden = true;
			}
			return hidden;
		}
	}

	public System.Collections.Generic.List<IGraphController> Listeners {
		get {
			throw new System.NotImplementedException ();
		}
	}

	#endregion
	
	public void Die()
	{
		head = null;
		tail = null;
		listeners.Clear();
	}
	public int HeadID()
	{
		return head.Id;
	}
	public int TailID()
	{
		return tail.Id;
	}
}
