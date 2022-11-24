using System;
using System.Collections;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private RaycastHit hit;
    public bool hasInteract;
    private GameObject pickedUpObject;
    private Vector3 startingPosition;

    public Task brushTeethTask;

    public Task makeToastTask;

    [SerializeField]
    private Transform pickupTransform;

    public GameObject bed;
    private string text = "Cut Rope";

    private void Awake()
    {
        startingPosition = new Vector3(bed.transform.position.x, bed.transform.position.y + .15f, bed.transform.position.z + 1);
    }

    private void Update()
    {
        if (Input.GetButtonUp("Interact") && pickedUpObject)
        {
            bool interacted = false;
            // Holding scissors?
            if (pickedUpObject.name == "Scissors")
            {
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
                {
                    switch (hit.transform.tag)
                    {
                        case "Cabinet":
                            if (!hit.transform.GetComponent<TrapCabinet>().Cut)
                            {
                                GameUI.Instance.InteractText.text = text;
                                text = "Open";
                            }
                            else
                            {
                                text = "Open";
                                GameUI.Instance.InteractText.text = text;
                            }
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                            hit.transform.GetComponent<TrapCabinet>().Interact(true);
                            interacted = true;
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
                            GameUI.Instance.InteractText.text = "Place Bread";
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                            // Rename
                            pickedUpObject.name = "Toasted Bread";
                            // Remove rigidbody
                            Destroy(pickedUpObject.GetComponent<Rigidbody>());
                            // Check trap
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
                            break;
                    }
                }
            }
            if (!interacted)
            {
                DropObject();
            }
        }
        else if (Input.GetButtonDown("Throw") && pickedUpObject)
        {
            ThrowObject();
        }
        else
        {
            if (!GameManager.Instance.Player.GetComponent<PlayerController>().Dead && Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
            {
                switch (hit.transform.tag)
                {
                    case "Pickup":
                        if (hit.transform.name == "SM_Item_Toothbrush_01")
                            GameUI.Instance.InteractText.text = "Brush Teeth";
                        else if (hit.transform.name == "Toasted Bread")
                            GameUI.Instance.InteractText.text = "Eat Toast";
                        else
                            GameUI.Instance.InteractText.text = "Pickup";
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                            PickupObject(hit.transform.gameObject);
                        break;
                    case "Door":
                        GameUI.Instance.InteractText.text = "Open";
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                            OpenDoor(hit.transform.gameObject);
                        break;
                    case "Pivot":
                        GameUI.Instance.InteractText.text = "Open";
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                            PivotObject(hit.transform.gameObject);
                        break;
                    case "Cabinet":
                        if (pickedUpObject && pickedUpObject.name == "Scissors")
                            GameUI.Instance.InteractText.text = text;
                        else
                            GameUI.Instance.InteractText.text = "Open";
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                            hit.transform.GetComponent<TrapCabinet>().Interact(false);
                        break;
                    case "Toaster":
                        if (pickedUpObject && pickedUpObject.name == "SM_Item_Bread_01")
                        {
                            GameUI.Instance.InteractText.text = "Place Bread";
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        }
                        break;
                    case "Bed":
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                        {
                            GameUI.Instance.ReverseBlink();
                            
                            StartCoroutine(GoToSleep());
                        }
                        break;
                    case "Light":
                        GameUI.Instance.InteractText.text = "Use";
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                            hit.transform.GetComponent<LightSwitch>().Switch();
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
    private void FixedUpdate()
    {
        if (pickedUpObject)
        {
            Vector3 desiredVelocity = (pickedUpObject.GetComponent<Renderer>()) ? Vector3.Normalize(pickupTransform.position - pickedUpObject.GetComponent<Renderer>().bounds.center) : Vector3.Normalize(pickupTransform.position - pickedUpObject.transform.position);
            float distance = (pickedUpObject.GetComponent<Renderer>()) ? Vector3.Distance(pickedUpObject.GetComponent<Renderer>().bounds.center, pickupTransform.position) : Vector3.Distance(pickedUpObject.transform.position, pickupTransform.position);
            // Distance before slowing down
            float stopDistance = 2f;
            // Speed to reach object
            float speed = 20f;
            // Get velocity
            desiredVelocity *= speed * (distance / stopDistance);
            // Set velocity
            pickedUpObject.GetComponent<Rigidbody>().velocity = desiredVelocity;
            // Face camera
            pickedUpObject.transform.LookAt(Camera.main.transform);
        }
    }

    private void PickupObject(GameObject objectToPickup)
    {
        // Brush teeth interaction
        if (objectToPickup.name == "SM_Item_Toothbrush_01")
        {
            AudioManager.Instance.PlayAudio("Brush Teeth");
            objectToPickup.tag = "Untagged";
            
            if (brushTeethTask != null)
            {
                GameManager.Instance.CompletedTask(brushTeethTask);
                if (makeToastTask.taskComplete == true && brushTeethTask.taskComplete == true)
                {
                    bed.tag = "Bed";
                }
            }
            return;
        }
        // Eat bread interaction
        else if (objectToPickup.name == "Toasted Bread")
        {
            AudioManager.Instance.PlayAudio("Eat");
            objectToPickup.tag = "Untagged";
            objectToPickup.GetComponent<Renderer>().enabled = false;

            if (brushTeethTask != null)
            {
                GameManager.Instance.CompletedTask(makeToastTask);
                if (makeToastTask.taskComplete == true && brushTeethTask.taskComplete == true)
                {
                    bed.tag = "Bed";
                }
            }
            return;
        }

        // Remove parent
        objectToPickup.transform.parent = null;
        // Add physics
        if (!objectToPickup.GetComponent<Rigidbody>())
            objectToPickup.AddComponent<Rigidbody>();
        // Remove gravity
        objectToPickup.GetComponent<Rigidbody>().useGravity = false;
        // Ignore raycasts
        objectToPickup.layer = 2;
        // Set position
        objectToPickup.transform.position = pickupTransform.position;

        // Save object
        pickedUpObject = objectToPickup;
    }

    private void DropObject()
    {
        // Reset
        pickedUpObject.layer = LayerMask.GetMask("Default");
        pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
        pickedUpObject = null;
    }

    private void ThrowObject()
    {
        // Add force
        pickedUpObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 15000f * Time.deltaTime);
        // Play sfx
        AudioManager.Instance.PlayAudio("Whoosh");

        // Reset
        pickedUpObject.layer = LayerMask.GetMask("Default");
        pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
        pickedUpObject = null;
    }

    private void OpenDoor(GameObject door)
    {
        door.SetActive(false);
    }

    private void PivotObject(GameObject pivotObj)
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

    IEnumerator GoToSleep()
    {
        Vector3 currentPosition = transform.position;
        float time = 1.0f;
        float iterations = 100;
        for (float i = 0; i < iterations; i++)
        {
            transform.position = Vector3.Lerp(currentPosition, startingPosition, i / iterations);
            yield return new WaitForSeconds(time / iterations);
        }
        GameManager.Instance.Restart();
    }
}