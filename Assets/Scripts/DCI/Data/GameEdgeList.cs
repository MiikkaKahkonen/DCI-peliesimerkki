using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEdgeList : Dictionary<string,RGameEdge>
{
	Dictionary<string,RGameEdge> _edges;
	
	public GameEdgeList()
	{
		_edges = new Dictionary<string, RGameEdge>();
	}
	
	public void Add(RGameEdge edge)
	{
		this.Add(edge.Id,edge);
	}
	
	public void Remove(RGameEdge edge)
	{
		this.Remove(edge.Id);
	}
	
	#region IDictionary implementation
	
	public new bool ContainsKey (string key)
	{
		return _edges.ContainsKey(key);
	}
	
	public new void Add (string key, RGameEdge value)
	{
		_edges.Add(key,value);
	}
	
	public new bool Remove (string key)
	{
		return _edges.Remove(key);
	}
	
	public new bool TryGetValue (string key, out RGameEdge value)
	{
		throw new System.NotImplementedException ();
	}
	
	public new RGameEdge this [string key] {
		get {
			return _edges[key];
		}
		set {
			_edges[key] = value;
		}
	}
	
	public new ICollection<string> Keys {
		get {
			return _edges.Keys;
		}
	}
	
	public new ICollection<RGameEdge> Values {
		get {
			return _edges.Values;
		}
	}
	
	#endregion
	
	#region ICollection implementation
	
	public void Add (KeyValuePair<string, RGameEdge> item)
	{
		_edges.Add(item.Key,item.Value);
	}
	
	public new void Clear ()
	{
		_edges.Clear();
	}
	
	public bool Contains (KeyValuePair<string, RGameEdge> item)
	{
		return _edges.ContainsKey(item.Key);
	}
	
	public void CopyTo (KeyValuePair<string, RGameEdge>[] array, int arrayIndex)
	{
		throw new System.NotImplementedException ();
	}
	
	public bool Remove (KeyValuePair<string, RGameEdge> item)
	{
		return _edges.Remove(item.Key);
	}
	
	public new int Count {
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

