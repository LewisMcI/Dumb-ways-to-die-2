using System.Collections;
using System.Linq;
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
    private RobotBearTrap bearTrap;
    [SerializeField]
    private RobotLaserDetection laserDetection;
    [SerializeField]
    private float followTime, attackCooldown;
    private float followTimer, attackTimer;

    [SerializeField]
    private GameObject fryingPan;

    private bool canMove = true;
    private bool activated;
    #endregion

    #region properties
    public bool Activated
    {
        get { return activated; }
        set { activated = value; }
    }
    public GameObject FryingPan
    {
        get { return fryingPan; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        // Disable movement
        agent.isStopped = true;

        // Switch to static screen
        tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, 0.0f, tvExpression.transform.localPosition.z);
        tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, -0.000175f, tvStatic.transform.localPosition.z);
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
            if (GameManager.Instance.IsPaused)
            {
                GetComponent<NavMeshAgent>().enabled = false;
                return;
            }
            else
            {
                GetComponent<NavMeshAgent>().enabled = true;
            }

            // If there are objects in line of sight
            if (punchingGlove && robotLineOfSight.Objs.Count > 0)
            {
                foreach (GameObject obj in robotLineOfSight.Objs)
                {
                    if (fryingPan)
                    {
                        // Explosive in sight
                        if (obj.layer == LayerMask.NameToLayer("Explosive") && obj.GetComponent<Collider>().isTrigger)
                        {
                            transform.GetChild(0).GetComponent<Animator>().SetBool("Defend Front", true);
                            transform.GetChild(0).GetComponent<Animator>().SetBool("Defend Back", false);
                        }
                        else if (transform.GetChild(0).GetComponent<Animator>().GetBool("Defend Front"))
                        {
                            transform.GetChild(0).GetComponent<Animator>().SetBool("Defend Front", false);
                        }
                    }

                    // Player in sight
                    if (obj.layer == LayerMask.NameToLayer("Player"))
                    {
                        // Player within attack range
                        if (Vector3.Distance(robotLineOfSight.Objs[0].transform.position, transform.position) < distanceToAttack && attackTimer <= 0.0f && !transform.GetChild(0).GetComponent<Animator>().GetBool("Defend Front"))
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
                        else if (canMove && currentState != RobotState.Chasing)
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
                }
            }
            else
            {
                if (canMove && currentState != RobotState.Patrolling)
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
                if (transform.GetChild(0).GetComponent<Animator>().GetBool("Defend Front"))
                {
                    transform.GetChild(0).GetComponent<Animator>().SetBool("Defend Front", false);
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

    public void Activate()
    {
        Switch();
        Activated = true;
        robotLineOfSight.Activate();
    }

    public void DisableMovement()
    {
        canMove = false;
        SwitchIdle();
    }

    public void Switch()
    {
        // Switch to expression screen
        tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, -0.000175f, tvExpression.transform.localPosition.z);
        tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, 0.0f, tvStatic.transform.localPosition.z);
    }

    public void CheckDeath()
    {
        if (!punchingGlove && !bearTrap && !laserDetection)
        {
            // Disable movement
            agent.isStopped = true;

            // Disable active behaviours
            foreach (SteeringBehaviour currentBehaviour in steeringBehvaiours)
            {
                currentBehaviour.enabled = false;
            }

            // Switch to static screen
            tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, 0.0f, tvExpression.transform.localPosition.z);
            tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, -0.000175f, tvStatic.transform.localPosition.z);
            GameManager.Instance.taskManager.UpdateTaskCompletion("Defeat Robot");
            activated = false;
        }
    }

    public void TriggerStun()
    {
        StartCoroutine(Stun());
        StartCoroutine(FryingPanForce());
        CheckDeath();
    }

    IEnumerator Stun()
    {
        SwitchIdle();

        // Switch to static screen
        tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, 0.0f, tvExpression.transform.localPosition.z);
        tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, -0.000175f, tvStatic.transform.localPosition.z);

        activated = false;

        yield return new WaitForSeconds(3.0f);

        // Switch to expression screen
        tvExpression.transform.localPosition = new Vector3(tvExpression.transform.localPosition.x, -0.000175f, tvExpression.transform.localPosition.z);
        tvStatic.transform.localPosition = new Vector3(tvStatic.transform.localPosition.x, 0.0f, tvStatic.transform.localPosition.z);

        activated = true;
    }

    IEnumerator FryingPanForce()
    {
        yield return new WaitForSeconds(0.3f);
        fryingPan.transform.parent = null;
        fryingPan.AddComponent<Rigidbody>();

        Vector3 force = (fryingPan.transform.forward * -10000.0f + fryingPan.transform.up * 10000.0f) * Time.deltaTime;
        yield return new WaitForFixedUpdate();
        force = (fryingPan.transform.forward * -10000.0f + fryingPan.transform.up * 10000.0f) * Time.deltaTime;
        fryingPan.GetComponent<Rigidbody>().AddForce(force);
        fryingPan = null;
    }
    #endregion
}
