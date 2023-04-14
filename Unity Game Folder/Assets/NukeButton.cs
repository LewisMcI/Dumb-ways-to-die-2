using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeButton : Interactable
{
    [SerializeField]
    private GameObject lights;
    [SerializeField]
    private RobotAgent robot;

    private Animator animator;

    [SerializeField]
    Transform[] barricadeParents;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Action()
    {
        lights.SetActive(true);
        animator.SetTrigger("activate");
        StartCoroutine(ActivateRobot());
    }

    IEnumerator ActivateRobot()
    {
        yield return new WaitForSeconds(1.0f);
        // TODO: Activate RigidBodies of Boxes
        // TODO: Default robot to lights off and Activate here
        foreach(Transform boxParent in barricadeParents)
        {
            for (int i = 0; i < boxParent.childCount; i++)
            {
                boxParent.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        robot.Activate();

        Destroy(this);
    }
}
