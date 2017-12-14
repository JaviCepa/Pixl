using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour {
	
	Vector3 forward { get { return transform.right; } }
	Vector3 up { get { return transform.up; } }

	public float minRadius=5, maxRadius=50;
	public float minAngle=5, maxAngle=45;

	float currentRadius=0;
	float currentAngle=0;

	public float spread=10f;

	private void Start()
	{
		currentAngle = Random.Range(minAngle, maxAngle);
		currentRadius = Random.Range(5, 50);
	}

	void Update()
	{

		GenerateTrack(currentRadius, currentAngle);

		currentAngle = Random.Range(minAngle, maxAngle);
		currentRadius *= Random.Range(0.5f, 1.5f);

		currentRadius = Mathf.Clamp(currentRadius, minRadius, maxRadius);
	}
	
	void GenerateTrack(float radius, float angle)
	{
		Vector3 centerUp = transform.position - up * radius;
		Vector3 centerDown = transform.position + up * radius;
		Vector3 upEndPoint = centerUp + (forward * Mathf.Sin(angle * Mathf.Deg2Rad) + up * Mathf.Cos(angle * Mathf.Deg2Rad)) * radius;
		Vector3 downEndPoint = centerDown + (forward * Mathf.Sin(angle * Mathf.Deg2Rad) - up * Mathf.Cos(angle * Mathf.Deg2Rad)) * radius;
		Vector3 center=Vector3.zero;
		float sign = 0;

		if (upEndPoint.x > downEndPoint.x)
		{
			sign = 1;
			center = centerUp;
		}
		else
		{
			sign = -1;
			center = centerDown;
		}

		if (Mathf.Abs(transform.position.y)/ spread > Random.value)
		{
			if (Mathf.Abs(upEndPoint.y) < Mathf.Abs(downEndPoint.y))
			{
				sign = 1;
				center = centerUp;
			}
			else
			{
				sign = -1;
				center = centerDown;
			}
		}


		//Debug.DrawLine(center, transform.position, Color.yellow, 100);
		int segments = 10;

		Vector3 pointA = transform.position;
		Vector3 pointB = transform.position;

		for (float alpha = 0; alpha < 1f; alpha += 1f / segments)
		{
			pointB = pointA;
			pointA = center + (forward * Mathf.Sin(alpha * angle * Mathf.Deg2Rad) + sign * up * Mathf.Cos(alpha * angle * Mathf.Deg2Rad)) * radius;
			Debug.DrawLine(pointA, pointB, Color.red, 100);
		}

		transform.position = pointA;
		transform.LookAt(transform.position + Vector3.Cross(up*sign, forward), center - pointA);
	}
}
