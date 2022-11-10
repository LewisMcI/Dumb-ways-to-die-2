using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields
    private bool enableControls;
    private float enableControlsTimer = 2.5f;

    public static GameManager Instance;
    #endregion

    #region properties
    public bool EnableControls
    {
        get { return enableControls; }
        set { enableControls = value; }
    }
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
        if (enableControlsTimer <= 0.0f)
        {
            enableControls = true;
        }
        else
            enableControlsTimer -= Time.deltaTime;
    }
    #endregion
}
