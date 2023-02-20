using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject character;

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

    [SerializeField]
    private Camera playerCam, toasterCam, fridgeCam, couchCam, bathroomCam, outsideCam;

    private float restartTimer = 5f;
    private bool dead;
    private bool canMove;

    private Rigidbody rig;
    private Rigidbody[] limbs;
    private Animator anim;

    public static PlayerController Instance;
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
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        rig = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        limbs =  transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
        StartCoroutine(OpenNotepadAfterAwake());
    }

    private void Update()
    {
        if (canMove)
        {
            // Controls
            if (GameManager.Instance.EnableControls)
            {
                Jump();
                Crouch();
            }

            // Book
            if (Input.GetButtonDown("Pause Game") && !dead)
            {
                GameManager.Instance.PauseGame();
            }

            // Notepad
            if (Input.GetButtonDown("Notepad"))
            {
                anim.SetBool("Notepad", !anim.GetBool("Notepad"));
            }

        }

        // Death
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

    public void EnableRagdoll()
    {
        GameManager.Instance.EnableControls = false;
        anim.enabled = false;
        foreach (Rigidbody rig in limbs)
        {
            rig.isKinematic = false;
            rig.detectCollisions = true;
        }
    }

    public void DisableRagdoll()
    {
        foreach (Rigidbody rig in limbs)
        {
            rig.isKinematic = true;
            rig.detectCollisions = false;
        }
    }

    public void AddRagdollForce(Vector3 force)
    {
        foreach (Rigidbody rig in limbs)
        {
            if (rig.transform.name == "bip Pelvis")
                rig.velocity = force;
        }
    }

    public void EnableNewCamera(SelectCam camera)
    {
        GameManager.Instance.EnableCamera = false;
        // Switch cameras
        Camera currentCam = Camera.main;
        currentCam.tag = "Untagged";
        currentCam.enabled = false;
        switch (camera)
        {
            case SelectCam.toasterCam:
                toasterCam.enabled = true;
                toasterCam.tag = "MainCamera";
                break;
            case SelectCam.fridgeCam:
                fridgeCam.enabled = true;
                fridgeCam.tag = "MainCamera";
                break;
            case SelectCam.couchCam:
                couchCam.enabled = true;
                couchCam.tag = "MainCamera";
                break;
            case SelectCam.bathroomCam:
                bathroomCam.enabled = true;
                bathroomCam.tag = "MainCamera";
                break;
            case SelectCam.outsideCam:
                outsideCam.enabled = true;
                outsideCam.tag = "MainCamera";
                break;
        }
    }

    public void ReEnablePlayer()
    {
        GameManager.Instance.EnableCamera = true;
        Camera currentCam = Camera.main;
        currentCam.tag = "Untagged";
        currentCam.enabled = false;

        playerCam.enabled = true;
        playerCam.tag = "MainCamera";
    }

    public void Die(SelectCam camera, float delay)
    {
        GameManager.Instance.EnableCamera = false;
        GameManager.Instance.EnableControls = false;
        EnableNewCamera(camera);
        dead = true;
        Debug.Log(dead);

        StartCoroutine(KillPlayer(delay));
    }
    public void Die(float delay)
    {
        GameManager.Instance.EnableControls = false;
        dead = true;
        Debug.Log(dead);

        StartCoroutine(KillPlayer(delay));
    }

    IEnumerator KillPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Enable ragdoll physics
        EnableRagdoll();
    }

    public void ResetCharacter()
    {
        // Set position
        transform.position = new Vector3(transform.GetChild(0).GetChild(0).GetChild(0).position.x, 1.46f, transform.GetChild(0).GetChild(0).GetChild(0).position.z);
        // Destroy current active character
        Destroy(transform.GetChild(0).gameObject);
        // Spawn new character
        GameObject newCharacter = Instantiate(character);
        newCharacter.transform.parent = transform;
        newCharacter.transform.localPosition = new Vector3(0.0f, -0.993f, 0.0f);
        newCharacter.transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        newCharacter.name = "Character";

        // Reset variables
        groundCheck = newCharacter.transform.GetChild(2);
        anim = newCharacter.GetComponent<Animator>();
        anim.SetBool("WakeUp", false);
        limbs = newCharacter.transform.GetChild(0).GetComponentsInChildren<Rigidbody>();
        foreach (Transform child in newCharacter.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "Notepad")
                notepad = child.gameObject;
            if (child.name == "PAUSETEXT")
                GameUI.Instance.pauseText = child.gameObject;
        }
        playerCam = Camera.main;
        InteractionSystem.Instance.PickupTransform = Camera.main.transform.GetChild(0);
        DisableRagdoll();
    }

    private IEnumerator OpenNotepadAfterAwake()
    {
        yield return new WaitForSeconds(3.3f);
        notepad.SetActive(true);

        GameManager.Instance.taskManager.FindNotepadText();

        anim.SetBool("Notepad", !anim.GetBool("Notepad"));
        canMove = true;
    }

    public enum SelectCam
    {
        toasterCam,
        fridgeCam,
        couchCam,
        bathroomCam,
        outsideCam
    }
    #endregion
}