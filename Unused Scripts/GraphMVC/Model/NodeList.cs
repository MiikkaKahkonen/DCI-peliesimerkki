using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeList : IList<NodeController>
{
	List<NodeController> nodes;


	public NodeList()
	{
		this.nodes = new List<NodeController>();
	}
	public NodeList(List<NodeController> nodes)
	{
		this.nodes = nodes;
	}

	public void Quess(string word)
	{
		foreach(NodeController node in nodes)
		{
			node.QuessAnswer(word);
		}
	}

	public NodeController GetNode(int id)
	{
		foreach(NodeController controller in nodes)
		{
			if(controller.Model.Id == id)
			{
				return controller;
			}
		}
		return null;
	}

	public List<string> ToListString()
	{
		List<string> list = new List<string>();
		foreach(NodeController controller in nodes)
		{
			list.Add(controller.ToStringLine());
		}
		return list;
	}

	#region IList implementation
	public int IndexOf (NodeController item)
	{
		throw new System.NotImplementedException ();
	}
	public void Insert (int index, NodeController item)
	{
		throw new System.NotImplementedException ();
	}
	public void RemoveAt (int index)
	{
		throw new System.NotImplementedException ();
	}
	public NodeController this [int index] {
		get {
			return nodes[index];
		}
		set {
			nodes[index] = value;
		}
	}
	#endregion
	#region ICollection implementation
	public void Add (NodeController item)
	{
		nodes.Add(item);
	}
	public void Clear ()
	{
		nodes.Clear();
	}
	public bool Contains (NodeController item)
	{
		throw new System.NotImplementedException ();
	}
	public void CopyTo (NodeController[] array, int arrayIndex)
	{
		throw new System.NotImplementedException ();
	}
	public bool Remove (NodeController item)
	{
		return nodes.Remove(item);
	}
	public int Count {
		get {
			return nodes.Count;
		}
	}
	public bool IsReadOnly {
		get {
			throw new System.NotImplementedException ();
		}
	}
	#endregion
	#region IEnumerable implementation
	public IEnumerator<NodeController> GetEnumerator ()
	{
		return nodes.GetEnumerator();
	}
	#endregion
	#region IEnumerable implementation
	IEnumerator IEnumerable.GetEnumerator ()
	{
		return nodes.GetEnumerator();
	}
	#endregion


}
