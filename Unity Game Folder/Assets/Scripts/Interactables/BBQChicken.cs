using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBQChicken : Interactable
{
    #region fields

    #endregion

    #region methods
    private void Awake()
    {
        CanInteract = false;
    }

    public override void Action()
    {
        GetComponent<Animator>().SetTrigger("Flip");
        GameManager.Instance.taskManager.UpdateTaskCompletion("Relax Outside");
    }
    #endregion
}
