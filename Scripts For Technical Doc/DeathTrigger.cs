using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private bool triggerKills = false;

    private void Awake()
    {
        EventManager.onClicked += ActivateCabinetTrap;

        anim = GetComponent<Animator>();
    }


    private Animator anim;

    private void ActivateCabinetTrap()
    {
        Debug.Log("Activate trap");
        // Pivot Object
        EventManager.onClicked -= ActivateCabinetTrap;
    }
}
