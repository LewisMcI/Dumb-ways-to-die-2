using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAgent : SteeringAgent
{
    protected enum State
    {
        Patrol,
        ChaseTarget
    }
    
    // Current state of Robot.
    private State currentState;

    // Line of Sight script of Robot.
    [SerializeField]
    LineOfSight robotLineOfSight;

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
        // If LineOfSight has detected objects.
        if (robotLineOfSight.Objs != null)
            // If there are objects in line of sight.
            if (robotLineOfSight.Objs.Count > 0)
            {
                ChangeState(State.ChaseTarget);
            }
            else
            {
                ChangeState(State.Patrol);
            }
        else
        {
            ChangeState(State.Patrol);
        }

        // TODO: FIX
        foreach (var behaviour in steeringBehvaiours)
        {
            behaviour.enabled = false;
        }
        if (currentState == State.ChaseTarget)
        {
            ChaseTarget chasePlayerScript = GetComponent<ChaseTarget>();
            if (chasePlayerScript)
                chasePlayerScript.enabled = true;
        }
        else if (currentState == State.Patrol)
            GetComponent<Patrol>().enabled = true;

        base.CooperativeArbitration();
    }

    protected void ChangeState(State newState)
    {
        currentState = newState;
        Debug.Log("Change state to: " + currentState);
    }
}
