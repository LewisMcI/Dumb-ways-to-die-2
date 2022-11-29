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
        RaycastHit hit;
        // Cast ray to see if player will be hit
        //if (Physics.Raycast(transform.GetChild(0).GetChild(0).transform.position, Vector3.right, out hit, 2f))
        //if (Physics.BoxCast(transform.GetChild(0).GetChild(0).transform.position, Vector3.right, out hit, 2f))
        if (Physics.BoxCast(transform.GetChild(0).GetChild(0).transform.position, new Vector3(0.5f, 0.2f, 0.6f), Vector3.right, out hit, Quaternion.identity, 3f))
        {
            Debug.Log(hit.transform.tag);
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Player" || hit.transform.tag == "MainCamera")
                StartCoroutine(TriggerTrap());
            else
                GetComponent<Animator>().SetTrigger("Activate");
        }
        else
        {
            Debug.Log("asd");
            GetComponent<Animator>().SetTrigger("Activate");
        }
    }

    IEnumerator TriggerTrap()
    {
        float delay = 0.75f;
        GameManager.Instance.Player.GetComponent<PlayerController>().Die(PlayerController.SelectCam.fridgeCam, delay);
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().SetTrigger("Activate");
    }
    #endregion
}
