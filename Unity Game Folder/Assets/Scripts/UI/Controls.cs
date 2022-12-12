using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject loading;
    #endregion

    #region methods
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !loading.activeSelf)
        {
            gameObject.SetActive(false);
            loading.SetActive(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    #endregion
}
