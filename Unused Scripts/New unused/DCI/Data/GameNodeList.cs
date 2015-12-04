using UnityEngine;
using System.Collections.Generic;
using DCI.Roles;

class GameNodeList : Dictionary<string, DCI.Roles.GameNode>
{

	Dictionary<string,DCI.Roles.GameNode> _nodes;

	public GameNodeList()
	{
		_nodes = new Dictionary<string, DCI.Roles.GameNode>();
	}

	public void Add(DCI.Roles.GameNode node)
	{
		this.Add(node.Id,node);
	}
	
	public void Remove(DCI.Roles.GameNode node)
	{
		this.Remove(node.Id);
	}

	#region IDictionary implementation

	public bool ContainsKey (string key)
	{
		return _nodes.ContainsKey(key);
	}

	public void Add (string key, DCI.Roles.GameNode value)
	{
		_nodes.Add(key,value);
	}

	public bool Remove (string key)
	{
		return _nodes.Remove(key);
	}

	public bool TryGetValue (string key, out DCI.Roles.GameNode value)
	{
		throw new System.NotImplementedException ();
	}

	public DCI.Roles.GameNode this [string key] {
		get {
			return _nodes[key];
		}
		set {
			_nodes[key] = value;
		}
	}

	public ICollection<string> Keys {
		get {
			return _nodes.Keys;
		}
	}

	public ICollection<DCI.Roles.GameNode> Values {
		get {
			return _nodes.Values;
		}
	}

	#endregion

	#region ICollection implementation

	public void Add (KeyValuePair<string, DCI.Roles.GameNode> item)
	{
		_nodes.Add(item.Key,item.Value);
	}

	public void Clear ()
	{
		_nodes.Clear();
	}

	public bool Contains (KeyValuePair<string, DCI.Roles.GameNode> item)
	{
		return _nodes.ContainsKey(item.Key);
	}

	public void CopyTo (KeyValuePair<string, DCI.Roles.GameNode>[] array, int arrayIndex)
	{
		throw new System.NotImplementedException ();
	}

	public bool Remove (KeyValuePair<string, DCI.Roles.GameNode> item)
	{
		return _nodes.Remove(item.Key);
	}

	public int Count {
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

