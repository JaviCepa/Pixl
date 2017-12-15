using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneColor : MonoBehaviour {

	public Material centralLaneMaterial;
	public Material leftLaneMaterial;
	public Material rightLaneMaterial;
	public Material leftmostLaneMaterial;
	public Material rightmostLaneMaterial;

	void Start () {
		switch (Mathf.RoundToInt(transform.position.z))
		{
			case 0: SetMaterial(centralLaneMaterial); break;
			case 2: SetMaterial(leftLaneMaterial); break;
			case -2: SetMaterial(rightLaneMaterial); break;
			case 4: SetMaterial(leftmostLaneMaterial); break;
			case -4: SetMaterial(rightmostLaneMaterial); break;
			default:
				break;
		}
	}
	
	void SetMaterial(Material mat) {
		foreach (var item in GetComponentsInChildren<MeshRenderer>())
		{
			item.material = mat;
		}
	}
}
