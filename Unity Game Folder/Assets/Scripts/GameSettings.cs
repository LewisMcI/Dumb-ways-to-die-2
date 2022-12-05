using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public bool tutorial = true;
    void Awake()
    {
        GameSettings[] objs = GameObject.FindObjectsOfType<GameSettings>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
