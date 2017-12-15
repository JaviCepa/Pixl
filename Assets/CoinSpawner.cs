using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour {

	public GameObject coinPrefab;
	public GameObject obstaclePrefab;

	int currentTime = 0;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		if (GameManager.gameStart) {
			if (currentTime != Mathf.FloorToInt(Time.timeSinceLevelLoad)) {
				currentTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
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

		if (Random.value > 0.5f) {
			var newObject = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
		} else {
			Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
		}


	}
}
