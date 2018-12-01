using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

	const string playerTag = "Player";

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == playerTag) {
			Destroy(collider.gameObject);
		}
	}
}
