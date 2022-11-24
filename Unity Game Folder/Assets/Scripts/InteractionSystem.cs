using System;
using System.Collections;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    #region fields
    private RaycastHit hit;

    [SerializeField]
    private Transform pickupTransform;
    private GameObject pickedUpObject;

    public static InteractionSystem Instance;
    #endregion

    #region properties
    public GameObject PickedUpObject
    {
        get { return pickedUpObject;}
    }
    #endregion

    #region methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp("Interact") && pickedUpObject)
        {
            CastRay();
            DropObject();
        }
        else if (Input.GetButtonDown("Throw") && pickedUpObject)
        {
            ThrowObject();
        }
        else
        {
            CastRay();
        }
    }

    private void FixedUpdate()
    {
        // Pick up physics
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

    private void CastRay()
    {
        if (!GameManager.Instance.Player.GetComponent<PlayerController>().Dead && Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
        {
            if (hit.transform.GetComponent<Interactable>())
            {
                switch (hit.transform.GetComponent<Interactable>().type)
                {
                    case Interactable.Type.Pickup:
                        GameUI.Instance.InteractText.text = hit.transform.GetComponent<Interactable>().text;
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                            PickupObject(hit.transform.gameObject);
                        break;
                    case Interactable.Type.Pivot:
                        GameUI.Instance.InteractText.text = hit.transform.GetComponent<Interactable>().text;
                        GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        if (Input.GetButtonDown("Interact"))
                            PivotObject(hit.transform.gameObject);
                        break;
                    case Interactable.Type.Trap:
                        if (hit.transform.GetComponent<Interactable>().text != "")
                        {
                            GameUI.Instance.InteractText.text = hit.transform.GetComponent<Interactable>().text;
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        }
                        if (Input.GetButtonDown("Interact") || pickedUpObject && Input.GetButtonUp("Interact"))
                            hit.transform.GetComponent<Interactable>().Action();
                        break;
                    case Interactable.Type.Other:
                        if (hit.transform.GetComponent<Interactable>().text != "")
                        {
                            GameUI.Instance.InteractText.text = hit.transform.GetComponent<Interactable>().text;
                            GameUI.Instance.DotAnim.SetBool("Interactable", true);
                        }
                        if (Input.GetButtonDown("Interact"))
                            hit.transform.GetComponent<Interactable>().Action();
                        break;
                }
            }
            else
            {
                if (GameUI.Instance.DotAnim.GetBool("Interactable"))
                    GameUI.Instance.DotAnim.SetBool("Interactable", false);
            }
        }
        else
        {
            if (GameUI.Instance.DotAnim.GetBool("Interactable"))
                GameUI.Instance.DotAnim.SetBool("Interactable", false);
        }
    }

    private void PickupObject(GameObject objectToPickup)
    {
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

    private void PivotObject(GameObject pivotObj)
    {
        PivotMultiple pivotMultiple = pivotObj.GetComponent<PivotMultiple>();
        if (pivotMultiple != null)
        {
            // If all pivotSettings are not in use.
            for (int i = 0; i < pivotMultiple.listOfPivotSettings.Length; i++)
            {
                if (pivotMultiple.listOfPivotSettings[i].inUse)
                    return;
            }
            // Start Coroutine for each
            foreach (PivotSettings pivotObject in pivotMultiple.listOfPivotSettings)
            {
                StartCoroutine(PivotObjectEnumerator(pivotObject.transform.GetChild(0).gameObject));
            }
        }
        else
        {
            StartCoroutine(PivotObjectEnumerator(pivotObj));
        }
    }

    IEnumerator PivotObjectEnumerator(GameObject pivotObj)
    {
        PivotSettings pivotSettings = pivotObj.GetComponent<PivotSettings>();
        if (pivotSettings == null)
        {
            pivotSettings = pivotObj.GetComponentInParent<PivotSettings>();
        }
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
    #endregion
}