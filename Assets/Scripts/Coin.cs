using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public AudioClip coinSound;

	AudioSource aus;

	void Start()
	{
		aus = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider collider)
	{
		//aus.clip = coinSound;
		//aus.Play();
		var targetPlayer = collider.gameObject.GetComponent<PlayerController>();

		if (targetPlayer != null)
		{
			targetPlayer.Grow();
			Destroy(gameObject);
		}
	}
}
