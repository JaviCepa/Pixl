using UnityEngine;
using System.Collections;

public class GravityManager : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (TimeCounter.gravitySwitch) {
			if (name=="Player") {
				GetComponent<Rigidbody>().useGravity=true;
			}
		}
		
		if (!TimeCounter.gameStart) {
			if (Random.value>0.999f) {
				iTween.RotateTo(gameObject, new Vector3(0,360,0), 1f);
			}
			if (Random.value>0.999f) {
				iTween.ShakeScale(gameObject, Vector3.one*0.1f, 1f);
			}
		}
	}
}
