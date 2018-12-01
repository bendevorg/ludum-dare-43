using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {

	public GameObject ropeShooter;
	private Rigidbody2D ropeShooterRb;
	private SpringJoint2D rope;
	public int maxRopeFrameCount;
	private int ropeFrameCount;

	public LineRenderer lineRenderer;
	public LayerMask layerMask;

	void Start() {
		if (ropeShooter) {
			ropeShooterRb = ropeShooter.GetComponent<Rigidbody2D>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Fire();
		} else if (Input.GetKeyDown(KeyCode.Space)) { 
			DestroyRope();
		}
	}

	void LateUpdate() {
		if (rope) {
			lineRenderer.enabled = true;
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, ropeShooter.transform.position);
			lineRenderer.SetPosition(1, rope.connectedAnchor);
		} else {
			lineRenderer.enabled = false;
		}
	}

	void FixedUpdate() {
		if (rope) {
			ropeFrameCount++;
			if (ropeFrameCount > maxRopeFrameCount) {
				GameObject.DestroyImmediate(rope);
				ropeFrameCount = 0;
				ropeShooterRb.gravityScale = 1;
			}
		}
	}

	void Fire() {
		if (!rope) {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 position = ropeShooter.transform.position;
			Vector3 direction = mousePosition - position;

			RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity, layerMask);

			if (hit.collider) {
				SpringJoint2D newRope = ropeShooter.AddComponent<SpringJoint2D>();
				newRope.enableCollision = true;
				newRope.frequency = 0f;
				newRope.connectedAnchor = hit.point;
				newRope.enabled = true;

				GameObject.DestroyImmediate(rope);
				rope = newRope;
				ropeFrameCount = 0;
				ropeShooterRb.gravityScale = 3;
			}
		}
	}

	void DestroyRope() {
		GameObject.DestroyImmediate(rope);
		ropeFrameCount = 0;
		ropeShooterRb.gravityScale = 1;
	}
}
