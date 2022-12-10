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
    [SerializeField]
    private TextMeshProUGUI clothingText;
    #endregion

    #region methods
    public void Check()
    {
        noCompleted++;
        clothingText.text = "Clothing Found " + noCompleted + "/4";
        // Check if clothes are collected
        bool allCollected = true;
        for (int i = 0; i < clothes.Length; i++)
        {
            if (!clothes[i].Collected)
                allCollected = false;
        }
        // Mark task as complete if all clothes collected

        GameManager.Instance.UpdateTaskCompletion("Get Clothes");
        if (allCollected)
        {
            clothingText.gameObject.SetActive(false);
        }
        
    }
    #endregion
}
