using UnityEngine;
using System.Collections;

public class CUpdateCamera : BaseContext
{
	RVariableStorage varStorage;
	RGameCamera camera;
	float z;
	Vector3 originalCamPos;

	public CUpdateCamera(RVariableStorage varStorage)
	{
		this.varStorage = varStorage;
		this.camera = Manager.GetCamera();
		z = camera.Cam.transform.position.z;
	}

	public new void Execute()
	{
		base.Execute();

		if(varStorage.BoolVar("moveCamera"))
		{
			camera.MoveTowards(camera.Cam.ScreenToWorldPoint(varStorage.Vector3Var("mousePosition")));	
		}

		base.Done();
	}
}