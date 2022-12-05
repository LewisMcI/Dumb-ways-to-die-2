using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBrush : Interactable
{
    #region fields
    #endregion

    #region methods
    public override void Action()
    {
        AudioManager.Instance.PlayAudio("Brush Teeth");
        type = Type.None;

        GameManager.Instance.SetTaskComplete("Brush Teeth");
    }
    #endregion
}
