using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchTV : Interactable
{
    #region fields
    #endregion

    #region methods
    public override void Action()
    {
        GameManager.Instance.SetTaskComplete("Watch TV");
    }
    #endregion
}
