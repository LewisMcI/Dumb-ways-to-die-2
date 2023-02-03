using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBrush : Interactable
{
    #region methods
    public override void Action()
    {
        AudioManager.Instance.PlayAudio("Brush Teeth");
        CanInteract = false;

        GameManager.Instance.UpdateTaskCompletion("Brush Teeth");
    }
    #endregion
}
