using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject blinkImage;
    private Animator blinkAnim;
    #endregion

    #region methods
    private void Awake()
    {
        blinkAnim = blinkImage.GetComponent<Animator>();
    }

    public void TriggerBlink()
    {
        blinkAnim.SetTrigger("Blink");
    }
    #endregion
}
