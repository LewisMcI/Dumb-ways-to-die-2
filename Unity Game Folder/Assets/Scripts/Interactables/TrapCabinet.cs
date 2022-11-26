using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCabinet : Interactable
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

    private void Update()
    {
        if (text != "Cut" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Scissors")
            text = "Cut";
        else if (text != "Open")
            text = "Open";
    }

    public override void Action()
    {
        // Cut rope
        if (InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Scissors" && !cut)
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
            GetComponent<Collider>().enabled = false;
        }
    }
    #endregion
}
