using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanTrap : MonoBehaviour
{
    #region fields
    private bool triggered;

    [SerializeField]
    AudioSource doingggSFX;
    #endregion

    #region methods
    private void Awake()
    {
        transform.GetChild(0).GetChild(0).GetComponent<Collider>().enabled = false;
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

        transform.GetChild(0).GetChild(0).GetComponent<Collider>().enabled = true;
        transform.GetChild(0).GetChild(0).GetComponent<Interactable>().CanInteract = true;
        transform.GetChild(1).GetComponent<LineRenderer>().enabled = false;
    }

    private void TriggerPlayer()
    {
        try
        {
            doingggSFX.Play();
        }
        catch { }
        PlayerController.Instance.DisableDeathFromCollision(5.0f);
        Trigger();
        PlayerController.Instance.ThrowPlayerBackwards(50f, 2.0f, true);
    }
    #endregion
}
