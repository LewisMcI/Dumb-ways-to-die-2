using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameUI gameUI;

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

    private void Update()
    {

    }
    #endregion
}
