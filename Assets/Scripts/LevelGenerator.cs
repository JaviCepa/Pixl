using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	
	public GameObject SmallPlatform;
	public GameObject SlopeUp;
	public GameObject SlopeDown;
	public GameObject Column;
	public GameObject Coin;
	public GameObject Wall;
	
	public Transform  trackSpawner1, trackSpawner2, trackSpawner3, trackSpawner4, trackSpawner5;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (TimeCounter.gameStart) {
			if (trackSpawner1.transform.position.x<30*0.01f) {GenerateTrack(trackSpawner1);}
			
			if (Random.value>0.95){
				Instantiate(Coin, new Vector3(Random.Range(40,60), 10, 2*Mathf.Round(4*(Random.value-0.5f))), Quaternion.identity);
			}
		}
	}
	
	void GenerateTrack(Transform trackSpawner) {
		Debug.Log("New track");
		float scale=Mathf.Round(1+Random.value*4);
		switch (Random.Range(0,8))  {
			case 0: if (Chance(100)) {trackSpawner.transform.position+=new Vector3(0.5f,0,0);       Instantiate(SmallPlatform, trackSpawner.transform.position, SmallPlatform.transform.rotation); trackSpawner.transform.position+=new Vector3(0.5f,0,0);       }; break;
			case 1: if (Chance(2))   {trackSpawner.transform.position+=new Vector3(0.5f*scale,0,0);                                                                             trackSpawner.transform.position+=new Vector3(0.5f*scale,0,0); }; break;
			case 2: if (Chance(5))   {trackSpawner.transform.position+=new Vector3(2f,0.5f,0);      Instantiate(SlopeUp,       trackSpawner.transform.position, SlopeUp.transform.rotation);       trackSpawner.transform.position+=new Vector3(2f,0.5f,0);      }; break;
			case 3: if (Chance(5))   {trackSpawner.transform.position+=new Vector3(2,-0.5f,0);      Instantiate(SlopeDown,     trackSpawner.transform.position, SlopeDown.transform.rotation);     trackSpawner.transform.position+=new Vector3(2,-0.5f,0);      }; break;
			case 4: if (Chance(1))   {trackSpawner.transform.position+=new Vector3(0.5f,0,0);       Instantiate(Column,        trackSpawner.transform.position, Column.transform.rotation);        trackSpawner.transform.position+=new Vector3(0.5f,0,0);       }; break;
			case 5: if (Chance(5))   {trackSpawner.transform.position+=new Vector3(0.5f,0,0);       Instantiate(SmallPlatform, trackSpawner.transform.position, SmallPlatform.transform.rotation); trackSpawner.transform.position =new Vector3(trackSpawner.transform.position.x+0.5f,0,trackSpawner.transform.position.z);       }; break;
			case 6: if (Chance(2))   {trackSpawner.transform.position+=new Vector3(0,1f,0);         Instantiate(Wall,          trackSpawner.transform.position, Wall.transform.rotation);          trackSpawner.transform.position+=new Vector3(0,1f,0);        }; break;
			case 7: if (Chance(2))   {trackSpawner.transform.position += new Vector3(0,-1f,0);      Instantiate(Wall,          trackSpawner.transform.position, Wall.transform.rotation);        trackSpawner.transform.position += new Vector3(0,-1f,0);       }; break;
		}
	}
	
	bool Chance(float odds) {
		return Random.value<odds*0.01;
	}
}
