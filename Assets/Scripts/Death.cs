using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

	const string playerTag = "Player";
	private AudioSource audioSource;
	public AudioClip deathClip;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == playerTag) {

			// collider.gameObject.SetActive(false);

			audioSource = GetComponent<AudioSource>();

			audioSource.PlayOneShot(deathClip, 1F);

			Destroy(collider.gameObject);
			GameController.gameController.PlayerDeath();
		}
	}
}