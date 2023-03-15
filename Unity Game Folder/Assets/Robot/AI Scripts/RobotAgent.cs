using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class RobotAgent : SteeringAgent
{
    #region fields
    protected enum State
    {
        Patrol,
        ChaseTarget, 
        Attacking
    }
    
    private State currentState;

    [SerializeField]
    LineOfSight robotLineOfSight;
    [SerializeField]
    private float distanceToAttack;

    [SerializeField]
    private float timeBetweenStuns = 8.0f;

    [SerializeField]
    private PunchingGlove punchingGlove;

    [SerializeField]
    private VideoPlayer tv1, tv2;

    bool patrolling, chasing, attacking;
    bool transitionTV;
    #endregion

    #region methods
    private void Awake()
    {
        // Used to generate NavMesh automatically without having to manually build on every scene edit
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    IEnumerator TransitionTV()
    {
        tv1.Stop();
        tv2.Stop();
        tv1.Play();
        tv2.Play();
        yield return new WaitForSeconds(0.3f);
        transitionTV = !transitionTV;
        tv1.transform.localPosition = (!transitionTV) ? new Vector3(tv1.transform.localPosition.x, -0.000175f, tv1.transform.localPosition.z) : new Vector3(tv1.transform.localPosition.x, 0.0f, tv1.transform.localPosition.z);
        tv2.transform.localPosition = (transitionTV) ? new Vector3(tv2.transform.localPosition.x, -0.000175f, tv2.transform.localPosition.z) : new Vector3(tv2.transform.localPosition.x, 0.0f, tv2.transform.localPosition.z);
    }

    protected override void CooperativeArbitration()
    {
        // If there are objects in line of sight
        if (robotLineOfSight.Objs.Count > 0)
        {
            if (Vector3.Distance(robotLineOfSight.Objs[0].transform.position, transform.position) < distanceToAttack && !attacking)
                AttackPlayer();
            else if (!chasing)
                ChasePlayer();
        }
        else if (!patrolling)
        {
            Patrol();
        }

        base.CooperativeArbitration();
    }

    private void Idle()
    {
        agent.isStopped = true;
    }

    private void AttackPlayer()
    {
        chasing = false;
        patrolling = false;
        attacking = true;

        //timeTillNextAttack = Time.time + timeBetweenStuns;
        ChangeState(State.Attacking);

        transform.LookAt(PlayerController.Instance.transform);
        punchingGlove.Action();
        robotLineOfSight.Objs.Clear();
        Debug.Log("attack");
    }

    private void ChasePlayer()
    {
        patrolling = false;
        chasing = true;
        patrolling = false;

        EnableSteeringBehaviour(steeringBehvaiours[1]);
        ChangeState(State.ChaseTarget);
        Debug.Log("chase");
    }

    private void Patrol()
    {
        patrolling = true;
        chasing = false;
        attacking = false;

        EnableSteeringBehaviour(steeringBehvaiours[0]);
        ChangeState(State.Patrol);
        Debug.Log("patrol");
    }

    protected void ChangeState(State newState)
    {
        currentState = newState;
        //Debug.Log("Change state to: " + currentState);
    }
    #endregion
}
