using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosive : Interactable
{
    #region fields
    [SerializeField]
    private LayerMask breakLayer;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private float sphereDistance;
    [SerializeField]
    private GameObject rootBone;
    [SerializeField]
    private RobotPunchingGlove punchingGlove;
    private RobotAgent robot;
    #endregion

    #region methods
    private void Awake()
    {
        robot = punchingGlove.transform.root.GetComponent<RobotAgent>();
        StartCoroutine(ActivateExplosive());
    }

    IEnumerator ActivateExplosive()
    {
        while (!Interacting)
        {
            yield return new WaitForFixedUpdate();
        }
        while (Interacting)
        {
            yield return new WaitForFixedUpdate();
        }

        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Play fx
        GetComponent<VisualEffect>().Play();
        GetComponent<AudioSource>().Play();

        // Disable
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(GetComponent<Rigidbody>());

        // Check player collision
        if (Physics.CheckSphere(transform.position, sphereDistance, playerLayer))
        {
            Vector3 dir = (PlayerController.Instance.transform.position - transform.position).normalized;
            PlayerController.Instance.ThrowPlayerInDirection(dir, 0.2f);
            return;
        }
        // Check robot collision
        if (Physics.CheckSphere(transform.position, sphereDistance, breakLayer))
        {
            BreakPunchingGlove();
            return;
        }
    }

    private void BreakPunchingGlove()
    {
        // Remove
        rootBone.transform.parent = null;
        Destroy(punchingGlove);
        robot.CheckDeath();
        // Drop
        rootBone.AddComponent<Rigidbody>();
        rootBone.AddComponent<Interactable>();
    }
    #endregion
}