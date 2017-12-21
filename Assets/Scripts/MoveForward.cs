using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {
	
	void Update ()
	{
		if (GameManager.isGameRunning)
		{
			transform.position+=new Vector3(GameManager.levelSpeed,0,0);
		}
		
	}
}
