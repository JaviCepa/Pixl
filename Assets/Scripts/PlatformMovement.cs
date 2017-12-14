using UnityEngine;
using System.Collections;

public class PlatformMovement : MonoBehaviour {
	
	public Material middleLane;
	
	// Use this for initialization
	void Start () {
		if (transform.position.z<=1 && transform.position.z>=-1)	{GetComponent<Renderer>().material=middleLane;}
	}
	
	// Update is called once per frame
	void Update () {
		if (TimeCounter.gameStart) {
			//transform.Translate(-TimeCounter.levelSpeed,0,0);
		}
		
		if (transform.position.x<-50) {
			Destroy(gameObject);
		}
	}
}
