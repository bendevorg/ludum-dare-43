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

  private AudioSource source;
  private float volLowRange = .5f;
  private float volHighRange = 1f;
  public AudioClip shootRope;

  public LayerMask layerMask;

  float currentRopeDistance = 0;
	public float maxFallingSegmentXForce = 400f;
	public float maxFallingSegmentYForce = 100f;
	public float maxFallingSegmentTorque = 200f;

	public int amountOfSegmentIdleAnimation = 2;
	public int amountOfSegmentDeadAnimation = 2;

  void Start() {
    ropeShooter = FindObjectOfType<Player>().gameObject;
    source = GetComponent<AudioSource>();
    if (ropeShooter) {
      ropeShooterRb = ropeShooter.GetComponent<Rigidbody2D>();
      ropePlayer = ropeShooter.GetComponent<Player>();
      ropeShooterCollider = ropeShooter.GetComponent<CircleCollider2D>();
    }
  }

  void Update() {
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
        float xDifference = ropeShooterCollider.bounds.center.x - rope.connectedAnchor.x;
        float yDifference = ropeShooterCollider.bounds.center.y - rope.connectedAnchor.y;
        int segmentAmount = Mathf.RoundToInt(currentRopeDistance / ropeSegmentSize);
        for (int i = 0; i < segmentAmount; i++) {
          Vector3 offset = new Vector3((xDifference / segmentAmount) * i, (yDifference / segmentAmount) * i, 0);
          Vector3 connectedAnchorVector = rope.connectedAnchor;
          Vector3 ropeSegmentPosition = connectedAnchorVector + offset;
          ropeSegments.Add(Instantiate(ropeSegment, ropeSegmentPosition, Quaternion.identity));
					ropeSegments[i].GetComponent<Animator>().SetInteger("Hold", Random.Range(1, amountOfSegmentIdleAnimation));
        }
      }
    } else {
      
    }
  }

  void FixedUpdate(){
    if (rope) {
      ropeFrameCount++;
      if (ropeFrameCount > maxRopeFrameCount) {
        GameObject.DestroyImmediate(rope);
        ropeFrameCount = 0;
        ropeShooterRb.gravityScale = 2.5f;
				DropSegments();
				ropeSegments = new List<GameObject>();
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
				float vol = Random.Range(volLowRange, volHighRange);
				source.PlayOneShot(shootRope, vol);

				GameObject.DestroyImmediate(rope);
				rope = newRope;
				ropeFrameCount = 0;
				ropeShooterRb.gravityScale = 3;
			}
    } else {
      DestroyRope();
			DropSegments();
			ropeSegments = new List<GameObject>();
    }
  }

	void DropSegments() {
		for (int i = 0; i < ropeSegments.Count; i++) {
			Rigidbody2D currentSegmentRigidbody = ropeSegments[i].GetComponent<Rigidbody2D>();
			currentSegmentRigidbody.isKinematic = false;
			ropeSegments[i].GetComponent<Animator>().SetInteger("Dead", Random.Range(1, amountOfSegmentDeadAnimation));
			currentSegmentRigidbody.AddForce(new Vector2(Random.Range(-maxFallingSegmentXForce, maxFallingSegmentXForce), 
				Random.Range(-maxFallingSegmentYForce, maxFallingSegmentYForce)));
			currentSegmentRigidbody.AddTorque(Random.Range(-maxFallingSegmentTorque, maxFallingSegmentTorque));
		}
	}

  void DestroyRope() {
    ropePlayer.onRope = false;
    GameObject.DestroyImmediate(rope);
    ropeFrameCount = 0;
    ropeShooterRb.gravityScale = 2.5f;
  }
}
