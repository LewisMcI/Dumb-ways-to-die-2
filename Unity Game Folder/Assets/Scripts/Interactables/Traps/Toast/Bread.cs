using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Interactable
{
    #region fields
    [SerializeField]
    private GameObject knife, jam;
    [SerializeField]
    private Mesh breadJam;
    [SerializeField]
    private AudioSource spreadSFX, eatSFX;

    private bool toasted;
    #endregion

    #region properties
    public bool Toasted
    {
        set { toasted = value; }
    }
    #endregion

    #region methods
    public override void Action()
    {
        // Play fx
        eatSFX.Play();
        Camera.main.transform.Find("VFX").transform.Find("Eating Effect").GetComponent<ParticleSystem>().Play();

        // Disable
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 1.0f);

        // Play grab animation
        Animator anim = PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>();
        if (!anim.GetBool("Notepad"))
            anim.SetTrigger("Grab");

        // Update
        GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Knife collision
        if (collision.gameObject == knife && toasted && jam.GetComponent<Jam>().Opened)
        {
            // Play sfx
            spreadSFX.Play();

            // Change mesh
            GetComponent<MeshFilter>().mesh = breadJam;

            // Make interactable
            GetComponent<Bread>().Type = InteractableType.Other;
            GetComponent<Bread>().CanInteract = true;
            GetComponent<Bread>().Text = "Eat";
            GetComponent<Collider>().enabled = true;

            // Update
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Jam Toast");
        }
    }
    #endregion
}
