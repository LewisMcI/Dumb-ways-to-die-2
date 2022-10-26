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

    [Header("Check Sphere")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float groundDistance = 0.01f;
    private bool isGrounded;

    private bool isCrouching;

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
        Jump();
        Crouch();
    }

    private void Move()
    {
        // Get axis
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // If moving diagonally normalise vector so that speed remains the same
        if (dir.magnitude > 1.0f)
            dir.Normalize();

        // Set velocity
        float currSpeed = (isCrouching) ? moveSpeed / 2 : moveSpeed;
        Vector3 vel = (transform.right * dir.x + transform.forward * dir.y) * currSpeed * Time.deltaTime;
        vel.y = rig.velocity.y;
        // Apply velocity
        rig.velocity = vel;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
            rig.velocity = new Vector3(0, jumpForce, 0);
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = Vector3.one;
            isCrouching = false;
        }
    }
    #endregion
}