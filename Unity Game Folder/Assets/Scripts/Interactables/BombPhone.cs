using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombPhone : Interactable
{
    enum CodeType
    {
        Number,
        Delete,
        Other
    }

    #region fields
    private string value;
    [SerializeField]
    private CodeType codeType;
    [SerializeField]
    private TextMeshPro codeText;
    #endregion

    #region methods
    private void Awake()
    {
        value = transform.name;
    }

    public override void Action()
    {
        switch (codeType)
        {
            case CodeType.Number:
                if (codeText.text.Length < 8)
                {
                    codeText.text = codeText.GetComponent<TextMeshPro>().text + value.ToString() + " ";
                    GetComponent<Animator>().SetTrigger("Press");
                    GetComponent<AudioSource>().Play();
                }
                break;
            case CodeType.Delete:
                codeText.text = codeText.text.Substring(0, codeText.text.Length - 1);
                GetComponent<Animator>().SetTrigger("Press");
                GetComponent<AudioSource>().Play();
                break;
            case CodeType.Other:
                GetComponent<Animator>().SetTrigger("Press");
                GetComponent<AudioSource>().Play();
                break;
        }
    }
    #endregion
}
