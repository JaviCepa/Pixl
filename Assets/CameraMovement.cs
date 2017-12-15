using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public PlayerController[] playerControllers;

	Vector3 startPosition;

	float offsetX, offsetY;

	void Awake()
	{
		startPosition = transform.position;
		offsetX = startPosition.x;
		offsetY = startPosition.y;
		playerControllers = FindObjectsOfType<PlayerController>();
	}
	
	void Update ()
	{
		if (GameManager.gameStart)
		{
			Vector3 average = Vector3.zero;
			float count = 0;
			for (int i = 0; i < playerControllers.Length; i++)
			{
				if (Mathf.Abs(playerControllers[i].transform.position.y) < 10)
				{
					average += playerControllers[i].transform.position;
					count++;
				}
			}
			if (count > 0)
			{
				average = average / count;

				Vector3 weightedAverage = Vector3.zero;
				float totalWeight = 0;
				for (int i = 0; i < playerControllers.Length; i++)
				{
					if (Mathf.Abs(playerControllers[i].transform.position.y) < 10)
					{
						var currentWeight = (playerControllers[i].transform.position - average).magnitude + 1;
						currentWeight = 1f / currentWeight;
						weightedAverage += playerControllers[i].transform.position * currentWeight;
						totalWeight += currentWeight;
					}
				}
				if (totalWeight != 0)
				{
					weightedAverage = weightedAverage / totalWeight;
				}
				else
				{
					weightedAverage = average;
				}

				var targetPosition = new Vector3(weightedAverage.x + offsetX, weightedAverage.y + offsetY, startPosition.z);
				var error = targetPosition - transform.position;
				transform.position += error * 0.01f;
			}
		}
	}
}
