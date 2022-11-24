using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Interactable
{
    #region fields
    [SerializeField]
    private Task makeToastTask;
    #endregion

    #region methods
    public override void Action()
    {
        AudioManager.Instance.PlayAudio("Eat");
        type = Type.None;
        transform.GetComponent<Renderer>().enabled = false;

        if (makeToastTask != null)
            GameManager.Instance.CompletedTask(makeToastTask);
    }
    #endregion
}
