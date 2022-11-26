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
        if (text != "Place" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Toasted Bread")
            text = "Place";
        else if (text != "Spread" && InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Jam")
            text = "Spread";
        else if (text != "")
            text = "";
    }

    public override void Action()
    {
        if (InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Toasted Bread")
        {
            GameObject obj = InteractionSystem.Instance.PickedUpObject;

            // Remove rigidbody
            Destroy(obj.GetComponent<Rigidbody>());
            // Attach to plate
            obj.transform.parent = transform;
            // Set transform
            obj.transform.localPosition = new Vector3(0.1f, 0.02f, 0.0f);
            obj.transform.localEulerAngles = new Vector3(0.0f, 90f, 0.0f);
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            // Make non-pickable
            obj.GetComponent<Bread>().type = Type.None;
            // Reset text
            obj.GetComponent<Bread>().text = "";
            // Disable collider
            obj.GetComponent<Collider>().enabled = false;

            // Save
            bread = obj;
        }
        if (InteractionSystem.Instance.PickedUpObject && InteractionSystem.Instance.PickedUpObject.name == "Jam")
        {
            // Change mesh
            bread.GetComponent<MeshFilter>().mesh = breadJam;
            // Make interactable
            bread.GetComponent<Bread>().type = Type.Other;
            // Change text
            bread.GetComponent<Bread>().text = "Eat";
            // Enable collider
            bread.GetComponent<Collider>().enabled = true;
        }
    }
    #endregion
}