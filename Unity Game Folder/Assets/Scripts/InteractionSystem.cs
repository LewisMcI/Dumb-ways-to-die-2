using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private RaycastHit hit;
    private GameObject pickedUpObject;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && pickedUpObject)
        {
            DropObject(pickedUpObject);
        }
        else
        {
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
            {
                {
                    switch (hit.transform.tag)
                    {
                        case "Interactable":
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                            if (Input.GetButtonDown("Interact"))
                                PickupObject(hit.transform.gameObject);
                            break;
                        case "Door":
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                            if (Input.GetButtonDown("Interact"))
                                OpenDoor(hit.transform.gameObject);
                            break;
                        case "Pivot":
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                            if (Input.GetButtonDown("Interact"))
                                PivotObject(hit.transform.gameObject);
                            break;
                        default:
                            GameUI.Instance.DotAnim.SetBool("Interactable", false);
                            break;
                    }
                }
            }
            else
            {
                GameUI.Instance.DotAnim.SetBool("Interactable", false);
            }
        }
    }

    void PickupObject(GameObject objectToPickup)
    {
        // Disable collision
        objectToPickup.GetComponent<Collider>().enabled = false;
        // Remove physics
        if (objectToPickup.TryGetComponent(typeof(Rigidbody), out Component component))
            Destroy(objectToPickup.GetComponent<Rigidbody>());
        // Set to ignore player layer
        objectToPickup.layer = 7;

        // Attach to camera
        objectToPickup.transform.parent = Camera.main.transform;
        // Add offset position
        objectToPickup.transform.localPosition = Vector3.forward * 1.25f;
        // Make object face camera
        objectToPickup.transform.LookAt(Camera.main.transform);

        // Save object
        pickedUpObject = objectToPickup;
    }

    void DropObject(GameObject objectToDrop)
    {
        // Enable collision
        objectToDrop.GetComponent<Collider>().enabled = true;
        // Add physics
        objectToDrop.AddComponent<Rigidbody>();

        // Remove parent
        objectToDrop.transform.parent = null;

        // Reset
        pickedUpObject = null;
    }

    void OpenDoor(GameObject door)
    {
        door.SetActive(false);
    }

    void PivotObject(GameObject pivotObj)
    {
        StartCoroutine(PivotObjectEnumerator(pivotObj));
    }

    IEnumerator PivotObjectEnumerator(GameObject pivotObj)
    {
        PivotSettings pivotSettings = pivotObj.GetComponentInParent<PivotSettings>();
        // If object is in use, Ignores
        if (pivotSettings.inUse == true)
        {
            yield break;
        }
        pivotSettings.open = !pivotSettings.open;
        // Setting up values for object
        pivotSettings.inUse = true;
        bool objState = pivotSettings.currentState;
        bool usingMovement = pivotSettings.usingMovement;

        Quaternion startingAngle;
        Quaternion endingAngle;
        Vector3 startingPos;
        Vector3 endingPos;
        if (objState == false)
        {
            startingAngle = pivotSettings.GetStartingAngle;
            endingAngle = pivotSettings.endingAngle;
            startingPos = pivotSettings.GetStartingPos;
            endingPos = pivotSettings.endingPos;
        }
        else
        {
            endingAngle = pivotSettings.GetStartingAngle;
            startingAngle = pivotSettings.endingAngle;
            endingPos = pivotSettings.GetStartingPos;
            startingPos = pivotSettings.endingPos;
        }
        int smoothness = pivotSettings.smoothness;
        float time = pivotSettings.timeToOpen;

        for (float i = 0; i <= smoothness; i++)
        {
            if (usingMovement)
            {
                pivotObj.transform.parent.localPosition = Vector3.Lerp(startingPos, endingPos, i / smoothness);
            }
            pivotObj.transform.parent.localRotation = Quaternion.Lerp(startingAngle, endingAngle, i / smoothness);
            pivotSettings.currentState = !objState;
            yield return new WaitForSeconds(time/smoothness);
        }
        pivotSettings.inUse = false;
    }
}