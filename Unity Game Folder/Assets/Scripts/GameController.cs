using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject UIMenu;
    List<GameObject> players = new List<GameObject>();
    public static GameController Instance { get; private set; }

    private bool gameBegun;

    private void Awake()
    {
        gameBegun = true;
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
