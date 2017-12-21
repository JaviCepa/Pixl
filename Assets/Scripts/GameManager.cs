using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour {
	
	public GameObject recordDisplay;
	
	static public bool gravitySwitch;
	static public bool isGameRunning;
	
	static public float gameStartTime;
	static public float levelSpeed;
	
	static public float record=0;
	
	public float distance=0;

	static PlayerController[] players;

	public DOTweenAnimation fader;

	void Start ()
	{
		players = FindObjectsOfType<PlayerController>();
		gravitySwitch=false;
		Time.timeScale=1f;
		levelSpeed=0.0f;
		isGameRunning=false;
		distance=0;
	}
	
	public static void StartGame()
	{
		if (!isGameRunning)
		{
			var t=Time.time;
			isGameRunning = true;
			gameStartTime = t;
		}
	}

	void Update ()
	{

		if (Input.GetKeyDown(KeyCode.Escape)) { RestartGame(); }
		
		distance = Mathf.Max(Camera.main.transform.position.x + 42f, 0);

		if (distance - 22 > record)
		{
			record = distance - 22;
		}

		if (GetCurrentPlayers()==0 && isGameRunning) {
			isGameRunning = false;
			Invoke("RestartGame", 1f);
		}
		
		recordDisplay.GetComponent<TextMesh>().text="Previous record:\n"+Mathf.Floor(record);
	}

	void RestartGame()
	{
		fader.GetComponent<UnityEngine.UI.Image>().DOFade(1f, 0.2f);
		Invoke("ReloadLevel", 0.3f);
	}

	void ReloadLevel()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
	}

	public static int GetCurrentPlayers()
	{
		int count = 0;

		for (int i = 0; i < players.Length; i++)
		{
			if (players[i] != null) {
				count++;
			}
		}

		return count;
	}
}
