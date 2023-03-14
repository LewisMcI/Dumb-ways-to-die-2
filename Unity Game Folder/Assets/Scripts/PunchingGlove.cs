using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingGlove : MonoBehaviour
{
    public Transform constraint;

    public float maxDist = -.01f;
    public float speed = 1.0f;
    private float startingDist;
    bool canCollide = false;
    bool shouldStop = false;
    private void Awake()
    {
        startingDist = constraint.transform.localPosition.y;
    }
    public void Action()
    {
        Attack();
    }

    void Attack()
    {
        canCollide = true;
        StartCoroutine(MovePunchingGlove(startingDist, maxDist, speed, true));
    }
    void Retract()
    {
        Debug.Log("Retracting: " + constraint.transform.localPosition.y + " " + startingDist);
        StartCoroutine(MovePunchingGlove(constraint.transform.localPosition.y, startingDist, speed, false));
    }
    IEnumerator MovePunchingGlove(float startingDistance, float maxDistance, float moveSpeed, bool shouldRetract)
    {
        while (shouldStop == true)
        {
            yield return new WaitForFixedUpdate();
        }
        moveSpeed = 1 / moveSpeed;
        for (float i = 0; i < moveSpeed; i += Time.deltaTime)
        {
            if (shouldStop)
            {
                shouldStop = false;
                canCollide = false;
                yield break;
            }
            float value = Mathf.Lerp(startingDistance, maxDistance, i / moveSpeed);
            constraint.localPosition = new Vector3(constraint.transform.localPosition.x, value, constraint.transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }

        constraint.localPosition = new Vector3(constraint.transform.localPosition.x, maxDistance, constraint.transform.localPosition.z);
        canCollide = false;
        if (shouldRetract)
            Retract();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer.ToString());
        Debug.Log("Collide");
        if (canCollide)
        {
            Debug.Log("Collide");
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("HIT PLAYER");
                PlayerController.Instance.DisableDeathFromCollision(4.0f);
                PlayerController.Instance.ThrowPlayerInRelativeDirection(25.0f, Direction.backwards, 1.0f, true);
            }
            shouldStop = true;
            Retract();
        }
    }
}
