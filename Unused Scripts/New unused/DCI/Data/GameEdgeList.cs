using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DCI.Roles;

class GameEdgeList : Dictionary<string,DCI.Roles.GameEdge>
{
	Dictionary<string,DCI.Roles.GameEdge> _edges;
	
	public GameEdgeList()
	{
		_edges = new Dictionary<string, DCI.Roles.GameEdge>();
	}
	
	public void Add(DCI.Roles.GameEdge edge)
	{
		this.Add(edge.Id,edge);
	}
	
	public void Remove(DCI.Roles.GameEdge edge)
	{
		this.Remove(edge.Id);
	}
	
	#region IDictionary implementation
	
	public bool ContainsKey (string key)
	{
		return _edges.ContainsKey(key);
	}
	
	public void Add (string key, DCI.Roles.GameEdge value)
	{
		_edges.Add(key,value);
	}
	
	public bool Remove (string key)
	{
		return _edges.Remove(key);
	}
	
	public bool TryGetValue (string key, out DCI.Roles.GameEdge value)
	{
		throw new System.NotImplementedException ();
	}
	
	public DCI.Roles.GameEdge this [string key] {
		get {
			return _edges[key];
		}
		set {
			_edges[key] = value;
		}
	}
	
	public ICollection<string> Keys {
		get {
			return _edges.Keys;
		}
	}
	
	public ICollection<DCI.Roles.GameEdge> Values {
		get {
			return _edges.Values;
		}
	}
	
	#endregion
	
	#region ICollection implementation
	
	public void Add (KeyValuePair<string, DCI.Roles.GameEdge> item)
	{
		_edges.Add(item.Key,item.Value);
	}
	
	public void Clear ()
	{
		_edges.Clear();
	}
	
	public bool Contains (KeyValuePair<string, DCI.Roles.GameEdge> item)
	{
		return _edges.ContainsKey(item.Key);
	}
	
	public void CopyTo (KeyValuePair<string, DCI.Roles.GameEdge>[] array, int arrayIndex)
	{
		throw new System.NotImplementedException ();
	}
	
	public bool Remove (KeyValuePair<string, DCI.Roles.GameEdge> item)
	{
		return _edges.Remove(item.Key);
	}
	
	public int Count {
		get {
			return _edges.Count;
		}
	}
	
	public bool IsReadOnly {
		get {
			return false;
		}
	}
	
	#endregion
	
	

}

