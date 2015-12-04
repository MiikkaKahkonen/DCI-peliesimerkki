using UnityEngine;
using System.Collections;
using DCI.Roles;

public abstract class View : DCI.Roles.Observable
{
	#region Observable implementation
	public abstract System.Collections.Generic.List<Observer> Observers {get;set;}
	public abstract bool Changed {get;set;}
	#endregion
	/*
	abstract public void Strip();
	abstract public void Select();
	abstract public void Unselect();
	*/
}
