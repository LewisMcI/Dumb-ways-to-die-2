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

    bool active = false;
    [SerializeField]
    private List<BoxCollider> walls;
    #endregion

    #region methods
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

        // Enable new camera.
        PlayerController.Instance.EnableNewCamera(PlayerController.SelectCam.outsideCam);
        // Disable player controller.
        PlayerController.Instance.enabled = false;

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

        // Enable Invisible Walls
        foreach (var wall in walls)
        {
            wall.enabled = true;
        }
        player.AddComponent<TopdownPlayerController>();
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetKey(KeyCode.Escape))
                ExitLawnmower();

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
        // Remove Topdown Controller
        Destroy(player.GetComponent<TopdownPlayerController>());
        // ReEnable player camera.
        PlayerController.Instance.ReEnablePlayer();
        // Unchild lawnmower.
        transform.parent = null;
        // Set new angular drag on player.
        player.GetComponent<Rigidbody>().angularDrag = initAngularDrag;
        // Enable player controller.
        PlayerController.Instance.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        // If hit grass
        if (other.name == "Grass Piece")
        {
            // Destroy grass
            Destroy(other.gameObject);
            if (grassMaster.childCount % 50 == 0)
            {
                Instantiate(grassBlock, transform.position + (-transform.forward * 2) + new Vector3(0, -1, 0), Quaternion.identity);
            }
            else if (grassMaster.childCount == 1)
            {
                Debug.Log("Complete");
                GameManager.Instance.taskManager.UpdateTaskCompletion("Mow Lawn");
                ExitLawnmower();
                Destroy(this);
            }

        }
        if (other.name == "Bomb"|| other.name == "Block of Grass(Clone)")
        {
            StartCoroutine(KillPlayer());
            explosionSFX.Play();
            Destroy(other.gameObject);
            explosionVFX.Play();

        }
    }

    IEnumerator KillPlayer()
    {
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
}
