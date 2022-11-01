using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields
    public static GameManager Instance;
    #endregion

    #region methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
