using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Clothing[] clothes;
    int noCompleted = 0;
    #endregion

    #region methods
    public void Check()
    {
        noCompleted++;
        // Mark task as complete if all clothes collected

        GameManager.Instance.taskManager.UpdateTaskCompletion("Get Clothes");
        
    }
    #endregion
}
