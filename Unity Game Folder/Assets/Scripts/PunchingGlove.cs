using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingGlove : MonoBehaviour
{
    public Transform constraint;
    public Transform punchingGloveBone;

    public float maxDist;
    public float speed = 1.0f;
    private Vector3 localStartingPos;

    bool shouldStop = false;
    [SerializeField]
    float distanceToHit = .5f;

    [SerializeField]
    LayerMask layerMask;
    private void Awake()
    {/*
        Action();*/
    }
    public void Action()
    {
        localStartingPos = constraint.localPosition;
        Attack();
    }
    void Attack()
    {
        Vector3 finalPos = constraint.InverseTransformPoint(PlayerController.Instance.transform.position);
        finalPos.y = maxDist;
        StartCoroutine(MovePunchingGlove(localStartingPos, finalPos, speed));
    }
    void Retract()
    {
        StartCoroutine(RetractGlove(constraint.localPosition, localStartingPos, speed));
    }
    
    IEnumerator MovePunchingGlove(Vector3 startingDistance, Vector3 finalPosition, float moveSpeed)
    {
        yield return new WaitForSeconds(3.0f);
        while (shouldStop == true)
        {
            yield return new WaitForFixedUpdate();
        }
        float time = 1 / moveSpeed;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            if (ShootRay())
            {
                shouldStop = false;
                yield break;
            }
            float x = Mathf.Lerp(startingDistance.x, finalPosition.x, (i / time) * 2);
            float y = Mathf.Lerp(startingDistance.y, finalPosition.y, (i / time));
            float z = Mathf.Lerp(startingDistance.z, finalPosition.z, (i / time) * 2);

            constraint.localPosition = new Vector3(x, y, z);
            yield return new WaitForFixedUpdate();
        }
        constraint.localPosition = finalPosition;
        Retract();
    }

    IEnumerator RetractGlove(Vector3 startingDistance, Vector3 finalPosition, float moveSpeed)
    {
        while (shouldStop == true)
        {
            yield return new WaitForFixedUpdate();
        }
        float time = 1 / moveSpeed;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            constraint.localPosition = Vector3.Lerp(startingDistance, finalPosition, i / time);
            yield return new WaitForFixedUpdate();
        }
        constraint.localPosition = finalPosition;
    }

    bool ShootRay()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(punchingGloveBone.transform.position, punchingGloveBone.transform.TransformDirection(Vector3.forward), out hit, distanceToHit, layerMask))
        {
            Debug.Log("Hit" + hit.collider.name);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player") || hit.collider.name == "Character")
            {
                PlayerController.Instance.DisableDeathFromCollision(4.0f);
                PlayerController.Instance.ThrowPlayerInRelativeDirection(25.0f, Direction.backwards, 1.0f, true);
            }
            Retract();
            return true;
        }
        return false;
    }

}
