using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ToothBrush : Interactable
{
    #region methods
    public override void Action()
    {
        // Play animation
        GetComponent<Animator>().SetTrigger("React");
        // Play audio
        AudioManager.Instance.PlayAudio("Brush Teeth");
        // Play bubble FX
        Camera.main.transform.Find("VFX").transform.Find("Bubble Effect").GetComponent<VisualEffect>().Play();

        // Mark as complete
        GameManager.Instance.taskManager.UpdateTaskCompletion("Brush Teeth");

        // Play grab animation
        Animator anim = PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>();
        if (!anim.GetBool("Notepad"))
            anim.SetTrigger("Grab");

        // Disable interaction
        CanInteract = false;
    }
    #endregion
}
