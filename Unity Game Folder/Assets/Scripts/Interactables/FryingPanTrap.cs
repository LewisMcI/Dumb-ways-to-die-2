using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanTrap : MonoBehaviour
{
    #region fields
    private GameObject fryingPan;
    private LineRenderer laser;
    private bool triggered;
    #endregion

    #region methods
    private void Awake()
    {
        if (transform.GetChild(0).GetChild(0).name == "FryingPan")
        {
            fryingPan = transform.GetChild(0).GetChild(0).gameObject;
        }
        else
        {
            fryingPan = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        }
        laser = transform.GetChild(1).GetComponent<LineRenderer>();
        fryingPan.GetComponent<Collider>().enabled = false;
        transform.GetChild(0).GetChild(0).GetComponent<Interactable>().CanInteract = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Character" && !triggered)
        {
            TriggerPlayer();
        }
        else if (!triggered)
        {
            Trigger();
        }
    }

    private void Trigger()
    {
        triggered = true;
        GetComponent<Animator>().SetTrigger("Trigger");

        fryingPan.GetComponent<Collider>().enabled = true;
        fryingPan.GetComponent<Interactable>().CanInteract = true;
        laser.enabled = false;
    }

    private void TriggerPlayer()
    {
        try
        {
            GetComponent<AudioSource>().Play();
        }
        catch { }
        PlayerController.Instance.DisableDeathFromCollision(5.0f);
        Trigger();
        PlayerController.Instance.ThrowPlayerInRelativeDirection(50f, Direction.backwards, 2.0f, true);
    }
    #endregion
}
