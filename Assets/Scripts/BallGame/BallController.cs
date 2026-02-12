using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BallController : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;
    new SphereCollider collider;
    public float speed = 500;
    public float jumpHeight = 100;
    public float groundDetectionShrink = 0.1f;
    public float groundDetectionLength = 0.2f;
    public LayerMask groundDetectionLayers;
    public LayerMask killLayers;
    public Vector3 spawnPoint;
    public bool grounded { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        collider = GetComponent<SphereCollider>();
        spawnPoint = transform.position;
        BallGameManager.instance.ball = this;
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        grounded = Physics.SphereCast(transform.position, collider.radius - groundDetectionShrink, Physics.gravity, out hit, groundDetectionLength, groundDetectionLayers);
    }
    public void Jump()
    {
        if (!grounded) return;
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
    }
    public void Move(Vector2 direction)
    {
        Vector3 newDirection = new Vector3(direction.x, 0, direction.y);
        float magnitude = newDirection.magnitude;
        if (magnitude > 0)
        {
            newDirection /= magnitude;
            magnitude = Mathf.Clamp01(magnitude);
            if (cam)
            {
                newDirection = cam.transform.TransformDirection(newDirection);
            }
            newDirection = Vector3.ProjectOnPlane(newDirection, Vector3.up).normalized * magnitude;
        }
        Move(newDirection);
    }
    public void Move(Vector3 directionWorld)
    {
        rb.AddForce(directionWorld * speed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (!collider) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collider.radius - groundDetectionShrink);
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + Physics.gravity.normalized * groundDetectionLength, collider.radius - groundDetectionShrink);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckKill(other.gameObject))
        {
            Kill();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (CheckKill(collision.collider.gameObject))
        {
            Kill();
        }
    }

    public bool CheckKill(GameObject go)
    {
        return ((killLayers & (1 << go.layer)) != 0);
    }
    public void Kill()
    {
        transform.position = spawnPoint;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
