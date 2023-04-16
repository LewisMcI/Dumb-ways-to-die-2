using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class DialogueSystem : Interactable
{
    [Header("Needed")]
    [SerializeField]
    bool interactable;
    [SerializeField]
    AudioClip tickAudio;

    [Header("Optional")]
    [SerializeField]
    AudioClip introAudio;
    [SerializeField]
    AudioClip outroAudio;


    [Header("Dialogue")]

    [SerializeField]
    public List<Dialogue> dialogues = new List<Dialogue>();


    bool complete = true;

    AudioSource audioSource;
    DialogueManager dialogueManager = new DialogueManager();
    
    public override void Action()
    {
        if (!interactable)
            return;

        if (TriggerDialogue())
            interactable = false;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            throw new System.Exception("AudioSource not added to dialogue system");
    }
    public bool TriggerDialogue()
    {
        if (dialogueManager.CanPlay())
        {
            audioSource.volume = 0.1f;
            StartCoroutine(dialogueManager.StartDialogue(GameUI.Instance, dialogues.ToArray(), audioSource, tickAudio, introAudio, outroAudio));
            return true;
        }
        return false;
    }
}
