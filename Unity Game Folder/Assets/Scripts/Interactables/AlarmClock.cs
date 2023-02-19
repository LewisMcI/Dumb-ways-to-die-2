using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : Interactable
{
    #region methods
    private void Start()
    {
        Alarm();
    }

    public override void Action()
    {
        // Stop audio
        GetComponent<AudioSource>().Stop();
        // Stop animation
        GetComponent<Animator>().SetBool("Alarm", false);
        CanInteract = false;
    }

    public void Alarm()
    {
        // Play audio
        GetComponent<AudioSource>().Play();
        // Play animation
        GetComponent<Animator>().SetBool("Alarm", true);
    }
    #endregion
}
