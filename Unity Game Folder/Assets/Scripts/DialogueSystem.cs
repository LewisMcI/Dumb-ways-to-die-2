using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
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
    public void TriggerDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(StartDialogue());
        GameUI.Instance.DialogueText.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator StartDialogue()
    {
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
    }

    IEnumerator UpdateDialogue(Dialogue dialogue)
    {
        GameUI.Instance.DialogueText.text = "";

        float speed = 1 / dialogue.speed;
        for (float i = 0; i < speed; i+=Time.deltaTime)
        {
            int value = (int)Mathf.Lerp(0, dialogue.text.Length - 1, i / speed);
            GameUI.Instance.DialogueText.text = dialogue.text.Substring(0, value);
            yield return new WaitForFixedUpdate();
        }
        GameUI.Instance.DialogueText.text = dialogue.text;
    }
}
