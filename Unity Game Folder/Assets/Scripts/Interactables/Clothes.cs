using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Clothing[] clothings;
    #endregion

    #region properties
    public Clothing[] Clothings
    { 
        get { return clothings; } 
    }
    #endregion

    #region methods
    public void Check()
    {
        // Mark task as complete if all clothes collected
        GameManager.Instance.taskManager.UpdateTaskCompletion("Get Clothes");
    }
    #endregion
}
