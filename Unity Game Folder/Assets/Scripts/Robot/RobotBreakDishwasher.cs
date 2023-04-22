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
            if (laserDetection)
            {
                laserDetection.ChangeRed();
                laserDetection.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
                Destroy(laserDetection);
            }
            robotBearTrap.StopAllCoroutines();
            robot.BearTrap = null;
            Destroy(robotBearTrap);
            robot.DisableMovement();
            robot.CheckDeath();

            // Enable smoke
            smoke.gameObject.SetActive(true);
            enabled = false;
        }
    }
    #endregion
}
