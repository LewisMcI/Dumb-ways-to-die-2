using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Interactable
{
    #region fields

    #endregion

    #region methods
    public override void Action()
    {
        AudioManager.Instance.PlayAudio("Eat");
        type = Type.None;
        transform.GetComponent<Renderer>().enabled = false;

        GameManager.Instance.UpdateTaskCompletion("Make and Eat Toast");
    }
    #endregion
}
