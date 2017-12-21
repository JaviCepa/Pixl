using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerTrigger : MonoBehaviour {
	
	public GameObject killParticles;

	private void OnTriggerEnter(Collider other)
	{
		var player = other.GetComponent<PlayerController>();
		if (player != null)
		{
			var particles = Instantiate(killParticles, player.transform.position, killParticles.transform.rotation);
			particles.BroadcastMessage("HitPlayer", player);
			Destroy(particles.gameObject, 10);
			Destroy(player.gameObject);
		}
	}
}
