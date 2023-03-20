using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PunchingGlove : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject constraint;
    [SerializeField]
    private VisualEffect whamVFX;
    private bool attack;
    #endregion

    #region methods
    private void Update()
    {
        if (attack)
        {
            // Rotate towards player
            Vector3 dir = (PlayerController.Instance.transform.position - transform.root.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.root.rotation = Quaternion.Slerp(transform.root.rotation, lookRot, 5.0f * Time.deltaTime);

            // Move boxing glove towards player
            constraint.transform.position = Vector3.Lerp(constraint.transform.position, PlayerController.Instance.transform.position, 5f * Time.deltaTime);

            // Retract towards initial position
            if (Vector3.Distance(constraint.transform.position, PlayerController.Instance.transform.position) <= 1.0f)
            {
                //constraint.transform.localPosition = new Vector3(0.0f, -0.0005245219f, 0.0f);
                PlayerController.Instance.DisableDeathFromCollision(4.0f);
                PlayerController.Instance.ThrowPlayerInRelativeDirection(25.0f, Direction.backwards, 1.0f, true);
                attack = false;
                whamVFX.Play();
            }
        }
        else
        {
            constraint.transform.localPosition = Vector3.Lerp(constraint.transform.localPosition, new Vector3(0.0f, -0.0005245219f, 0.0f), 3.5f * Time.deltaTime);
        }
    }

    public void Action()
    {
        attack = true;
    }
    #endregion

    /*
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
    */
}
