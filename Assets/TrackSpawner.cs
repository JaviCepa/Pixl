using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour {
	
	Vector3 forward { get { return transform.right; } }
	Vector3 up { get { return transform.up; } }

	public float minRadius=5, maxRadius=50;
	public float minAngle=5;

	float maxAngle;

	float currentRadius=0;
	float currentAngle=0;

	public float spread=10f;

	public GameObject platformPrefab;
	public GameObject platformEndPrefab;

	public Material material;

	public Transform targetTransform;

	bool active = true;

	private void Start()
	{
		maxAngle = Random.Range(15f, 45f);
		currentAngle = minAngle;
		currentRadius = maxRadius;
	}

	void Update()
	{

		if (transform.position.x < targetTransform.position.x+100)
		{
			GenerateTrack(currentRadius, currentAngle);

			currentAngle = Random.Range(minAngle, maxAngle);
			currentRadius *= Random.Range(0.25f, 1.75f);

			currentRadius = Mathf.Clamp(currentRadius, minRadius, maxRadius);
		}
	}
	
	void GenerateTrack(float radius, float angle)
	{
		var previouslyActive = active;
		active = (transform.position.y >= -3);
		if (Random.value < 0.05f) { active = false; } //Make some segments randomly dissapear

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


		float segments = 20;

		Vector3 pointA = transform.position;
		Vector3 pointB = transform.position;
		Vector3 backgroundDir = Vector3.Cross(up*sign, forward);
		Vector3 radialDir = (forward * Mathf.Sin(0 * angle * Mathf.Deg2Rad) + sign * up * Mathf.Cos(0 * angle * Mathf.Deg2Rad)) * radius;
		Vector3 previousRadialDir = radialDir;

		float size = angle / segments * Mathf.Deg2Rad * radius * 1.05f;
		Vector3 startPoint = transform.position-forward*size*0.5f;

		for (float alpha = 0; alpha < 1f; alpha += 1f / segments)
		{
			pointB = pointA;
			previousRadialDir = radialDir;
			radialDir = (forward * Mathf.Sin(alpha * angle * Mathf.Deg2Rad) + sign * up * Mathf.Cos(alpha * angle * Mathf.Deg2Rad)) * radius;
			pointA = center + radialDir;
			Debug.DrawLine(pointA, pointB, Color.red, 100);

			if (active)
			{
				var newPlatform = Instantiate(platformPrefab, (pointB + pointA) / 2f, Quaternion.LookRotation(backgroundDir, (radialDir+previousRadialDir)/2f));
				foreach (var item in newPlatform.GetComponentsInChildren<MeshRenderer>())
				{
					item.material = material;
				}

				newPlatform.transform.localScale = new Vector3(size, newPlatform.transform.localScale.y, newPlatform.transform.localScale.z);

				//CODE FOR PILLARS
				//if (Random.value < 0.995f)
				//{
				//	newPlatform.transform.localScale = new Vector3(size, newPlatform.transform.localScale.y, newPlatform.transform.localScale.z);
				//}
				//else
				//{
				//	if (Random.value > 0.5f) {
				//		newPlatform.transform.localScale = new Vector3(size, 200f, newPlatform.transform.localScale.z);
				//	}
				//	else
				//	{
				//		newPlatform.transform.localScale = new Vector3(size, newPlatform.transform.localScale.y, 200f);
				//	}
				//}

			}
		}

		Vector3 endPoint = pointA;

		//TRACK END
		if (previouslyActive && !active)
		{
			var newPlatform = Instantiate(platformEndPrefab, transform.position, transform.rotation);
			foreach (var item in newPlatform.GetComponentsInChildren<MeshRenderer>())
			{
				item.material = material;
			}
		}

		transform.position = endPoint;
		transform.LookAt(transform.position + backgroundDir, center - pointA);

		//TRACK START
		if (!previouslyActive && active)
		{
			var newPlatform = Instantiate(platformEndPrefab, startPoint, transform.rotation);
			foreach (var item in newPlatform.GetComponentsInChildren<MeshRenderer>())
			{
				item.material = material;
			}
		}

		//DRAW RADIUS
		//Debug.DrawLine(center, startPoint, Color.yellow, 100);
		//Debug.DrawLine(center, endPoint, Color.yellow, 100);
	}
}
