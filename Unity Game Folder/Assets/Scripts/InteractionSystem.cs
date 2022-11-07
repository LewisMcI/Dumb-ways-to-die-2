using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private RaycastHit hit;
    private bool interactHeldDown = false;
    public bool hasInteract;
    private GameObject pickedUpObject;
    private Vector3 pickedUpPos, pickedUpRot;
    private Transform pickedUpTransformParent;

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
                            {
                                PivotObject(hit.transform.gameObject);
                                interactHeldDown = true;
                            }
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
            if (Input.GetButtonUp("Interact"))
            {
                interactHeldDown = false;
            }
        }
    }

    void PickupObject(GameObject objectToPickup)
    {
        // Disable collision
        objectToPickup.GetComponent<Collider>().enabled = false;
        // Get position and rotation
        pickedUpTransformParent = objectToPickup.transform.parent;
        pickedUpPos = objectToPickup.transform.position;
        pickedUpRot = objectToPickup.transform.rotation.eulerAngles;
        // Attach to camera
        objectToPickup.transform.parent = Camera.main.transform;
        // Add offset position
        objectToPickup.transform.localPosition = Vector3.zero + Vector3.forward * 1f;
        // Make object face camera
        objectToPickup.transform.LookAt(Camera.main.transform);
        // Save object
        pickedUpObject = objectToPickup;
    }

    void DropObject(GameObject objectToDrop)
    {
        // Enalbe collision
        objectToDrop.GetComponent<Collider>().enabled = true;
        // Remove parent
        objectToDrop.transform.parent = pickedUpTransformParent;
        // Place back to original transform
        objectToDrop.transform.position = pickedUpPos;
        objectToDrop.transform.eulerAngles = pickedUpRot;
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
        // Setting up values for object
        pivotSettings.inUse = true;
        bool objState = pivotSettings.currentState;

        Quaternion startingAngle;
        Quaternion endingAngle;
        if (objState == false)
        {
            startingAngle = pivotSettings.GetStartingAngle;
            endingAngle = pivotSettings.endingAngle;
        }
        else
        {
            endingAngle = pivotSettings.GetStartingAngle;
            startingAngle = pivotSettings.endingAngle;
        }
        int smoothness = pivotSettings.smoothness;
        float time = pivotSettings.timeToOpen;

        for (float i = 0; i <= smoothness; i++)
        {
            pivotObj.transform.parent.localRotation = Quaternion.Lerp(startingAngle, endingAngle, i / smoothness);
            pivotSettings.currentState = !objState;
            yield return new WaitForSeconds(time/smoothness);
        }
        pivotSettings.inUse = false;
    }
}