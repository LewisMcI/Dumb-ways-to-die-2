using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : Interactable
{
    #region fields
    [SerializeField]
    private bool ring;
    #endregion

    #region methods
    private void Awake()
    {
        if (ring)
            StartCoroutine(StartAlarm());
        else
            CanInteract = false;
    }

    public override void Action()
    {
        GetComponent<DialogueSystem>().TriggerDialogue();
        // Stop audio
        GetComponent<AudioSource>().Stop();
        // Stop animation
        GetComponent<Animator>().SetBool("Ring", false);
        CanInteract = false;
    }

    public void Ring()
    {
        // Play audio
        GetComponent<AudioSource>().Play();
        // Play animation
        GetComponent<Animator>().SetBool("Ring", true);
    }

    private IEnumerator StartAlarm()
    {
        yield return new WaitForSeconds(0.25f);
        Ring();
    }
    #endregion
}