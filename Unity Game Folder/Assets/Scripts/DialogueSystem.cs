using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip introAudio;
    [SerializeField]
    AudioClip audio;
    [SerializeField]
    AudioClip outroAudio;


    [System.Serializable]
    public class Dialogue
    {
        public string text;
        public float speed = 5.0f;
        public float holdAfterFinished = 3.0f;
    }

    [SerializeField]
    public List<Dialogue> dialogues = new List<Dialogue>();

    bool complete = true;

    private void Awake()
    {
        if (!audioSource)
            throw new System.Exception("AudioSource not added to dialogue system");
    }
    public void TriggerDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(StartDialogue());
        GameUI.Instance.DialogueText.transform.parent.gameObject.SetActive(true);
    }
    void PlayMurmur()
    {
        if (audio)
        {
            audioSource.pitch = Random.Range(0.90f, 1.10f);
            audioSource.PlayOneShot(audio);
        }
    }
    IEnumerator StartDialogue()
    {
        audioSource.PlayOneShot(introAudio);
        yield return new WaitForSeconds(1.0f);
        foreach(Dialogue dialogue in dialogues)
        {
            StartCoroutine(UpdateDialogue(dialogue));
            if (!complete)
                yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(dialogue.holdAfterFinished);
        }
        yield return new WaitForSeconds(2.0f);
        GameUI.Instance.DialogueText.transform.parent.gameObject.SetActive(false);
        GameUI.Instance.DialogueText.text = "";


        audioSource.Stop();
        audioSource.PlayOneShot(outroAudio);
    }

    IEnumerator UpdateDialogue(Dialogue dialogue)
    {
        GameUI gameUI = GameUI.Instance;
        gameUI.DialogueText.text = "";
        float speed = 1 / dialogue.speed;
        for (float i = 0; i < speed; i+=Time.deltaTime)
        {
            int value = (int)Mathf.Lerp(0, dialogue.text.Length - 1, i / speed);
            if (value > gameUI.DialogueText.text.Length)
            {
                gameUI.DialogueText.text = dialogue.text.Substring(0, value);

                PlayMurmur();
            }
            yield return new WaitForFixedUpdate();
        }
        gameUI.DialogueText.text = dialogue.text;
    }
}
