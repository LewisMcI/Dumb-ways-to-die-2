using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.WSA;

public class InteractionSystem : MonoBehaviour
{
    private RaycastHit hit;
    public bool hasInteract;
    private GameObject pickedUpObject;

    public Task brushTeethTask;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && pickedUpObject)
        {
            // Holding scissors?
            bool interacted = false;
            if (pickedUpObject.name == "Scissors")
            {
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
                {
                    switch (hit.transform.tag)
                    {
                        case "Cabinet":
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                            if (Input.GetButtonDown("Interact"))
                            {
                                hit.transform.GetComponent<TrapCabinet>().Interact(true);
                                interacted = true;
                            }
                            break;
                    }
                }
            }
            else if (pickedUpObject.name == "SM_Item_Bread_01")
            {
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
                {
                    switch (hit.transform.tag)
                    {
                        case "Toaster":
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                            if (Input.GetButtonDown("Interact"))
                            {
                                pickedUpObject.name = "Toasted Bread";
                                hit.transform.GetComponent<TrapToaster>().Interact();
                                // Attach to toaster
                                pickedUpObject.transform.parent = hit.transform;
                                // Set transform
                                pickedUpObject.transform.localPosition = new Vector3(0.0f, 0.075f, 0.03f);
                                pickedUpObject.transform.localEulerAngles = new Vector3(90, 0, 0);
                                pickedUpObject.transform.localScale = new Vector3(1.0f, 0.8f, 1.2f);
                                // Reset
                                pickedUpObject.layer = LayerMask.GetMask("Default");
                                pickedUpObject = null;
                                interacted = true;
                            }
                            break;
                    }
                }
            }
            // Else drop
            if (!interacted)
            {
                DropObject(pickedUpObject);
            }
        }
        else
        {
            if (!GameManager.Instance.Player.GetComponent<PlayerController>().Dead && Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
            {
                switch (hit.transform.tag)
                {
                    case "Pickup":
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
                    case "Cabinet":
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                            hit.transform.GetComponent<TrapCabinet>().Interact(false);
                        break;
                    case "Toaster":
                        if (pickedUpObject && pickedUpObject.name == "SM_Item_Bread_01")
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        break;
                    default:
                        GameUI.Instance.DotAnim.SetBool("Interactable", false);
                        break;
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
        // Brush teeth interaction
        if (objectToPickup.name == "SM_Item_Toothbrush_01")
        {
            objectToPickup.GetComponent<AudioSource>().Play();
            objectToPickup.tag = "Untagged";
            
            if (brushTeethTask != null)
            {
                GameManager.Instance.CompletedTask(brushTeethTask);
            }
            else
            {
                Debug.Log("Task Uninitialized for " + gameObject.name);
            }
            return;
        }
        else if (objectToPickup.name == "Toasted Bread")
        {
            objectToPickup.GetComponent<AudioSource>().Play();
            objectToPickup.tag = "Untagged";
            if (brushTeethTask != null)
            {
                GameManager.Instance.CompletedTask(brushTeethTask);
            }
            else
            {
                Debug.Log("Task Uninitialized for " + gameObject.name);
            }
            return;
        }

        Rigidbody rig = (Rigidbody)objectToPickup.GetComponent(typeof(Rigidbody));
        // Remove Rigidbody
        if (rig != null)
            Destroy(rig);

        // Ignore raycasts
        objectToPickup.layer = LayerMask.GetMask("Ignore Raycast");
        // Disable collision
        objectToPickup.GetComponent<Collider>().enabled = false;
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
        // Enable collision
        objectToDrop.GetComponent<Collider>().enabled = true;
        // Add physics
        objectToDrop.AddComponent<Rigidbody>();

        // Remove parent
        objectToDrop.transform.parent = null;

        // Reset
        objectToDrop.layer = LayerMask.GetMask("Default");
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
            endingAngle = Quaternion.Euler(pivotSettings.endingAngle.x, pivotSettings.endingAngle.y, pivotSettings.endingAngle.z);
            startingPos = pivotSettings.GetStartingPos;
            endingPos = pivotSettings.endingPos;
        }
        else
        {
            endingAngle = pivotSettings.GetStartingAngle;
            startingAngle = Quaternion.Euler(pivotSettings.endingAngle.x, pivotSettings.endingAngle.y, pivotSettings.endingAngle.z);
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