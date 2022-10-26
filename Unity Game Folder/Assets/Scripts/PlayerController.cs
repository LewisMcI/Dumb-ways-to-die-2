using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region fields
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    [Range(0f, 0.5f)]
    private float moveSmoothTime;

    [Header("Check Sphere")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float groundDistance = 0.01f;
    private bool isGrounded;

    private Rigidbody rig;
    #endregion

    #region methods
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Move();

        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();
    }

    private void Move()
    {
        // Get axis
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // If moving diagonally normalise vector so that speed remains the same
        if (dir.magnitude > 1.0f)
            dir.Normalize();

        // Set velocity
        Vector3 vel = (transform.right * dir.x + transform.forward * dir.y) * moveSpeed * Time.deltaTime;
        vel.y = rig.velocity.y;
        // Apply velocity
        rig.velocity = vel;
    }

    private void Jump()
    {
        rig.velocity = new Vector3(0, jumpForce, 0);
    }
    #endregion
}