using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PunchingGlove : MonoBehaviour
{
    public Transform constraint;
    public Transform punchingGloveBone;

    public float maxDist;
    public float speed = 1.0f;
    private Vector3 localStartingPos;
    private VisualEffect whamVFX;
    Vector3 finalPos;
    bool shouldStop = false;
    [SerializeField]
    float distanceToHit = .5f;

    bool canAttack = true;
    [SerializeField]
    LayerMask layerMask;
    private void Awake()
    {
        whamVFX = punchingGloveBone.GetComponentInChildren<VisualEffect>();
    }
    public void Action()
    { 
        if (canAttack)
        {
            localStartingPos = constraint.localPosition;
            Attack();
        }
    }
    void Attack()
    {
        canAttack = false;
        finalPos = constraint.InverseTransformPoint(PlayerController.Instance.transform.position);

        StartCoroutine(MovePunchingGlove(localStartingPos, speed));
    }
    void Retract()
    {
        StartCoroutine(RetractGlove(constraint.localPosition, localStartingPos, speed));
    }
    
    IEnumerator MovePunchingGlove(Vector3 startingDistance, float moveSpeed)
    {
        while (shouldStop == true)
        {
            yield return new WaitForFixedUpdate();
        }
        float time = 1 / moveSpeed;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            finalPos = constraint.InverseTransformPoint(PlayerController.Instance.transform.position);
            finalPos.y = maxDist;

            if (ShootRay())
            {
                shouldStop = false;
                yield break;
            }
            float x = Mathf.Lerp(startingDistance.x, finalPos.x, (i +.9f/ time));
            float y = Mathf.Lerp(startingDistance.y, finalPos.y, (i / time));
            float z = Mathf.Lerp(startingDistance.z, finalPos.z, (i +.9f / time));

            constraint.localPosition = new Vector3(x, y, z);
            yield return new WaitForFixedUpdate();  
        }
        constraint.localPosition = finalPos;
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
        canAttack = true;
    }

    bool ShootRay()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(punchingGloveBone.transform.position, punchingGloveBone.transform.TransformDirection(Vector3.forward), out hit, distanceToHit, layerMask))
        {
            whamVFX.Play();
            Debug.Log("Hit" + hit.collider.name);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player") || hit.collider.name == "Character" || hit.collider.name == "Main Camera")
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
