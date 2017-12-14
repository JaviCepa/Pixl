using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	
	public GameObject target;
	public float offset;

	void Update ()
	{
		if (TimeCounter.gameStart)
		{
			transform.position+=new Vector3(TimeCounter.levelSpeed,0,0);
		}
		
	}
}
