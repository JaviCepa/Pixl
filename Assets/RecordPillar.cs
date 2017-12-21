using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPillar : MonoBehaviour {

	void Start () {
		float recordPosition = GameManager.record-4;
		if (recordPosition>10) {
			transform.position = new Vector3(recordPosition, transform.position.y, transform.position.z);
		}
	}
	

}
