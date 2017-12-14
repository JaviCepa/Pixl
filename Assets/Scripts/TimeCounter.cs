using UnityEngine;
using System.Collections;

public class TimeCounter : MonoBehaviour {
	
	public GameObject fader;
	public GameObject cameraManager;
	public GameObject scoreDisplay;
	public GameObject recordDisplay;
	
	static public bool gravitySwitch;
	static public bool gameStart;
	
	static public float gameStartTime;
	static public float levelSpeed;
	
	static public int killCount=0;
	static public float record=0;
	
	private float distance=0;
	
	// Use this for initialization
	void Start () {
		killCount=0;
		gravitySwitch=false;
		fader.GetComponent<FadeIn>().Begin();
		Time.timeScale=1f;
		levelSpeed=0.0f;
		gameStart=false;
		distance=0;
	}
	
	// Update is called once per frame
	void Update () {
		var t=Time.realtimeSinceStartup;
		
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
			gameStart=true;
			gameStartTime=t;
		};
		
		if (gameStart) {
			gravitySwitch=true;
			if (levelSpeed<2*0.01f) {
				levelSpeed+=0.02f*0.01f;
			}
			if (levelSpeed>=2*0.01f) {
				levelSpeed+=0.01f*0.01f;
			}
			if (levelSpeed>4*0.01f) {
				levelSpeed=4*0.01f;
			}
			
			distance+=levelSpeed;
			if (distance>record) {record=distance;}
		}
		
		if (killCount>=2) {
			fader.GetComponent<FadeIn>().End();
			Application.LoadLevel("Main");
			TimeCounter.gameStart=false;
			killCount=0;
		}
		
		scoreDisplay.GetComponent<TextMesh>().text="Distance: "+Mathf.Floor(distance/100);
		recordDisplay.GetComponent<TextMesh>().text="Record: "+Mathf.Floor(record/100);
	}
}
