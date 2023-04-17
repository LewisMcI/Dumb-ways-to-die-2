using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBreakDishwasher : MonoBehaviour
{
    #region fields
    [SerializeField]
    private RobotLaserDetection laserDetection;
    [SerializeField]
    private GameObject smoke;
    [SerializeField]
    private RobotBearTrap robotBearTrap;
    private RobotAgent robot;
    #endregion

    #region methods
    private void Awake()
    {
        robot = robotBearTrap.transform.root.GetComponent<RobotAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("Axe"))
        {
            // Reset animation
            transform.root.GetChild(0).GetComponent<Animator>().SetBool("Defend Back", false);

            // Disable scripts
            laserDetection.ChangeRed();
            laserDetection.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
            robotBearTrap.StopAllCoroutines();
            robot.BearTrap = null;
            Destroy(laserDetection);
            Destroy(robotBearTrap);
            robot.DisableMovement();
            robot.CheckDeath();

            // Enable smoke
            smoke.gameObject.SetActive(true);
        }
    }
    #endregion
}
