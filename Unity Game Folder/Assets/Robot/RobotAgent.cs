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
    /// <summary>
    /// The current state the guard is in
    /// </summary>
    private State currentState;

    int index = 0;

    [SerializeField]
    LineOfSight playerLineOfSight;

    private void Awake()
    {
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }
    protected override void CooperativeArbitration()
    {
        /* If can see player
         * Chase Player.
         */
        if (playerLineOfSight.Objs != null)
            if (playerLineOfSight.Objs.Count > 0)
            {
                ChangeState(State.ChaseTarget);
            }
        /* If can't see player
         * Patrol Set Area.
         */
        else
        {
            ChangeState(State.Patrol);
        }

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
/*    private bool IsPlayerInSight()
    {
        return IsPointInLineRenderer(PlayerController.Instance.transform.position, lineOfSight);
    }*/
   
    static public bool IsPointInLineRenderer(Vector2 point, LineRenderer lineRenderer)
    {
        if (lineRenderer.loop == false)
        {
            return false;
        }

        var polygon = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(polygon);

        if (lineRenderer.useWorldSpace == false)
        {
            for (int index = 0; index < polygon.Length; ++index)
            {
                polygon[index] = lineRenderer.localToWorldMatrix.MultiplyPoint(polygon[index]);
            }
        }


        var previousIndex = polygon.Length - 1;
        var isInsidePolygon = false;

        for (int index = 0; index < polygon.Length; ++index)
        {
            if (point.x == polygon[index].x && point.y == polygon[index].y)
            {
                // point is a corner
                return true;
            }

            if ((polygon[index].y > point.y) != (polygon[previousIndex].y > point.y))
            {
                var slope = (point.x - polygon[index].x) * (polygon[previousIndex].y - polygon[index].y) - (polygon[previousIndex].x - polygon[index].x) * (point.y - polygon[index].y);

                if (slope == 0)
                {
                    // Point is on a boundary
                    return true;
                }
                if ((slope < 0) != (polygon[previousIndex].y < polygon[index].y))
                {
                    isInsidePolygon = !isInsidePolygon;
                }

            }

            previousIndex = index;
        }

        return isInsidePolygon;
    }
}
