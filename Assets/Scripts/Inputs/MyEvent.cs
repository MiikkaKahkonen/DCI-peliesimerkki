using UnityEngine;
using System.Collections;

public class MyEvent : IMyEvent {

	public string myName = "";
	public int id = -1;
	public string data = "";
	public object sender = null;

	public MyEvent(string name, int id, string data,object sender)
	{
		this.myName = name;
		this.id = id;
		this.data = data;
		this.sender = sender;
	}
	public MyEvent(string name, string data,object sender)
	{
		this.myName = name;
		this.data = data;
		this.sender = sender;
	}
	public MyEvent(string name)
	{
		this.myName = name;
	}
	public MyEvent(object sender)
	{
		this.sender = sender;
	}
	public MyEvent(string name, string data)
	{
		this.myName = name;
		this.data = data;
	}

	#region IMyEvent implementation

	public string MyName {
		get {
			return myName;
		}
	}

	public int Id {
		get {
			return id;
		}
	}

	public string Data {
		get {
			return data;
		}
	}

	public object Sender {
		get {
			return sender;
		}
	}

	#endregion
}
