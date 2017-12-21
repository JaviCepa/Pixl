using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public PlayerController[] playerControllers;

	Vector3 startPosition;

	public float startX=0;
	public float cameraAcceleration=0.05f;
	public float offsetX;

	float cameraTime=0;
	float offsetY;

	static CameraMovement instance;

	Vector3 targetPosition;

	public static bool shouldFollow {get { return instance.targetPosition.x > instance.startX; } }

	void Awake()
	{
		instance = this;
		startPosition = transform.position;
		offsetX = startPosition.x;
		offsetY = startPosition.y;
		playerControllers = FindObjectsOfType<PlayerController>();
	}
	
	void Update ()
	{
		if (GameManager.isGameRunning)
		{
			cameraTime += Time.deltaTime;
			Vector3 average = Vector3.zero;
			float count = 0;
			for (int i = 0; i < playerControllers.Length; i++)
			{
				if (playerControllers[i]!=null && playerControllers[i].transform.position.y > -10)
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
					if (playerControllers[i] != null && playerControllers[i].transform.position.y > -10 && playerControllers[i].transform.position.x > average.x)
					{
						var currentWeight = Mathf.Max(playerControllers[i].transform.position.x*playerControllers[i].transform.position.x, 1);
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

				targetPosition = new Vector3(weightedAverage.x + offsetX, weightedAverage.y + offsetY, startPosition.z);
				var error = targetPosition - transform.position;
				if (shouldFollow)
				{
					transform.position += error * cameraAcceleration;
				}
			}
		}
	}
}
