using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jam : Interactable
{
    #region fields
    [SerializeField]
    private GameObject knife;
    [SerializeField]
    private Mesh knifeJam;

    private bool opened;
    #endregion

    #region properties
    public bool Opened
    {
        get { return opened; }
    }
    #endregion

    #region methods
    private void OnCollisionEnter(Collision collision)
    {
        // Knife collision
        if (collision.gameObject == knife)
        {
            // Open lid
            Rigidbody lidRig = transform.GetChild(2).GetComponent<Rigidbody>();
            lidRig.GetComponent<Collider>().enabled = true;
            lidRig.isKinematic = false;
            StartCoroutine(AddForce(lidRig));
            lidRig.transform.parent = null;

            // Change knife mesh
            collision.transform.GetComponent<MeshFilter>().mesh = knifeJam;
            GetComponent<AudioSource>().Play();
            opened = true;

            // Update
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");
        }
    }

    IEnumerator AddForce(Rigidbody rig)
    {
        Vector3 force = rig.transform.up * 300.0f * Time.deltaTime;
        yield return new WaitForFixedUpdate();
        force = rig.transform.up * 300.0f * Time.deltaTime;
        rig.AddForce(force, ForceMode.VelocityChange);
    }
    #endregion
}
