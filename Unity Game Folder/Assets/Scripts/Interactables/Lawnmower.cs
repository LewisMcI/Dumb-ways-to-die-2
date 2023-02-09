using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawnmower : Interactable
{
    #region fields
    
    public Transform playerPosition;
    public BoxCollider triggerCollider;
    private int destroyedCount;

    public Camera outsideCam;
    bool active = false;
    #endregion

    #region methods
    public override void Action()
    {
        // Enable new camera
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().EnableNewCamera(PlayerController.SelectCam.outsideCam);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<InteractionSystem>().enabled = false;
        player.transform.position = playerPosition.position;
        player.transform.rotation = playerPosition.rotation;
        transform.parent = player.transform;
        player.GetComponent<Rigidbody>().angularDrag = 1.0f;

        triggerCollider.enabled = true;

        GameObject.FindGameObjectWithTag("Player").AddComponent<TopdownPlayerController>();
        active = true;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grass")
        {
            Destroy(other.gameObject);
            destroyedCount++;
            Debug.Log(destroyedCount);
        }
        if (other.tag == "Bomb")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<TopdownPlayerController>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = true;
            Debug.Log("HUGE EXPLOSION KPOW BODY FLIES AWAY MINECRAFT DEATH SOUND (THERE IS AN ERROR BELOW THIS IT IS OKAY)");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Die(PlayerController.SelectCam.outsideCam, 0.1f);
        }
    }
}
