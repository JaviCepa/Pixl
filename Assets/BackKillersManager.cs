using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackKillersManager : MonoBehaviour {

	public float startSpeed=1f;
	public float acceleration=0.01f;
	
	void Update ()
	{
		if (GameManager.isGameRunning)
		{
			startSpeed += acceleration * Time.deltaTime;
			transform.position += Vector3.right * startSpeed * Time.deltaTime;
		}
	}
}
