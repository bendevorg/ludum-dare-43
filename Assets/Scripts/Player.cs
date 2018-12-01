using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

	Controller2D controller;
	Vector2 input;
	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;

	public PhysicsMaterial2D bouncinessMaterial;
	private PhysicsMaterial2D oldShooterMaterial;

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
			collider.sharedMaterial.bounciness = 0.75f;
			ResetCollider();
			controller.Move(velocity * Time.deltaTime, input);
		} else {
			collider.sharedMaterial.bounciness = 1f;
			ResetCollider();
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

	void ResetCollider() {
		collider.enabled = false;
		collider.enabled = true;
	}

	struct RaycastOrigins {
		public Vector2 bottomLeft, bottomRight;
	}
}
