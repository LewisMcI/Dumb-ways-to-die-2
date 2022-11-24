using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
        if (InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Bread")
            text = "Drop";
        else
            text = "";
    }

    public override void Action()
    {
        if (InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Bread")
        {
            GameObject obj = InteractionSystem.Instance.PickedUpObject;
            // If knife not taken out kill player
            if (knife.transform.IsChildOf(transform))
                StartCoroutine(KillPlayer());
            GetComponent<Animator>().SetTrigger("Activate");

            // Rename
            obj.name = "Toasted Bread";
            obj.GetComponent<Bread>().type = Type.Other;
            obj.GetComponent<Bread>().text = "Eat";

            // Remove rigidbody
            Destroy(obj.GetComponent<Rigidbody>());

            // Attach to toaster
            obj.transform.parent = transform;
            // Set transform
            obj.transform.localPosition = new Vector3(0.0f, 0.075f, 0.03f);
            obj.transform.localEulerAngles = new Vector3(90, 0, 0);
            obj.transform.localScale = new Vector3(1.0f, 0.8f, 1.2f);
        }
    }

    IEnumerator KillPlayer()
    {
        GameManager.Instance.Player.GetComponent<PlayerController>().Die();
        yield return new WaitForSeconds(0.25f);
        AudioManager.Instance.PlayAudio("Explosion");
        explosionVFX.SetActive(true);
        tableRig.isKinematic = false;
    }
    #endregion
}
