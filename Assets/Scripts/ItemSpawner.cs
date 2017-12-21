using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

	public GameObject coinPrefab;
	public GameObject obstaclePrefab;

	public int secondsBetweenSpawns=5;

	int currentTime = 0;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		if (GameManager.isGameRunning)
		{
			//var progress = Time.timeSinceLevelLoad;
			var progress = transform.position.x;
			if (currentTime != Mathf.FloorToInt(progress / secondsBetweenSpawns)) {
				currentTime = Mathf.FloorToInt(progress / secondsBetweenSpawns);
				Spawn();
			}
		}
	}

	private void Spawn()
	{
		var laneDepth = 0;

		switch (Random.Range(0, 5))
		{
			case 0: laneDepth = 0; break;
			case 1: laneDepth = 2; break;
			case 2: laneDepth = -2; break;
			case 3: laneDepth = 4; break;
			case 4: laneDepth = -4; break;
		}

		var spawnPosition = transform.position + Vector3.up * 10 + Vector3.forward * laneDepth;

		if (Random.value > 0.9f)
		{
			Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
		}
		else
		{
			Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
		}


	}
}
