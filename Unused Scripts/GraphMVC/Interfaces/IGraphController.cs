using UnityEngine;
using System.Collections;

public interface IGraphController 
{
	IGraphModel Model {get;}
	IGraphView View {get;}

	void MyUpdate(IMyEvent e);
	Vector3 Initialize();
	
	IGraphController Select();
	IGraphController Unselect();

	void Move(Vector3 to);
}	
