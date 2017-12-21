using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public PlayerController[] playerControllers;

	Vector3 startPosition;

	public float startX=0;
	public float cameraAcceleration=0.05f;
	public float offsetX;
	public float maxOffsetX=-12f;

	float cameraTime=0;
	float offsetY;

	float previousX;

	public static float currentCameraSpeed { get { return (instance.offsetX - instance.startPosition.x) / (instance.maxOffsetX - instance.startPosition.x); } }

	static CameraMovement instance;

	void Awake()
	{
		instance = this;
		startPosition = transform.position;
		previousX = startPosition.x;
		offsetX = startPosition.x;
		offsetY = startPosition.y;
		playerControllers = FindObjectsOfType<PlayerController>();
	}
	
	void Update ()
	{
		if (GameManager.isGameRunning)
		{
			previousX = transform.position.x;

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

				var targetPosition = new Vector3(weightedAverage.x + offsetX, weightedAverage.y + offsetY, startPosition.z);
				var error = targetPosition - transform.position;
				if (targetPosition.x > startX)
				{
					float speedFactor = 1f - (GameManager.GetCurrentPlayers()-1f)/4f;
					if (offsetX<maxOffsetX)
					{
						offsetX += 0.3f * speedFactor * Time.deltaTime; // Increase difficulty over time
					}
					transform.position += error * cameraAcceleration;
				}
			}
		}
	}
}
