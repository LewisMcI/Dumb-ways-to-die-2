using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBrush : Interactable
{
    #region methods
    public override void Action()
    {
        AudioManager.Instance.PlayAudio("Brush Teeth");
        CanInteract = false;

        GameManager.Instance.UpdateTaskCompletion("Brush Teeth");
        // Play grab animation
        Animator anim = PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>();
        if (!anim.GetBool("Notepad"))
            anim.SetTrigger("Grab");
    }
    #endregion
}
