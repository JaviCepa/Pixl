using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DestroyerTrigger : MonoBehaviour {
	
	public GameObject killParticles;

	private void OnTriggerEnter(Collider other)
	{
		var player = other.GetComponent<PlayerController>();
		if (player != null && player.isAlive)
		{
			player.isAlive = false;
			Sequence sequence = DOTween.Sequence();
			sequence.Append(player.transform.DOScale(player.transform.localScale*1.5f, 0.2f).SetEase(Ease.OutExpo));
			sequence.AppendCallback(() =>
			{
				var particles = Instantiate(killParticles, player.transform.position, killParticles.transform.rotation);
				particles.BroadcastMessage("HitPlayer", player);
				Destroy(particles.gameObject, 10);
				Destroy(player.gameObject);
			});
		}
	}
}
