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

		//	Remove this comment if you want to make the player able to jump
		// if (Input.GetAxisRaw("Vertical") > 0) {
		// 	player.Jump();
		// }
	}
}
