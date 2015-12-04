using UnityEngine;
using System.Collections;

public class EdgeView : IGraphView 
{
	EdgeMono mono;

	public EdgeView(EdgeMono mono, int id)
	{
		this.mono = mono;
		this.mono.SetID(id);
	}

	public void UpdatePosition(Vector3 head,Vector3 tail)
	{
		mono.UpdatePosition(head,tail);
	}

	#region IGraphView implementation

	public void Hide ()
	{
		mono.Hide();
	}

	public void Show ()
	{
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
	}
}
