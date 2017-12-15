using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlatform : MonoBehaviour {

	public TrackSpawner spawner;

	Vector3 platformPivot;
	
	void Awake()
	{
		platformPivot = new Vector3(-4, transform.position.y, transform.position.z);
		float platformSize = Random.Range(5, 10);
		transform.position = platformPivot + Vector3.right * platformSize * 0.5f;
		transform.localScale = new Vector3(platformSize, transform.localScale.y, transform.localScale.z);
		spawner.transform.position = platformPivot + Vector3.right * platformSize;
	}
}
