using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{

    [HideInInspector]
    public GameObject ropeShooter;
    private Player ropePlayer;
    private Rigidbody2D ropeShooterRb;
    private CircleCollider2D ropeShooterCollider;
    private SpringJoint2D rope;
    public int maxRopeFrameCount;
    private int ropeFrameCount;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1f;
    public AudioClip shootRope;

    public LineRenderer lineRenderer;
    public LayerMask layerMask;

    void Start()
    {
        ropeShooter = FindObjectOfType<Player>().gameObject;
        source = GetComponent<AudioSource>();
        if (ropeShooter)
        {
            ropeShooterRb = ropeShooter.GetComponent<Rigidbody2D>();
            ropePlayer = ropeShooter.GetComponent<Player>();
            ropeShooterCollider = ropeShooter.GetComponent<CircleCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void LateUpdate()
    {
        if (rope)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, ropeShooterCollider.bounds.center);
            lineRenderer.SetPosition(1, rope.connectedAnchor);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (rope)
        {
            ropeFrameCount++;
            if (ropeFrameCount > maxRopeFrameCount)
            {
                GameObject.DestroyImmediate(rope);
                ropeFrameCount = 0;
                ropeShooterRb.gravityScale = 2.5f;
            }
        }
    }

    void Fire()
    {

        if (!rope)
        {
            ropePlayer.onRope = true;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = ropeShooterCollider.bounds.center;
            Vector3 direction = mousePosition - position;

            RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity, layerMask);

            if (hit.collider)
            {
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
        }
        else
        {
            DestroyRope();
        }
    }

    void DestroyRope()
    {
        ropePlayer.onRope = false;
        GameObject.DestroyImmediate(rope);
        ropeFrameCount = 0;
        ropeShooterRb.gravityScale = 2.5f;
    }
}
