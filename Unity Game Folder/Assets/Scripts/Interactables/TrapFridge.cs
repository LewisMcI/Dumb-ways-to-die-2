using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TrapFridge : Interactable
{
    #region methods
    public override void Action()
    {
        // Ignore fridge objects
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        transform.GetChild(1).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Cast ray to see if player will be hit
        RaycastHit hit;
        if (Physics.BoxCast(transform.GetChild(0).GetChild(0).transform.position, new Vector3(0.5f, 0.2f, 0.6f), Vector3.right, out hit, Quaternion.identity, 2f))
        {
            if (hit.transform.tag == "Player" || hit.transform.tag == "MainCamera")
                StartCoroutine(TriggerTrap());
            else
                GetComponent<Animator>().SetTrigger("Activate");
        }
        else
            GetComponent<Animator>().SetTrigger("Activate");
    }

    IEnumerator TriggerTrap()
    {
        // Disable controls
        GameManager.Instance.EnableControls = false;
        // Reset player velocity and animation
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("dirX", 0);
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("dirY", 0);
        float delay = 0.75f;
        PlayerController.Instance.Die(delay, true, SelectCam.fridgeCam);
        yield return new WaitForSeconds(delay);
        // Add backwards force
        PlayerController.Instance.AddRagdollForce(new Vector3(100, 10, 0));
        GetComponent<Animator>().SetTrigger("Activate");
        GetComponent<AudioSource>().Play();
    }
    #endregion
}
