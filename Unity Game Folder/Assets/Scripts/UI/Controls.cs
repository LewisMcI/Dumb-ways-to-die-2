using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
