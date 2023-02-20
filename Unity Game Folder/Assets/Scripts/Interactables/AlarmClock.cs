using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : Interactable
{
    #region fields
    [SerializeField]
    private bool enable;
    #endregion

    #region methods
    private void Awake()
    {
        if (enable)
        {
            StartCoroutine(StartAlarm());
            StartCoroutine(StopAlarm());
        }
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

    private IEnumerator StartAlarm()
    {
        yield return new WaitForSeconds(0.25f);
        Alarm();
    }

    private IEnumerator StopAlarm()
    {
        yield return new WaitForSeconds(15f);
        Action();
    }
    #endregion
}