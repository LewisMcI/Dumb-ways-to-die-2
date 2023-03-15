using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosive : Interactable
{
    BoxCollider boxCollider;

    VisualEffect explosiveVFX;
    [SerializeField]
    LayerMask breakLayer;
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    float sphereDistance;
    [SerializeField]
    GameObject rootBone;
    [SerializeField]
    PunchingGlove punchingGlove;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        explosiveVFX = GetComponent<VisualEffect>();
        StartCoroutine(ActivateExplosive());
    }

    IEnumerator ActivateExplosive()
    {
        while (!Interacting)
        {
            yield return new WaitForFixedUpdate();
        }
        while (Interacting)
        {
            yield return new WaitForFixedUpdate();
        }

        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        explosiveVFX.Play();
        boxCollider.enabled = false;
        Destroy(GetComponent<Rigidbody>());
        GetComponent<MeshRenderer>().enabled = false;
        if (Physics.CheckSphere(transform.position, sphereDistance, playerLayer))
        {
            Vector3 dir = (PlayerController.Instance.transform.position - transform.position).normalized;
            PlayerController.Instance.ThrowPlayerInDirection(dir, 0.2f);
            return;
        }
        if (Physics.CheckSphere(transform.position, sphereDistance, breakLayer))
        {
            BreakPunchingGlove();
            return;
        }
    }

    private void BreakPunchingGlove()
    {
        rootBone.transform.parent = null;
        Destroy(punchingGlove);
        rootBone.AddComponent<Rigidbody>();
        rootBone.AddComponent<Interactable>();
        
    }
}