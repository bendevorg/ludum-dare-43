using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {

	Player player;

	void Start () {
		player = GetComponent<Player>();
	}

	void Update () {
		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
		player.SetDirectionalInput(directionalInput);

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			player.Jump();
		}
	}
}
