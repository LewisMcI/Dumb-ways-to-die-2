using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopdownPlayerController : MonoBehaviour
{
    float speed = 100.0f;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Move();
    }

    private void Move()
    {

        // Get axis
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // If moving diagonally normalise vector so that speed remains the same
        if (dir.magnitude > 1.0f)
            dir.Normalize();
        Vector3 vel = ((transform.right * dir.x + transform.forward * dir.y) * speed) * Time.fixedDeltaTime;
        vel.y = rb.velocity.y;
        // Apply velocity
        rb.velocity = vel;
        if (vel.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(vel, Vector3.up);
        }
    }
}
