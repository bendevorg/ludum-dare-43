using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	const string playerTag = "Player";

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == playerTag) {
			GameController.gameController.LevelCleared();
		}
	}
}
