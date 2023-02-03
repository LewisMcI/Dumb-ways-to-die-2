using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Interactable
{
    #region fields
    [SerializeField]
    private Mesh breadJam;

    private GameObject bread;
    #endregion

    #region methods
    private void Update()
    {
        if (Text != "Place" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Toasted Bread")
            Text = "Place";
        else if (Text != "Spread" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Jam" && bread)
            Text = "Spread";
        else if (Text != "")
            Text = "";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Toasted Bread")
        {
            GameObject obj = collision.gameObject;
            InteractionSystem.Instance.DropObject();

            // Remove rigidbody
            Destroy(obj.GetComponent<Rigidbody>());
            // Attach to plate
            obj.transform.parent = transform;
            // Set transform
            obj.transform.localPosition = new Vector3(0.1f, 0.02f, 0.0f);
            obj.transform.localEulerAngles = new Vector3(0.0f, 90f, 0.0f);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // Make non-pickable
            obj.GetComponent<Bread>().CanInteract = false;
            // Reset text
            obj.GetComponent<Bread>().Text = "";
            // Disable collider
            obj.GetComponent<Collider>().enabled = false;
            GameManager.Instance.UpdateTaskCompletion("Make and Eat Toast");
            // Save
            bread = obj;
        }
        else if (collision.transform.name == "Jam" && bread)
        {
            // Play sfx
            AudioManager.Instance.PlayAudio("Spread");
            // Change mesh
            bread.GetComponent<MeshFilter>().mesh = breadJam;
            // Make interactable
            bread.GetComponent<Bread>().CanInteract = true;
            // Change text
            bread.GetComponent<Bread>().Text = "Eat";
            // Enable collider
            bread.GetComponent<Collider>().enabled = true;
            bread = null;
            GameManager.Instance.UpdateTaskCompletion("Make and Eat Toast");
        }
    }
    #endregion
}