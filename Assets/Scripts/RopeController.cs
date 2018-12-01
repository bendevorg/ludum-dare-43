using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {

	[HideInInspector]
	public GameObject ropeShooter;
	private Player ropePlayer;
	private Rigidbody2D ropeShooterRb;
	private CircleCollider2D ropeShooterCollider;
	private SpringJoint2D rope;
	public int maxRopeFrameCount;
	private int ropeFrameCount;
	public GameObject ropeSegment;
	float ropeSegmentSize = 0.75f;
	private List<GameObject> ropeSegments = new List<GameObject>();

	public LayerMask layerMask;

	float currentRopeDistance = 0;

	void Start() {
		ropeShooter = FindObjectOfType<Player>().gameObject;
		if (ropeShooter) {
			ropeShooterRb = ropeShooter.GetComponent<Rigidbody2D>();
			ropePlayer = ropeShooter.GetComponent<Player>();
			ropeShooterCollider = ropeShooter.GetComponent<CircleCollider2D>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Fire();
		}
	}

	void LateUpdate() {
		if (rope) {
			if (ropeSegments.Count > 0) {
				float xDifference = ropeShooterCollider.bounds.center.x - rope.connectedAnchor.x;
				float yDifference = ropeShooterCollider.bounds.center.y - rope.connectedAnchor.y;
				int segmentAmount = Mathf.RoundToInt(currentRopeDistance / ropeSegmentSize);
				for (int i = 0; i < ropeSegments.Count; i++) {
					Vector3 offset = new Vector3((xDifference / segmentAmount) * i, (yDifference / segmentAmount) * i, 0);
					Vector3 connectedAnchorVector = rope.connectedAnchor;
					Vector3 ropeSegmentPosition = connectedAnchorVector + offset;
					ropeSegments[i].transform.position = ropeSegmentPosition;
				}

			} else {
				float drawedRopeSize = 0f;
				float xDifference = ropeShooterCollider.bounds.center.x - rope.connectedAnchor.x;
				float yDifference = ropeShooterCollider.bounds.center.y - rope.connectedAnchor.y;
				int segmentAmount = Mathf.RoundToInt(currentRopeDistance / ropeSegmentSize);
				for (int i = 0; i < segmentAmount; i++) {
					Vector3 offset = new Vector3((xDifference / segmentAmount) * i, (yDifference / segmentAmount) * i, 0);
					Vector3 connectedAnchorVector = rope.connectedAnchor;
					Vector3 ropeSegmentPosition = connectedAnchorVector + offset;
					ropeSegments.Add(Instantiate(ropeSegment, ropeSegmentPosition, Quaternion.identity));
				}
			}
		} else {
			//	Drop all mindus
		}
	}

	void FixedUpdate() {
		if (rope) {
			ropeFrameCount++;
			if (ropeFrameCount > maxRopeFrameCount) {
				GameObject.DestroyImmediate(rope);
				ropeFrameCount = 0;
				ropeShooterRb.gravityScale = 2.5f;
			}
		}
	}

	void Fire() {
		if (!rope) {
			ropePlayer.onRope = true;
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 position = ropeShooterCollider.bounds.center;
			Vector3 direction = mousePosition - position;

			RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity, layerMask);

			if (hit.collider) {
				currentRopeDistance = hit.distance;
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
		} else {
			DestroyRope();
		}
	}

	void DestroyRope() {
		ropePlayer.onRope = false;
		GameObject.DestroyImmediate(rope);
		ropeFrameCount = 0;
		ropeShooterRb.gravityScale = 2.5f;
	}
}
