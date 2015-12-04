using UnityEngine;
using System.Collections;

public class NodeModel : IGraphModel
{
	public enum NodeState {Hidden = 0,Shown = 1,Revealed = 2, ShownFromStart = 3}

	Vector3 position;
	string word;
	string emptyWord;
	int id;


	bool dead = false;

	NodeState state = NodeState.Hidden;
	System.Collections.Generic.List<IGraphController> listeners;

	public NodeModel (Vector3 position, string word, NodeState state,int id)
	{
		this.position = position;
		this.word = word;
		this.id = id;
		NodeController.UpdateID(id);
		listeners = new System.Collections.Generic.List<IGraphController>();
		this.state = state;
		
		System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^ ]");
		emptyWord = rgx.Replace(word, "X");
		emptyWord += "("+(word.Replace(" ","")).Length+")";
	}
	public NodeModel (Vector3 position, string word, NodeState state)
	{
		this.position = position;
		this.word = word;
		id = NodeController.GetNewID();
		listeners = new System.Collections.Generic.List<IGraphController>();
		this.state = state;

		System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^ ]");
		emptyWord = rgx.Replace(word, "X");
		emptyWord += "("+(word.Replace(" ","")).Length+")";
	}
	public string GetWord()
	{
		if(state == NodeState.Revealed || state == NodeState.ShownFromStart || Game.mode == "edit")
			return word;
		else
		{
			return emptyWord;
		}
	}
	public bool checkWord(string newWord)
	{
		return newWord == word;
	}

	public Vector3 GetPosition()
	{
		return position;
	}

	public string ToStringLine()
	{
		string attsep = ":";
		string valsep = "=";
		return "position="+(int)position.x+","+(int)position.y+","+(int)position.z+attsep+"id"+valsep+id+attsep+"state"+valsep+(int)state+attsep+"word"+valsep+word;
	}

	#region IGraphModel implementation

	public void HasChanged (IMyEvent e)
	{
		foreach(IGraphController listener in listeners)
		{
			listener.MyUpdate(e);
		}
	}

	public void AddListener (IGraphController controller)
	{
		listeners.Add(controller);
	}

	public void RemoveListener (IGraphController controller)
	{
		if(dead) 
			return;
		listeners.Remove(controller);
	}

	public int Id {
		get {
			return id;
		}
	}

	public NodeState State {
		get {
			return state;
		}
		set {
			if(state == value)
				return;
			state = value;
			HasChanged(new MyEvent("State"));
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
		dead = true;
		HasChanged(new MyEvent("Killed"));
		listeners.Clear();
	}
	public void SetPosition(Vector3 pos)
	{
		this.position = pos;
		HasChanged(new MyEvent("Moved"));
	}

	public void SetWord (string str)
	{
		word = str;
		HasChanged(new MyEvent("Renamed"));
	}

}
