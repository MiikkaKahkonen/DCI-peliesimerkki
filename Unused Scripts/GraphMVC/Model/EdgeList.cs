using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EdgeList : IList<EdgeController>
{
	List<EdgeController> edges;

	public EdgeList()
	{
		this.edges = new List<EdgeController>();
	}
	public EdgeList(List<EdgeController> edges)
	{
		this.edges = edges;
	}

	public EdgeController ContainsSym (int node1, int node2)
	{
		foreach(EdgeController edge in edges)
		{
			if((edge.Model as EdgeModel).HeadID() == node1 || (edge.Model as EdgeModel).HeadID() == node2)
			{
				if((edge.Model as EdgeModel).TailID() == node1 || (edge.Model as EdgeModel).TailID() == node2)
				{
					return edge;
				}
			}
		}
		return null;
	}
	
	public List<string> ToListString()
	{
		List<string> list = new List<string>();
		foreach(EdgeController controller in edges)
		{
			list.Add(controller.ToStringLine());
		}
		return list;
	}
	public EdgeController GetEdge(int id)
	{
		foreach(EdgeController controller in edges)
		{
			if(controller.Model.Id == id)
			{
				return controller;
			}
		}
		return null;
	}

	public void RemoveAssosiates (int nid)
	{
		List<EdgeController> list = new List<EdgeController>();

		foreach(EdgeController controller in edges)
		{
			if((controller.Model as EdgeModel).IsAssossiate(nid))
			{
				list.Add(controller);
			}
		}

		foreach(EdgeController controller in list)
		{
			Remove(controller);
		}

		list = null;
	}

	#region IList implementation
	public int IndexOf (EdgeController item)
	{
		throw new System.NotImplementedException ();
	}
	public void Insert (int index, EdgeController item)
	{
		throw new System.NotImplementedException ();
	}
	public void RemoveAt (int index)
	{
		throw new System.NotImplementedException ();
	}
	public EdgeController this [int index] {
		get {
			return edges[index];
		}
		set {
			edges[index] = value;
		}
	}
	#endregion
	#region ICollection implementation
	public void Add (EdgeController item)
	{
		edges.Add(item);
	}
	public void Clear ()
	{
		edges.Clear();
	}
	public bool Contains (EdgeController item)
	{
		return edges.Contains(item);
	}
	public void CopyTo (EdgeController[] array, int arrayIndex)
	{
		throw new System.NotImplementedException ();
	}
	public bool Remove (EdgeController item)
	{
		return edges.Remove(item);
	}
	public int Count {
		get {
			return edges.Count;
		}
	}
	public bool IsReadOnly {
		get {
			throw new System.NotImplementedException ();
		}
	}
	#endregion
	#region IEnumerable implementation
	public IEnumerator<EdgeController> GetEnumerator ()
	{
		return edges.GetEnumerator();
	}
	#endregion
	#region IEnumerable implementation
	IEnumerator IEnumerable.GetEnumerator ()
	{
		return edges.GetEnumerator();
	}
	#endregion

}
