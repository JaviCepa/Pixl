using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y<-20) {
			
			GetComponent<Renderer>().material.color=new Color(255,255,255,0);
			GetComponent<Renderer>().enabled=false;
			GetComponent<Rigidbody>().Sleep();
			this.enabled=false;
		}
	}
}
