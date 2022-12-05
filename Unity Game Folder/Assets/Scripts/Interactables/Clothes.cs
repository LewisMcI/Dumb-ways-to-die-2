using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Clothing[] clothes;
    #endregion

    #region methods
    public void Check()
    {
        // Check if clothes are collected
        bool allCollected = true;
        for (int i = 0; i < clothes.Length; i++)
        {
            if (!clothes[i].Collected)
                allCollected = false;
        }
        // Mark task as complete if all clothes collected
        if (allCollected)
            GameManager.Instance.SetTaskComplete("Get Clothes");
    }
    #endregion
}
