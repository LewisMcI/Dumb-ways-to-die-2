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
    #endregion

    #region methods
    public override void Action()
    {
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

        player.AddComponent<TopdownPlayerController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            ExitLawnmower();
    }
    #endregion
    void ExitLawnmower()
    {
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
            if (grassMaster.childCount == 1)
            {
                Debug.Log("Complete");
                GameManager.Instance.taskManager.UpdateTaskCompletion("Mow Lawn");
                ExitLawnmower();
            }
       /*     if (destroyedCount >= 100)
                ExitLawnmower();*/
        }
        if (other.name == "Bomb")
        {
            explosionSFX.Play();
            Destroy(other.gameObject);
            explosionVFX.Play();

            // Disable player controller.
            PlayerController.Instance.GetComponent<TopdownPlayerController>().enabled = false;
            StartCoroutine(KillPlayer());
        }
    }

    IEnumerator KillPlayer()
    {
        yield return new WaitForSeconds(1.0f);
        ExitLawnmower();
        Destroy(this);
    }
}
