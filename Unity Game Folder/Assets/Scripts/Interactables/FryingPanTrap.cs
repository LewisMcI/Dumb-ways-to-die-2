using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPanTrap : MonoBehaviour
{
    #region fields
    private bool triggered;
    #endregion

    #region methods
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name);
        if (other.transform.name == "Character" && !triggered)
        {
            Trigger();
        }
    }

    private void Trigger()
    {
        triggered = true;
        GetComponent<Animator>().SetTrigger("Trigger");
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
