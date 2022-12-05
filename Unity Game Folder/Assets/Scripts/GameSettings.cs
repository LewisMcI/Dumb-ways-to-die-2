using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public bool tutorial = true;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
