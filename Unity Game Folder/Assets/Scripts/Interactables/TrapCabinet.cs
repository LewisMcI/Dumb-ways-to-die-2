using System;
using System.Collections;
using UnityEngine;

public class TrapCabinet : Interactable
{
    #region fields
    private bool cut;

    private Animator anim;

    public bool kills = false;
    #endregion

    #region methods
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Text != "Cut" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Scissors")
            Text = "Cut";
        else if (Text != "Open")
            Text = "Open";
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Cut rope
        if (collision.transform.name == "Scissors" && !cut)
        {
            anim.SetTrigger("Cut");
            AudioManager.Instance.PlayAudio("Cut");
            cut = true;
        }
    }

    public override void Action()
    {
        if (InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Scissors")
            return;

        // Open and shoot if not cut
        if (cut)
            anim.SetTrigger("Open");
        else if (!cut && kills)
        {
            // Cast ray to see if player will be hit
            RaycastHit hit;
            if (Physics.BoxCast(transform.GetChild(0).GetChild(0).transform.position, new Vector3(0.3f, 0.2f, 0.6f), Vector3.right, out hit, Quaternion.identity, 3f))
            {
                if (hit.transform.tag == "Player" || hit.transform.tag == "MainCamera")
                    StartCoroutine(TriggerTrap());
            }
            anim.SetTrigger("Shoot Smoke");
        }
        else if (!cut && !kills)
        {
            anim.SetTrigger("Shoot Confetti");
        }

        GetComponent<Collider>().enabled = false;
    }

    IEnumerator TriggerTrap()
    {
        // Disable controls
        GameManager.Instance.EnableControls = false;
        // Reset player velocity and animation
        PlayerController.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("dirX", 0);
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetFloat("dirY", 0);
        float delay = 0.5f;
        PlayerController.Instance.Die(delay, true, PlayerController.SelectCam.bathroomCam);
        yield return new WaitForSeconds(delay);
        // Add backwards force
        PlayerController.Instance.AddRagdollForce(new Vector3(100, 10, 0));
        GetComponent<AudioSource>().Play();
    }
    #endregion
}
