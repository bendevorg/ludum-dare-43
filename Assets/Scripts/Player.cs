using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	Controller2D controller;
	Vector2 input;
	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;

	const float skinWidth = 0.55f;
	const int horizontalRayCount = 5;

	public Vector2 velocity;
	public int maxJumpVelocity = 100;

	public LayerMask groundLayer;
	bool isGrounded;
	public bool onRope;

	void Start() {
		controller = GetComponent<Controller2D>();
		collider = GetComponent<BoxCollider2D>();
	}
	
	void Update () {
		CheckForGroundBelow();
		if (!onRope) {
			controller.Move(velocity * Time.deltaTime, input);
		}
	}

	public void SetDirectionalInput(Vector2 directionalInput) {
		input = directionalInput;
	}

	public void Jump() {
		if (isGrounded && !onRope) {
			controller.Jump(maxJumpVelocity);
		}
	}

	void CheckForGroundBelow() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.max.y);
		float horizontalRaySpacing = bounds.size.x / (horizontalRayCount - 1);
		isGrounded = false;
		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = raycastOrigins.bottomLeft;
			rayOrigin += Vector2.right * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, skinWidth, groundLayer);
			if (hit) {
				isGrounded = true;
				break;
			}
		}
	}

	struct RaycastOrigins {
		public Vector2 bottomLeft, bottomRight;
	}
}
