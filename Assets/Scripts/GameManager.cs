using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public GameObject scoreDisplay;
	public GameObject recordDisplay;
	
	static public bool gravitySwitch;
	static public bool gameStart;
	
	static public float gameStartTime;
	static public float levelSpeed;
	
	static public int killCount=0;
	static public float record=0;
	
	public float distance=0;

	public float currentPlayers=5;

	void Start ()
	{
		killCount=0;
		gravitySwitch=false;
		Time.timeScale=1f;
		levelSpeed=0.0f;
		gameStart=false;
		distance=0;
	}
	
	void Update ()
	{
		var t=Time.realtimeSinceStartup;
		
		if (Input.anyKeyDown && !gameStart)
		{
			gameStart=true;
			gameStartTime=t;
		};

		distance = Mathf.Max(Camera.main.transform.position.x + 42f, 0);

		if (distance > record)
		{
			record = distance;
		}

		if (killCount >= currentPlayers) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
			GameManager.gameStart=false;
			killCount=0;
		}
		
		scoreDisplay.GetComponent<TextMesh>().text="Distance: "+Mathf.Floor(distance);
		recordDisplay.GetComponent<TextMesh>().text="Record: "+Mathf.Floor(record);
	}
}
