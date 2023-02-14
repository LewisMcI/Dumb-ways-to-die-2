using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Interactable
{
    #region methods
    public override void Action()
    {
        // Play SFX
        AudioManager.Instance.PlayAudio("Eat");
        // Play eat FX
        Camera.main.transform.Find("VFX").transform.Find("Eating Effect").GetComponent<ParticleSystem>().Play();

        CanInteract = false;
        transform.GetComponent<Renderer>().enabled = false;

        GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");
        // Play grab animation
        Animator anim = PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>();
        if (!anim.GetBool("Notepad"))
            anim.SetTrigger("Grab");
    }
    #endregion
}
