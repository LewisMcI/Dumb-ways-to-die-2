using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBreakDishwasher : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject destroyer;
    [SerializeField]
    private RobotLaserDetection laserDetection;
    [SerializeField]
    private GameObject smoke;
    [SerializeField]
    RobotBearTrap robotBearTrap;
    #endregion

    #region methods
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == destroyer)
        {
            // Reset animation
            transform.root.GetChild(0).GetComponent<Animator>().SetBool("Frying Pan", false);

            // Disable scripts
            laserDetection.ChangeRed();
            laserDetection.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
            Destroy(laserDetection);
            robotBearTrap.StopAllCoroutines();
            Destroy(robotBearTrap);

            // Enable smoke
            smoke.gameObject.SetActive(true);
        }
    }
    #endregion
}
