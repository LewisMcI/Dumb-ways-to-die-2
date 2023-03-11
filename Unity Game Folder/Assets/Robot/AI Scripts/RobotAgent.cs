using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAgent : SteeringAgent
{
    protected enum State
    {
        Patrol,
        ChaseTarget, 
        Attacking
    }
    
    // Current state of Robot.
    private State currentState;

    // Line of Sight script of Robot.
    [SerializeField]
    LineOfSight robotLineOfSight;
    [SerializeField]
    float distanceToAttack;

    /* Called on Play().
     * Used to generate NavMesh automatically without having to manually build on every scene edit.
     */
    private void Awake()
    {
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    /* Cooperative Arbitration
     * 
     */
    protected override void CooperativeArbitration()
    {
        // TODO: FIX
        foreach (var behaviour in steeringBehvaiours)
        {
            behaviour.enabled = false;
        }
        // If LineOfSight has detected objects.
        if (robotLineOfSight.Objs != null)
            // If there are objects in line of sight.
            if (robotLineOfSight.Objs.Count > 0)
            {
                if (Vector3.Distance(robotLineOfSight.Objs[0].transform.position, transform.position) < distanceToAttack)
                    AttackPlayer();
                else
                    ChasePlayer();
            }
            else
            {
                Patrol();
            }
        else
        {
            Patrol();
        }
        base.CooperativeArbitration();
    }

    void AttackPlayer()
    {
       /* PlayerController.Instance.*/
        ChangeState(State.Attacking);
    }
    void ChasePlayer()
    {

        ChaseTarget chasePlayerScript = GetComponent<ChaseTarget>();
        if (chasePlayerScript)
            chasePlayerScript.enabled = true;
        ChangeState(State.ChaseTarget);
    }
    void Patrol()
    {
        GetComponent<Patrol>().enabled = true;
        ChangeState(State.Patrol);
    }


    protected void ChangeState(State newState)
    {
        currentState = newState;
        Debug.Log("Change state to: " + currentState);
    }
}
