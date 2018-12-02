using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	const string playerTag = "Player";
	private AudioSource source;
	public AudioClip goal;

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == playerTag) {
			source = GetComponent<AudioSource>();
			source.PlayOneShot(goal, 1f);
			GameController.gameController.LevelCleared();
		}
	}
}