using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Explosive : Interactable
{
    BoxCollider boxCollider;

    VisualEffect explosiveVFX;
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
        Destroy(GetComponent<Rigidbody>());
        GetComponent<MeshRenderer>().enabled = false;
    }
}