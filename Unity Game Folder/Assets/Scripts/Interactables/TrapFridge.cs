using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class TrapFridge : Interactable
{
    #region fields
    [SerializeField]
    private Rigidbody tableRig;
    #endregion

    #region methods
    public override void Action()
    {
        // Ignore fridge objects
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        transform.GetChild(1).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Cast ray to see if player will be hit
        RaycastHit hit;
        if (Physics.BoxCast(transform.GetChild(0).GetChild(0).transform.position, new Vector3(0.5f, 0.2f, 0.6f), Vector3.right, out hit, Quaternion.identity, 3f))
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
        float delay = 0.75f;
        GameManager.Instance.Player.GetComponent<PlayerController>().Die(PlayerController.SelectCam.fridgeCam, delay);
        yield return new WaitForSeconds(delay);
        tableRig.isKinematic = false;
        GetComponent<Animator>().SetTrigger("Activate");
        GetComponent<AudioSource>().Play();
    }
    #endregion
}
