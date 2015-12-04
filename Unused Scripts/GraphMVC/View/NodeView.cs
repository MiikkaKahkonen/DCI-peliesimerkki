using UnityEngine;
using System.Collections;

public class NodeView : IGraphView
{
	NodeMono mono;

	public NodeView(NodeMono mono,int id,NodeModel.NodeState state)
	{
		this.mono = mono;
		this.mono.SetID(id);

		mono.ShownFromStart(state == NodeModel.NodeState.ShownFromStart);
	}

	public void UpdatePosition(Vector3 position)
	{
		mono.UpdatePosition(position);
	}
	public void UpdateState(NodeModel.NodeState state)
	{
		mono.ShownFromStart(state == NodeModel.NodeState.ShownFromStart);
	}

	#region IGraphView implementation

	public void Hide ()
	{
		mono.Hide();
	}

	public void Show (string word)
	{
		mono.gameObject.GetComponentInChildren<TextMesh>().text = word;
		mono.Show();
	}

	public GameObject Go {
		get {
			return mono.gameObject;
		}
	}

	#endregion

	public void Die()
	{
		GameObject.Destroy(mono.gameObject);
		mono = null;
	}

	public void UpdateName (string word)
	{
		mono.gameObject.GetComponentInChildren<TextMesh>().text = word;
	}
}
