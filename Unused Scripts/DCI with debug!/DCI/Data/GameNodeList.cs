using UnityEngine;
using System.Collections.Generic;

public class GameNodeList : Dictionary<string, RGameNode>
{

	Dictionary<string,RGameNode> _nodes;

	public GameNodeList()
	{
		_nodes = new Dictionary<string, RGameNode>();
	}

	public void Add(RGameNode node)
	{
		this.Add(node.Id,node);
	}
	
	public void Remove(RGameNode node)
	{
		this.Remove(node.Id);
	}

	#region IDictionary implementation

	public new bool ContainsKey (string key)
	{
		return _nodes.ContainsKey(key);
	}

	public new void Add (string key, RGameNode value)
	{
		_nodes.Add(key,value);
	}

	public new bool Remove (string key)
	{
		return _nodes.Remove(key);
	}

	public new bool TryGetValue (string key, out RGameNode value)
	{
		throw new System.NotImplementedException ();
	}

	public new RGameNode this [string key] {
		get {
			return _nodes[key];
		}
		set {
			_nodes[key] = value;
		}
	}

	public new ICollection<string> Keys {
		get {
			return _nodes.Keys;
		}
	}

	public new ICollection<RGameNode> Values {
		get {
			return _nodes.Values;
		}
	}

	#endregion

	#region ICollection implementation

	public void Add (KeyValuePair<string, RGameNode> item)
	{
		_nodes.Add(item.Key,item.Value);
	}

	public new void Clear ()
	{
		_nodes.Clear();
	}

	public bool Contains (KeyValuePair<string, RGameNode> item)
	{
		return _nodes.ContainsKey(item.Key);
	}

	public void CopyTo (KeyValuePair<string, RGameNode>[] array, int arrayIndex)
	{
		throw new System.NotImplementedException ();
	}

	public bool Remove (KeyValuePair<string, RGameNode> item)
	{
		return _nodes.Remove(item.Key);
	}

	public new int Count {
		get {
			return _nodes.Count;
		}
	}

	public bool IsReadOnly {
		get {
			return false;
		}
	}

	#endregion



}

