using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackKiller : MonoBehaviour {

	public LayerMask mask;

	void LateUpdate ()
	{
		Ray ray = new Ray(transform.position+Vector3.up*100, Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 200, mask.value))
		{
			transform.position = new Vector3(transform.position.x, hit.point.y+transform.lossyScale.y*0.5f, transform.position.z);
		}
		else
		{
			transform.position = new Vector3(transform.position.x, -20f, transform.position.z);
		}
	}
}
