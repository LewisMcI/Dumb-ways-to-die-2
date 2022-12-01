using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{ 
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }
}
