using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : Interactable
{
    [Header("Needed")]
    [SerializeField]
    bool interactable = false;
    [SerializeField]
    AudioSource audioSource;
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

    DialogueManager dialogueManager = new DialogueManager();

    public override void Action()
    {
        if (!interactable)
            return;
        Debug.Log("Interacted");
        TriggerDialogue();
        Destroy(this);
    }

    private void Awake()
    {
        if (!audioSource)
            throw new System.Exception("AudioSource not added to dialogue system");
    }
    public void TriggerDialogue()
    {
        if (dialogueManager.CanPlay())
        { 
            Debug.Log("CanPlay");
            StartCoroutine(dialogueManager.StartDialogue(GameUI.Instance, dialogues.ToArray(), audioSource, tickAudio, introAudio, outroAudio));
        }
    }
}
