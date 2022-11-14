using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCabinet : MonoBehaviour
{
    #region fields
    private bool cut;

    private Animator anim;
    #endregion

    #region methods
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact(bool scissors)
    {
        // Cut rope
        if (scissors && !cut)
        {
            anim.SetTrigger("Cut");
            cut = true;
        }
        else
        {
            // Open and shoot if not cut
            if (cut)
                anim.SetTrigger("Open");
            else
                anim.SetTrigger("Shoot");
            // Reset tag
            transform.tag = "Untagged";
        }
    }
    #endregion
}
