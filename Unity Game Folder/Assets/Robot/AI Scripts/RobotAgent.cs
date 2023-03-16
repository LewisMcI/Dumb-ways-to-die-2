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
    private VideoClip[] expressions;
    [SerializeField]
    private VideoPlayer tvExpression, tvStatic;
    [SerializeField]
    RobotBearTrap bearTrap;

    bool patrolling, chasing, attacking;
    #endregion

    #region methods
    private void Awake()
    {
        // Used to generate NavMesh automatically without having to manually build on every scene edit
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    IEnumerator TransitionTV(VideoClip clip)
    {
        tvExpression.clip = clip;
        tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, 0.0f, tvExpression.transform.localPosition.z);
        tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, -0.000175f, tvStatic.transform.localPosition.z);
        yield return new WaitForSeconds(0.25f);
        tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, -0.000175f, tvExpression.transform.localPosition.z);
        tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, 0.0f, tvStatic.transform.localPosition.z);
    }

    protected override void CooperativeArbitration()
    {
        TryPlaceBearTrap();
        // If there are objects in line of sight
        if (robotLineOfSight.Objs.Count > 0)
        {
            if (Vector3.Distance(robotLineOfSight.Objs[0].transform.position, transform.position) < distanceToAttack && !attacking)
            {
                AttackPlayer();
            }
            else if (!chasing)
            {
                ChasePlayer();
            }
            if (tvExpression.clip != expressions[1])
                StartCoroutine(TransitionTV(expressions[1]));
        }
        else if (!patrolling)
        {
            Patrol();

            if (tvExpression.clip != expressions[0])
                StartCoroutine(TransitionTV(expressions[0]));
        }

        base.CooperativeArbitration();
    }
    void TryPlaceBearTrap()
    {
        bearTrap.Place();
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
        foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours)
        {
            currentBehaviour.enabled = false;
        }
        agent.isStopped = true;

        //timeTillNextAttack = Time.time + timeBetweenStuns;
        ChangeState(State.Attacking);

        punchingGlove.Action();
        robotLineOfSight.Objs.Clear();
        Debug.Log("attack");
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;

        patrolling = false;
        chasing = true;
        patrolling = false;

        EnableSteeringBehaviour(steeringBehvaiours[1]);
        ChangeState(State.ChaseTarget);
        Debug.Log("chase");
    }

    private void Patrol()
    {
        agent.isStopped = false;
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
