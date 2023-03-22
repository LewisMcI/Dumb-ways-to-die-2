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
    protected enum RobotState
    {
        Patrolling,
        Chasing, 
        Attacking,
        Idle
    }
    
    private RobotState currentState = RobotState.Idle;

    [SerializeField]
    LineOfSight robotLineOfSight;
    [SerializeField]
    private float distanceToAttack;

    [SerializeField]
    private VideoPlayer tvExpression, tvStatic;
    [SerializeField]
    private VideoClip[] expressions;

    [SerializeField]
    private RobotPunchingGlove punchingGlove;
    [SerializeField]
    private float followTime, attackCooldown;
    private float followTimer, attackTimer;

    [SerializeField]
    RobotBearTrap bearTrap;

    private bool activated;
    #endregion

    #region properties
    public bool Activated
    {
        get { return activated; }
        set { activated = value; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        // Generate NavMesh
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    IEnumerator TransitionTV(VideoClip clip)
    {
        // Change video clip
        tvExpression.clip = clip;
        // Switch to static screen
        tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, 0.0f, tvExpression.transform.localPosition.z);
        tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, -0.000175f, tvStatic.transform.localPosition.z);
        // Wait for load
        yield return new WaitForSeconds(0.25f);
        // Switch to expression screen
        tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, -0.000175f, tvExpression.transform.localPosition.z);
        tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, 0.0f, tvStatic.transform.localPosition.z);
    }

    protected override void CooperativeArbitration()
    {
        if (activated)
        {
            // If there are objects in line of sight
            if (robotLineOfSight.Objs.Count > 0)
            {
                // Player within attack range
                if (Vector3.Distance(robotLineOfSight.Objs[0].transform.position, transform.position) < distanceToAttack && attackTimer <= 0.0f)
                {
                    if (currentState != RobotState.Attacking)
                    {
                        SwitchAttack();
                    }
                    else
                    {
                        TryAttackPlayer();
                    }
                }
                // Player not in attack range
                else if (currentState != RobotState.Chasing)
                {
                    SwitchChase();
                }

                // Reset
                if (followTimer != followTime)
                    followTimer = followTime;

                // Switch to angry expression
                if (tvExpression.clip != expressions[1])
                    StartCoroutine(TransitionTV(expressions[1]));
            }
            else if (currentState != RobotState.Patrolling)
            {
                if (followTimer <= 0.0f)
                {
                    SwitchPatrol();

                    // Switch to neutral expression
                    if (tvExpression.clip != expressions[0])
                        StartCoroutine(TransitionTV(expressions[0]));
                }
                else
                {
                    followTimer -= DefaultUpdateTimeInSecondsForAI;
                }
            }

            // Place beartraps when patrolling
            if (currentState == RobotState.Patrolling)
            {
                TryPlaceBearTrap();
            }

            if (attackTimer > 0.0f)
            {
                attackTimer -= DefaultUpdateTimeInSecondsForAI;
            }
        }

        base.CooperativeArbitration();
    }

    protected void ChangeState(RobotState newState)
    {
        currentState = newState;
        Debug.Log("Change state to: " + currentState);
    }

    private void SwitchIdle()
    {
        // Disable movement
        agent.isStopped = true;

        // Disable active behaviours
        foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours)
        {
            currentBehaviour.enabled = false;
        }
        // Switch state
        ChangeState(RobotState.Idle);
    }

    private void SwitchAttack()
    {
        // Switch state
        ChangeState(RobotState.Attacking);
    }

    private void SwitchChase()
    {
        // Enable movement
        agent.isStopped = false;

        // Change to chase behaviour
        EnableSteeringBehaviour(steeringBehvaiours[1]);
        // Switch state
        ChangeState(RobotState.Chasing);
    }

    private void SwitchPatrol()
    {
        // Enable movement
        agent.isStopped = false;

        // Change to patrol behaviour
        EnableSteeringBehaviour(steeringBehvaiours[0]);
        // Switch state
        ChangeState(RobotState.Patrolling);
    }

    private void TryPlaceBearTrap()
    {
        bearTrap.Place();
    }

    private void TryAttackPlayer()
    {
        // Has attack cooldown finished
        if (attackTimer <= 0.0f)
        {
            // Punch
            punchingGlove.Action();
            // Reset
            attackTimer = attackCooldown;
        }
    }
    #endregion
}
