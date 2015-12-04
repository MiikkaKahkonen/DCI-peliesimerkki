using UnityEngine;
using System.Collections;
using DCI.Roles;

public class MoveCamera : BaseContext 
{
	GameCamera gameCam;
	VariableStorage variableStorage;

	public MoveCamera(GameCamera gameCam, VariableStorage vs)
	{
		this.gameCam = gameCam;
		this.variableStorage = vs;
	}

	public override void Execute ()
	{
		//base.Execute ();

		if(variableStorage.BoolVar ("updateCameraToPointer"))
		{
			if(variableStorage.BoolVar ("moveCameraWithDrag"))
			{
				variableStorage.Vector3Var("lastMousePosition", Camera.main.ScreenToViewportPoint(variableStorage.Vector3Var("mousePosition") - variableStorage.Vector3Var("dragPosition")));
				float x = variableStorage.Vector3Var("lastMousePosition").x * 1;
				float y = variableStorage.Vector3Var("lastMousePosition").y * 1;
				
				Vector3 move = new Vector3(x , y , 0);
				
				if(move.x > 0)
					move.x = 0.75f;
				if(move.x < 0)
					move.x = -0.75f;
				if(move.y > 0)
					move.y = 0.75f;
				if(move.y < 0)
					move.y = -0.75f;
				
				gameCam.Translate(move, Space.World);
				//BoundCamera();
			}
			else
			{
				variableStorage.Vector3Var("lastMousePosition", gameCam.ScreenToViewportPoint(variableStorage.Vector3Var("mousePosition") - variableStorage.Vector3Var("dragPosition")));
				
				float x = variableStorage.Vector3Var("lastMousePosition").x * variableStorage.FloatVar("dragSpeed");
				float y = variableStorage.Vector3Var("lastMousePosition").y * variableStorage.FloatVar("dragSpeed");
				
				if(x > variableStorage.FloatVar("maxXSpeed") | x < -variableStorage.FloatVar("maxXSpeed"))
				{
					if( x < -variableStorage.FloatVar("maxXSpeed"))
					{
						x = -variableStorage.FloatVar("maxXSpeed");
					} 
					else
					{
						x = variableStorage.FloatVar("maxXSpeed");
					}
				}
				if(y > variableStorage.FloatVar("maxYSpeed") | y < -variableStorage.FloatVar("maxYSpeed"))
				{
					if( y < -variableStorage.FloatVar("maxYSpeed"))
					{
						y = -variableStorage.FloatVar("maxYSpeed");
					}
					else
					{
						y = variableStorage.FloatVar("maxYSpeed");
					}
				}
				
				Vector3 move = new Vector3(x, y, 0);
				
				gameCam.Translate(-move, Space.World);
			}
		}
		else if(variableStorage.BoolVar ("slowCameraMovement"))
		{
			variableStorage.TimerVar("slowDownTimer",variableStorage.TimerVar("slowDownTimer")-Time.deltaTime);
			
			if(variableStorage.TimerVar("slowDownTimer") > 0)
			{
				variableStorage.FloatVar("slowDragSpeed", variableStorage.FloatVar("slowDragSpeed") - variableStorage.FloatVar("slowDragSpeed")/(variableStorage.TimerVar("slowDownTimer")/Time.deltaTime));
				Vector3 move = new Vector3(variableStorage.Vector3Var("lastMousePosition").x * variableStorage.TimerVar("slowDownTimer"), variableStorage.Vector3Var("lastMousePosition").y * variableStorage.TimerVar("slowDownTimer"), 0);
				//move = RestrictMovement(-move);
				gameCam.Translate(move, Space.World);
			}
			else
			{
				variableStorage.BoolVar ("slowCameraMovement",false);
			}
			
		}
		
		if(variableStorage.BoolVar ("orthChange"))
		{
			if(variableStorage.BoolVar ("orthRise"))
				gameCam.Zoom++;
			else
				gameCam.Zoom--;
			
			gameCam.Zoom = Mathf.Clamp(gameCam.Zoom, variableStorage.IntVar("orthographicSizeMin"), variableStorage.IntVar("orthographicSizeMax") );

			variableStorage.BoolVar ("orthChange",false);
		}
		
	}
}
