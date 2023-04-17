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
    [SerializeField]
    private RobotAgent robot;
    #endregion

    #region methods
    private void Awake()
    {
        value = transform.name;
    }

    public override void Action()
    {
        // Input
        switch (codeType)
        {
            case CodeType.Number:
                if (codeText.text.Length < 8)
                {
                    codeText.text = codeText.GetComponent<TextMeshPro>().text + value.ToString() + " ";
                    StartCoroutine(Click());
                    GetComponent<AudioSource>().Play();
                }
                break;
            case CodeType.Delete:
                if (codeText.text.Length >= 2)
                {
                    codeText.text = codeText.text.Substring(0, codeText.text.Length - 2);
                    StartCoroutine(Click());
                    GetComponent<AudioSource>().Play();
                }
                break;
            case CodeType.Other:
                StartCoroutine(Click());
                GetComponent<AudioSource>().Play();
                break;
        }
        // Code complete
        if (codeText.text.Length >= 8)
        {
            if (robot.Dead)
            {
                int num1 = int.Parse(codeText.text.ToString()[0].ToString());
                int num2 = int.Parse(codeText.text.ToString()[2].ToString());
                int num3 = int.Parse(codeText.text.ToString()[4].ToString());
                int num4 = int.Parse(codeText.text.ToString()[6].ToString());
                int[] code = new int[4] { num1, num2, num3, num4 };
                for (int i = 0; i < code.Length; i++)
                {
                    if (code[i] != robot.Code[i])
                    {
                        ReduceTime();
                        return;
                    }
                }
                Debug.Log("SUCCESS");
            }
            else
            {
                ReduceTime();
            }
        }
    }

    private void ReduceTime()
    {
        Debug.Log("FAIL");
    }

    IEnumerator Click()
    {
        float startingY = transform.localPosition.y;
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, -0.02f, transform.localPosition.z), 50.0f * Time.deltaTime);
        yield return new WaitForSeconds(0.1f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, startingY, transform.localPosition.z), 50.0f * Time.deltaTime);
    }
    #endregion
}
