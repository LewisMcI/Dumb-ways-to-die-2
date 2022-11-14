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

    [SerializeField]
    private GameObject notepad;
    private bool isCrouching, isJumping;

    private Rigidbody[] limbs;
    [SerializeField]
    private Camera kitchenCam;
    private float restartTimer = 5f;
    private bool dead;

    private Rigidbody rig;
    private Animator anim;
    #endregion

    #region properties
    public bool Dead
    {
        get { return dead; }
        set { dead = value; }
    }
    #endregion

    #region methods
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        limbs =  transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
        kitchenCam.enabled = false;
    }

    private void Update()
    {
        if (GameManager.Instance.EnableControls)
        {
            Jump();
            Crouch();
        }

        // Notepad
        if (Input.GetButtonDown("GameUI"))
            anim.SetBool("Notepad", !anim.GetBool("Notepad"));

        if (dead)
        {
            if (restartTimer <= 0.0f)
                GameManager.Instance.Restart();
            else
                restartTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (GameManager.Instance.EnableControls)
        {
            Move();
        }
    }
    private void Move()
    {
        // Get axis
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // If moving diagonally normalise vector so that speed remains the same
        if (dir.magnitude > 1.0f)
            dir.Normalize();
        // Set animation parameters
        anim.SetFloat("dirX", dir.x);
        anim.SetFloat("dirY", dir.y);
        // Set velocity
        float currSpeed = (isCrouching) ? moveSpeed / 2 : moveSpeed;
        Vector3 vel = ((transform.right * dir.x + transform.forward * dir.y) * currSpeed) * Time.fixedDeltaTime;
        vel.y = rig.velocity.y;
        // Apply velocity
        rig.velocity = vel;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            // Add force
            rig.velocity = new Vector3(0, jumpForce, 0);
            // Trigger jump animation
            anim.SetBool("Jumping", true);
            // Set
            isJumping = true;
        }

        if (isJumping && rig.velocity.y <= 0.0f)
        {
            if (isGrounded)
            {
                // Trigger jump animation
                anim.SetBool("Jumping", false);
                // Reset
                isJumping = false;
            }
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            // Scale collider
            float reductionScale = 0.7f;
            transform.GetChild(0).GetComponent<CapsuleCollider>().center = new Vector3(0.0f, 0.9f * reductionScale, 0.0f);
            transform.GetChild(0).GetComponent<CapsuleCollider>().height = 1.8f * reductionScale;

            // Trigger crouching animation
            anim.SetBool("Crouching", true);

            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            // Reset collider
            transform.GetChild(0).GetComponent<CapsuleCollider>().center = new Vector3(0.0f, 0.9f, 0.0f);
            transform.GetChild(0).GetComponent<CapsuleCollider>().height = 1.8f;

            // Trigger standing animation
            anim.SetBool("Crouching", false);

            isCrouching = false;
        }
    }

    private void DisableRagdoll()
    {
        foreach (Rigidbody rig in limbs)
        {
            rig.isKinematic = true;
            rig.detectCollisions = false;
        }
    }

    private void EnableRagdoll()
    {
        GameManager.Instance.EnableControls = false;
        anim.enabled = false;
        foreach (Rigidbody rig in limbs)
        {
            rig.isKinematic = false;
            rig.detectCollisions = true;
        }
    }

    public void Die()
    {
        // Switch cameras
        Camera.main.enabled = false;
        kitchenCam.enabled = true;
        // Enable ragdoll physics
        EnableRagdoll();

        dead = true;
    }
    #endregion
}