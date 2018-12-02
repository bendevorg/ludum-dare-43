using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

	const string playerTag = "Player";
	private AudioSource audioSource;
	public AudioClip deathClip;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == playerTag) {

			audioSource = GetComponent<AudioSource>();
			audioSource.PlayOneShot(deathClip, 1F);

			collider.gameObject.SetActive(false);
			GameController.gameController.PlayerDeath();
		}
	}
}