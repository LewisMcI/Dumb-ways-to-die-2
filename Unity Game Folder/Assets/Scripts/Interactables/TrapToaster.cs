using System;
using System.Collections;
using UnityEngine;

public class TrapToaster : Interactable
{
    #region fields
    [SerializeField]
    private GameObject bread;
    [SerializeField]
    private GameObject knife;
    [SerializeField]
    private GameObject jam;
    [SerializeField]
    private GameObject plate;
    private bool placed;

    [SerializeField]
    private GameObject explosionVFX;

    [SerializeField]
    private Rigidbody tableRig;
    #endregion

    #region methods
    private void Awake()
    {
        knife = transform.GetChild(2).gameObject;
        tableRig.isKinematic = true;
    }

    private void Update()
    {
        if (text != "Drop" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Bread")
            text = "Drop";
        else if (text != "")
            text = "";

        // Change toast color
        if (placed)
            bread.GetComponent<Renderer>().material.color = Color.Lerp(bread.GetComponent<Renderer>().material.color, new Color32(145, 126, 98, 255), 0.5f * Time.deltaTime);
    }

    public override void Action()
    {
        if (InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Bread")
        {
            GameObject obj = InteractionSystem.Instance.PickedUpObject;
            // If knife not taken out kill player
            if (knife.transform.IsChildOf(transform))
                StartCoroutine(KillPlayer());
            else
                StartCoroutine(ChangeBread());

            // Remove rigidbody
            Destroy(obj.GetComponent<Rigidbody>());

            // Attach to toaster
            obj.transform.parent = transform;
            // Change type
            obj.GetComponent<Bread>().type = Type.None;
            // Change text
            obj.GetComponent<Bread>().text = "";
            // Set transform
            obj.transform.localPosition = new Vector3(0.0f, 0.075f, 0.03f);
            obj.transform.localEulerAngles = new Vector3(90f, 0.0f, 0.0f);
            obj.transform.localScale = new Vector3(1.0f, 0.8f, 1.2f);
        }
    }

    IEnumerator KillPlayer()
    {
        // Disable controls
        GameManager.Instance.EnableControls = false;
        GetComponent<Animator>().SetTrigger("Explode");
        float delay = 0.5f;
        PlayerController.Instance.Die(PlayerController.SelectCam.toasterCam, delay);
        yield return new WaitForSeconds(delay);
        // Add backwards force
        PlayerController.Instance.AddRagdollForce(new Vector3(100, 10, 0));
        Destroy(bread);
        AudioManager.Instance.PlayAudio("Explosion");
        explosionVFX.SetActive(true);
        tableRig.isKinematic = false;
    }

    IEnumerator ChangeBread()
    {
        placed = true;
        GetComponent<Animator>().SetTrigger("Activate");
        yield return new WaitForSeconds(2.0f);
        // Rename
        bread.name = "Toasted Bread";
        // Change type
        bread.GetComponent<Bread>().type = Type.Pickup;
        // Change text
        bread.GetComponent<Bread>().text = "Pick Up";
        // Move slightly up
        bread.transform.position += Vector3.up * 0.025f;
        // Reset
        placed = false;
    }
    #endregion
}
