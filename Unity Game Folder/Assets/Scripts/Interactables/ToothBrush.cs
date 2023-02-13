using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ToothBrush : Interactable
{
    VisualEffect bubbleVFX;

    #region methods
    private void Awake()
    {
        bubbleVFX = GameObject.Find("Player Bubble Effect").GetComponent<VisualEffect>();
        if (!bubbleVFX)
            Debug.Log("Could not find player bubble effect");
    }
    public override void Action()
    {
        // Play animations, effects and audio
        GetComponent<Animator>().SetTrigger("React");
        AudioManager.Instance.PlayAudio("Brush Teeth");
        bubbleVFX.Play();

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
