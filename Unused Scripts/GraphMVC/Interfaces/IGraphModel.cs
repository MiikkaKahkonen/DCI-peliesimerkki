using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGraphModel
{
	int Id {get;}
	List<IGraphController> Listeners {get;}

	void HasChanged(IMyEvent e);
	void AddListener(IGraphController controller);
	void RemoveListener(IGraphController controller);
}
