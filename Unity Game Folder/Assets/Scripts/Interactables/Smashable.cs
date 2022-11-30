using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smashable : MonoBehaviour
{
    [SerializeField]
    private GameObject[] smashedPieces;
    [SerializeField]
    private float destroyTime = 2.5f;

    private Rigidbody rb;
    private BoxCollider bc;
    private bool broken = false;

    private void Awake()
    {
        try
        {
            rb = GetComponent<Rigidbody>();
        }
        catch
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        try
        {
            bc = GetComponent<BoxCollider>();
        }
        catch
        {
            bc = gameObject.AddComponent<BoxCollider>();
        }
    }
    private void FixedUpdate()
    {
        if (!broken && rb.velocity.magnitude >= 1)
        {
            CanBreak();
        }
    }

    void CanBreak()
    {
        if (!GetComponent<Interactable>().interacting)
        {
            bc.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        broken = true;
        Destroy(bc); Destroy(rb);
        GetComponent<Interactable>().type = Interactable.Type.None;
        foreach(var piece in smashedPieces)
        {
            Rigidbody newRb = piece.AddComponent<Rigidbody>();
            newRb.AddExplosionForce(10, Vector3.down, 10);
            AudioManager.Instance.PlayAudio("Bottle Smash");
        }
        StartCoroutine(DestroyAfterSeconds(destroyTime));
    }

    IEnumerator DestroyAfterSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
