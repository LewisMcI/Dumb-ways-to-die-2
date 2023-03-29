using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    backwards,
    forwards,
    up,
    down,
    right,
    left
}
public enum SelectCam
{
    toasterCam,
    fridgeCam,
    couchCam,
    bathroomCam,
    outsideCam,
    invalid = 100
}
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

    public Transform CameraTransform { get => playerCam.transform; }
    private float restartTimer = 5f;
    private Rigidbody[] limbs;
    private bool dead;

    [SerializeField]
    private ParticleSystem[] electricFX;

    private Rigidbody rig;
    private Animator anim;

    private bool canDieFromCollision = true;

    public static PlayerController Instance;
    #endregion

    #region properties
    public bool Dead
    {
        get { return dead; }
        set { dead = value; }
    }

    public ParticleSystem[] ElectricFX
    {
        get { return electricFX; }
    }
    #endregion

    #region methods
    #region UnityCalls
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
        if (!dead)
        {
            // Controls
            if (GameManager.Instance.EnableControls)
            {
                Jump();
                Crouch();
            }
            else if (isCrouching || isJumping)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Crouching", false);
            }

            // Book
            if (Input.GetButtonDown("Pause Game") && !dead && GameManager.Instance.EnableControls || Input.GetButtonDown("Pause Game") && GameManager.Instance.IsPaused)
            {
                GameManager.Instance.PauseGame();
            }

            // Notepad
            if (Input.GetButtonDown("Notepad") && GameManager.Instance.EnableControls)
            {
                anim.SetBool("Notepad", !anim.GetBool("Notepad"));
            }
        }
        else if (dead)
        {
            if (restartTimer <= 0.0f)
                GameManager.Instance.Restart();
            else
                restartTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (GameManager.Instance.EnableControls)
            {
                Move();
            }
            else
            {
                rig.velocity = new Vector3(0.0f, rig.velocity.y, 0.0f);
                anim.SetFloat("dirX", 0.0f);
                anim.SetFloat("dirY", 0.0f);
            }
        }
    }
    #endregion
    #region Movement
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
            Camera.main.GetComponent<CameraController>().FollowHeadTime = 5.0f;
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
                Camera.main.GetComponent<CameraController>().FollowHeadTime = 15.0f;
                // Reset
                isJumping = false;
            }
        }
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            Camera.main.GetComponent<CameraController>().FollowHeadTime = 0.0f;
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
            StartCoroutine(ResetCrouch());
            // Reset collider
            transform.GetChild(0).GetComponent<CapsuleCollider>().center = new Vector3(0.0f, 0.9f, 0.0f);
            transform.GetChild(0).GetComponent<CapsuleCollider>().height = 1.8f;

            // Trigger standing animation
            anim.SetBool("Crouching", false);

            isCrouching = false;
        }
    }

    private IEnumerator ResetCrouch()
    {
        yield return new WaitForSeconds(0.25f);
        Camera.main.GetComponent<CameraController>().FollowHeadTime = 15.0f;
    }

    #endregion
    #region Ragdoll
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

    public void ResetCharacterAfterRagdoll()
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
        }
        playerCam = Camera.main;
        InteractionSystem.Instance.PickupTransform = Camera.main.transform.GetChild(0);
        DisableRagdoll();
        StartCoroutine(WaitBeforeFindingNotepad());
    }
    IEnumerator WaitBeforeFindingNotepad()
    {
        yield return new WaitForSeconds(.2f);
        notepad.SetActive(true);
        GameManager.Instance.taskManager.FindNotepadText();
    }

    #endregion
    #region Functions

    /// <summary>
    /// Player dies and the game restarts after the time given.
    /// </summary>
    /// <param name="delay">Delay before the game restarts.</param>
    /// <param name="ragdoll">Should the player ragdoll?</param>
    /// <param name="camera">what camera will we switch to.</param>
    /// <param name="force">Force if player is thrown.</param>
    public void Die(float delay, bool ragdoll = true, SelectCam? camera = null, Vector3? force = null)
    {
        if (camera != null)
        {
            GameManager.Instance.EnableCamera = false;
            GameManager.Instance.EnableControls = false;
            EnableNewCamera(camera);
        }
        dead = true;
        Debug.Log(dead);
        if (force == null)
            force = Vector3.zero;
        StartCoroutine(KillPlayer(delay, force.Value, ragdoll));
    }
    /// <summary>
    /// Throws player in direction relative to player.
    /// </summary>
    /// <param name="force">Force to throw player.</param>
    /// <param name="direction">Direction to throw player.</param>
    /// <param name="timeTillEnds">Time till either the game ends, or till the player recovers.</param>
    /// <param name="recover">Should the player recover after being thrown?</param>
    public void ThrowPlayerInRelativeDirection(float force, Direction direction, float timeTillEnds, bool recover = false)
    {
        EnableRagdoll();
        switch(direction)
        {
            case Direction.backwards:
                AddRagdollForce(-transform.forward * force);
                break;
            case Direction.forwards:
                AddRagdollForce(transform.forward * force);
                break;
            case Direction.up:
                AddRagdollForce(transform.up * force);
                break;
            case Direction.down:
                AddRagdollForce(-transform.up * force);
                break;
            case Direction.right:
                AddRagdollForce(transform.right * force);
                break;
            case Direction.left:
                AddRagdollForce(-transform.right * force);
                break;
        }
           
        GameManager.Instance.EnableControls = false;
        CameraController camController = Camera.main.GetComponent<CameraController>();
        camController.FreezeRotation = true;
        float followHeadTime = camController.FollowHeadTime;
        camController.FollowHeadTime = 0.0f;
        if (recover)
            StartCoroutine(Recover(timeTillEnds, followHeadTime));
        else
        {
            Die(timeTillEnds);
        }
    }
    /// <summary>
    ///  Throws player with direction and force given.
    /// </summary>
    /// <param name="forceInDirection">Power and Direction to Throw Player.</param>
    /// <param name="delay">Delay before player is thrown.</param>
    /// <param name="newCam">Camera to switch to.</param>
    /// <param name="recover">Should the player recover after being thrown?</param>
    /// <param name="timeTillRecover">How long should it take before the player recovers.</param>
    public void ThrowPlayerInDirection(Vector3 forceInDirection, float delay = 0, SelectCam newCam = SelectCam.invalid, bool recover = false, float timeTillRecover = 0)
    {
        // Disable Camera movement
        CameraController camController = Camera.main.GetComponent<CameraController>();
        GameManager.Instance.EnableControls = false;
        camController.FreezeRotation = true;

        float followHeadTime = camController.FollowHeadTime;
        camController.FollowHeadTime = 0.0f;

        // Prevent additional death from collision
        DisableDeathFromCollision(delay * 2);
        
        EnableNewCamera(newCam);

        if (recover)
        {
            // Ragdoll
            EnableRagdoll();
            AddRagdollForce(forceInDirection);

            // Recover
            StartCoroutine(Recover(timeTillRecover, followHeadTime));
        }
        else
        {
            // Die after delay
            Die(delay, true, null, forceInDirection);
        }
    }
    #endregion
    #region Misc
    private IEnumerator Recover(float timeTillEnds, float followHeadTime)
    {
        yield return new WaitForSeconds(timeTillEnds);
        GameManager.Instance.EnableControls = true;
        ResetCharacterAfterRagdoll();
        Camera.main.GetComponent<CameraController>().FreezeRotation = false;
        Camera.main.GetComponent<CameraController>().FollowHeadTime = followHeadTime;
    }
    IEnumerator KillPlayer(float delay, Vector3 force, bool ragdoll = false)
    {
        yield return new WaitForSeconds(delay);
        // Enable ragdoll physics
        if (ragdoll)
        {
            EnableRagdoll();
            AddRagdollForce(force);
        }
    }
    public void EnableNewCamera(SelectCam? camera)
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
            case SelectCam.invalid:
                break;
        }
    }
    private IEnumerator OpenNotepadAfterAwake()
    {
        yield return new WaitForSeconds(3.3f);
        notepad.SetActive(true);

        GameManager.Instance.taskManager.FindNotepadText();

        anim.SetBool("Notepad", !anim.GetBool("Notepad"));
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
    #endregion
    #region Collision

    private void OnCollisionEnter(Collision collision)
    {
        if (canDieFromCollision && collision.relativeVelocity.magnitude > 20)
        {
            Debug.Log("Player ragdolled by speed: " + collision.relativeVelocity.magnitude);
            Camera.main.GetComponent<CameraController>().FollowHeadTime = 0.0f;
            ThrowPlayerInRelativeDirection(50f, Direction.backwards, 2.0f);
        }
    }

    public void DisableDeathFromCollision(float time = 0)
    {
        canDieFromCollision = false;
        if (time == 0)
        {
            Debug.Log("Death from Collision disabled until next respawn or next edit.");
            return;
        }    
        StartCoroutine(ReEnableDeathFromCollision(time));
    }

    IEnumerator  ReEnableDeathFromCollision(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canDieFromCollision = true;
    }
    #endregion
    #endregion
}