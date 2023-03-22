using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBunker : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject lights;
    [SerializeField]
    private RobotAgent robot;
    #endregion

    #region methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            lights.SetActive(true);
            robot.Activated = true;
        }
    }
    #endregion
}
