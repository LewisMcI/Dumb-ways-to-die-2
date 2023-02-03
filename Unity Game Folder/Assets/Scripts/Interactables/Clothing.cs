using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothing : Interactable
{
    #region fields
    private bool collected;
    #endregion

    #region properties
    public bool Collected
    { 
        get { return collected; }
    }
    #endregion

    #region methods
    public override void Action()
    {
        // Mark as collected
        collected = true;
        // Disable object
        gameObject.SetActive(false);
        transform.parent.GetComponent<Clothes>().Check();
        AudioManager.Instance.PlayAudio("Cloth");
        // Play grab animation
        Animator anim = PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>();
        if (!anim.GetBool("Notepad"))
            anim.SetTrigger("Grab");
    }
    #endregion
}
