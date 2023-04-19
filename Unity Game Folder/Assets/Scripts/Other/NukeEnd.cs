using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeEnd : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    GameObject explosion;

    [SerializeField]
    AudioSource explosionSFX;

    [SerializeField]
    Collider[] colliders;
    [SerializeField]
    float distance;
    [SerializeField]
    LayerMask targetLayers;
    [SerializeField]
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(AddForce());
    }

    IEnumerator AddForce()
    {
        yield return new WaitForSeconds(2.0f);
        animator.SetTrigger("Fire");
        yield return new WaitForSeconds(7.0f);
        int count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, targetLayers, QueryTriggerInteraction.Collide);
        Collider[] newColliders = Physics.OverlapSphere(transform.position, distance, targetLayers);
        Debug.Log(newColliders.Length);

        for (int i = 0; i < newColliders.Length; i++)
        {
            newColliders[i].gameObject.AddComponent<Rigidbody>();
        }
        rb.AddExplosionForce(20000.0f, transform.position, distance);

        explosionSFX.Play();
        Destroy(this);
    }
}
