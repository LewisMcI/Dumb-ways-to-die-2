using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Lawnmower : Interactable
{
    #region fields

    [SerializeField]
    private Transform grassMaster;

    public Transform playerPosition;
    public Camera associatedCam;

    private float initAngularDrag;

    public VisualEffect explosionVFX;
    public AudioSource explosionSFX;

    public AudioSource lawnmowerStartup;
    public AudioClip lawnMowerContinuous;
    public AudioClip lawnmowerEnd;

    public GameObject grassBlock;
    [SerializeField]
    private int minSpawnGrass = 45;
    [SerializeField]
    private int maxSpawnGrass = 100;
    private int noToSpawnGrass;
    private int noOfGrass;

    [SerializeField][Range(0.0f, 1.0f)]
    private float percentDestroyedToWin = 0.75f;

    private int noToWin;
    bool active = false;
    [SerializeField]
    private List<BoxCollider> walls;

    private Vector3 initPosition;
    private Quaternion initRotation;
    #endregion

    #region methods
    private void Awake()
    {
        noToSpawnGrass = Random.Range(minSpawnGrass, maxSpawnGrass);
        noOfGrass = grassMaster.childCount;
        noToWin = (int)(noOfGrass * (1-percentDestroyedToWin));

        initRotation = associatedCam.transform.rotation;
        initPosition = associatedCam.transform.position;
    }
    public override void Action()
    {
        active = true;
        lawnmowerStartup.Play();
        GameObject player;
        // Find player
        try
        {
            player = PlayerController.Instance.gameObject;
        }
        catch
        {
            Debug.Log("Cannot find PlayerController instance");
            throw new System.Exception("Could not find PlayerController Instance");
        }

        // Disable player controller.
        PlayerController.Instance.enabled = false;
        StopCoroutine(moveCameraToPlayer(player));
        StartCoroutine(moveCameraFromPlayer(player));
        // Enable new camera.
        PlayerController.Instance.EnableNewCamera(PlayerController.SelectCam.outsideCam);


        // Move player to position
        player.transform.position = playerPosition.position;
        player.transform.rotation = playerPosition.rotation;

        // Add lawnmower as child to player to control movement.
        transform.parent = player.transform;

        Rigidbody playerRB = player.GetComponent<Rigidbody>();
        // Get old angular drag.
        initAngularDrag = playerRB.angularDrag;
        // Set new angular drag on player.
        playerRB.angularDrag = 1.0f;

        // Play player lawnmower animation
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Lawnmower", true);

        // Enable Invisible Walls
        foreach (var wall in walls)
        {
            wall.enabled = true;
        }
    }

    private void Update()
    {
        if (active)
        {
            Animator anim = PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>();
            // Get axis
            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            // If moving diagonally normalise vector so that speed remains the same
            if (dir.magnitude > 1.0f)
                dir.Normalize();
            // Set animation parameters
            anim.SetFloat("dirX", dir.x);
            anim.SetFloat("dirY", dir.y);

            if (Input.GetKey(KeyCode.Escape) && !PlayerController.Instance.Dead)
            {
                ExitLawnmower();
            }

            if (!lawnmowerStartup.isPlaying)
            {
                lawnmowerStartup.PlayOneShot(lawnMowerContinuous);
            }
        }
    }
    #endregion
    void ExitLawnmower()
    {
        lawnmowerStartup.Stop();
        lawnmowerStartup.PlayOneShot(lawnmowerEnd);
        active = false;
        foreach (var wall in walls)
        {
            wall.enabled = false;
        }
        GameObject player;
        // Find player
        try
        {
            player = PlayerController.Instance.gameObject;
        }
        catch
        {
            Debug.Log("Cannot find PlayerController instance");
            throw new System.Exception("Could not find PlayerController Instance");
        }
        StopCoroutine(moveCameraFromPlayer(player));
        StartCoroutine(moveCameraToPlayer(player));

        // Unchild lawnmower.
        transform.parent = null;
        // Set new angular drag on player.
        player.GetComponent<Rigidbody>().angularDrag = initAngularDrag;
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Lawnmower", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        // If hit grass
        if (other.name.Contains("SM_Env_Grass_Patch"))
        {
            // Destroy grass
            Destroy(other.gameObject);
            noOfGrass--;
            noToSpawnGrass--;
            Debug.Log(noOfGrass);
            if (noToSpawnGrass == 0)
            {
                noToSpawnGrass = Random.Range(minSpawnGrass, maxSpawnGrass);
                Debug.Log("Spawned, next is: " + noToSpawnGrass);
                Rigidbody newRb = Instantiate(grassBlock, transform.position + (-transform.forward * 2) + new Vector3(0, -1, 0), Quaternion.identity).GetComponent<Rigidbody>();
                newRb.AddForce(-transform.forward * 2, ForceMode.Impulse);

            }
            else if (grassMaster.childCount <= noToWin)
            {
                Debug.Log("Complete");
                GameManager.Instance.taskManager.UpdateTaskCompletion("Mow Lawn");
                ExitLawnmower();
                Destroy(this);
            }

        }
        if (other.name == "Bomb"|| other.name == "Block of Grass(Clone)")
        {
            Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            if (!otherRb)
                otherRb = other.gameObject.AddComponent<Rigidbody>();
            otherRb.AddForce(-transform.forward * 400, ForceMode.Force);
            StartCoroutine(KillPlayer(other.gameObject));

        }
    }

    IEnumerator KillPlayer(GameObject other)
    {
        yield return new WaitForSeconds(0.5f);
        explosionSFX.Play();
        Destroy(other.gameObject);
        explosionVFX.Play();
        // Disable player controller.
        PlayerController.Instance.GetComponent<TopdownPlayerController>().enabled = false;
        lawnmowerStartup.Stop();
        lawnmowerStartup.PlayOneShot(lawnmowerEnd);
        GameObject player;
        // Find player
        try
        {
            player = PlayerController.Instance.gameObject;
        }
        catch
        {
            Debug.Log("Cannot find PlayerController instance");
            throw new System.Exception("Could not find PlayerController Instance");
        }
        // Remove Topdown Controller
        Destroy(player.GetComponent<TopdownPlayerController>());
        // Unchild lawnmower.
        transform.parent = null;
        // Enable player controller.
        PlayerController.Instance.enabled = true;
        float delay = 0.1f;
        PlayerController.Instance.Die(delay);
        yield return new WaitForSeconds(delay);
        // Add backwards force
        PlayerController.Instance.AddRagdollForce(new Vector3(100, 200, 0));
    }

    IEnumerator moveCameraFromPlayer(GameObject player)
    {
        Vector3 startPos = PlayerController.Instance.CameraTransform.position;

        Quaternion startQuat = PlayerController.Instance.CameraTransform.rotation;

        for (float i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(1.0f / 50);
            associatedCam.transform.position = Vector3.Lerp(startPos, initPosition, i / 50);
            associatedCam.transform.rotation = Quaternion.Lerp(startQuat, initRotation, i / 50);
        }

        player.AddComponent<TopdownPlayerController>();
    }

    IEnumerator moveCameraToPlayer(GameObject player)
    {
        // Remove Topdown Controller
        Destroy(player.GetComponent<TopdownPlayerController>());
        Vector3 startPos = associatedCam.transform.position;
        Vector3 endPos = PlayerController.Instance.CameraTransform.position;

        Quaternion startQuat = associatedCam.transform.rotation;
        Quaternion endQuat = PlayerController.Instance.CameraTransform.rotation;

        for (float i = 0; i < 50; i++)
        {
            associatedCam.transform.position = Vector3.Lerp(startPos, endPos, i / 50);
            associatedCam.transform.rotation = Quaternion.Lerp(startQuat, endQuat, i / 50);

            yield return new WaitForSeconds(1.0f / 50);
        }
        // ReEnable player camera.
        PlayerController.Instance.ReEnablePlayer();
        // Enable player controller.
        PlayerController.Instance.enabled = true;

        associatedCam.transform.position = startPos;
        associatedCam.transform.rotation = startQuat;
        // Remove Topdown Controller
        Destroy(player.GetComponent<TopdownPlayerController>());
    }


}
