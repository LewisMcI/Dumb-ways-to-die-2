using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapToaster : MonoBehaviour
{
    #region fields
    private GameObject knife;
    private bool removed;

    private Animator anim;

    public GameObject explosionVFX;

    [SerializeField]
    private Rigidbody tableRig;
    #endregion

    #region methods
    private void Awake()
    {
        knife = transform.GetChild(0).gameObject;
        tableRig.isKinematic = true;
    }

    public void Interact()
    {
        if (knife.transform.IsChildOf(transform))
        {
            GetComponent<AudioSource>().Play();
            explosionVFX.SetActive(true);
            GameManager.Instance.Player.GetComponent<PlayerController>().Die();
            tableRig.isKinematic = false;
        }

        // Reset tag
        transform.tag = "Untagged";
    }
    #endregion
}
