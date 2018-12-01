using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	Controller2D controller;
	Vector2 input;

	public Vector2 velocity;
	public int maxJumpVelocity = 100;

	public LayerMask groundLayer;
	bool isGrounded;

	void Start() {
		controller = GetComponent<Controller2D>();
	}
	
	void Update () {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, groundLayer);
		isGrounded = hit?true:false;
		Debug.Log(isGrounded);
		controller.Move(velocity * Time.deltaTime, input);
	}

	public void SetDirectionalInput(Vector2 directionalInput) {
		input = directionalInput;
	}

	public void Jump() {
		if (isGrounded) {
			controller.Jump(maxJumpVelocity);
		}
	}
}
