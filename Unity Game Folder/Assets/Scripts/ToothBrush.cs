using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothBrush : Interactable
{
    #region fields
    [SerializeField]
    private Task brushTeethTask;
    #endregion

    #region methods
    public override void Action()
    {
        AudioManager.Instance.PlayAudio("Brush Teeth");
        type = Type.None;

        if (brushTeethTask != null)
            GameManager.Instance.CompletedTask(brushTeethTask);
    }
    #endregion
}
