using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Image dotImage;
    private Animator dotAnim;

    public static GameUI Instance;
    #endregion

    #region properties
    public Animator DotAnim
    {
        get { return dotAnim; }
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

        dotAnim = dotImage.GetComponent<Animator>();
    }
    #endregion
}
