using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanTrap : MonoBehaviour
{
    #region fields
    private bool triggered;
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
        Trigger();
        PlayerController.Instance.EnableRagdoll();
        PlayerController.Instance.AddRagdollForce(-transform.forward * 50f);
        GameManager.Instance.EnableControls = false;
        StartCoroutine(Recover());
    }

    private IEnumerator Recover()
    {
        yield return new WaitForSeconds(2.0f);
        PlayerController.Instance.ResetCharacter();
        GameManager.Instance.EnableControls = true;
    }
    #endregion
}
