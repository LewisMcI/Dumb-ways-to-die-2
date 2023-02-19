using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : Interactable
{
    [System.Serializable]
    public class Dialogue
    {
        public string text;
        public float speed = 0.1f;
        public float waitBetweenLines = 0.2f;
    }
    private TextMeshProUGUI dialogueText;

    bool complete = false;
    bool interactable = true;
    public float waitAfterComplete = 2.0f;

    [SerializeField]
    public List<Dialogue> dialogues = new List<Dialogue>();

    public override void Action()
    {
        if (interactable)
        {
            GetComponent<AudioSource>().Play();
            StopAllCoroutines();
            StartCoroutine(StartDialogue());
            GameUI.Instance.DialogueText.transform.parent.gameObject.SetActive(true);
        }
    }

    IEnumerator StartDialogue()
    {
        foreach(Dialogue dialogue in dialogues)
        {
            complete = false;
            StartCoroutine(UpdateDialogue(dialogue));
            while (!complete)
                yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(dialogue.waitBetweenLines);
        }
        yield return new WaitForSeconds(waitAfterComplete);
        GameUI.Instance.DialogueText.transform.parent.gameObject.SetActive(false);
        GameUI.Instance.DialogueText.text = "";
    }

    IEnumerator UpdateDialogue(Dialogue dialogue)
    {
        GameUI.Instance.DialogueText.text = "";
        foreach (char character in dialogue.text.ToCharArray())
        {
            yield return new WaitForFixedUpdate();
            GameUI.Instance.DialogueText.text += character;
            yield return null;
        }
        complete = true;
    }
}
