using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayMusic : Interactable
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void Action()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
        else 
            audioSource.Stop();
    }
}
