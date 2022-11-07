using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private RaycastHit hit;
    private bool interactHeldDown = false;
    public bool hasInteract;

    void Update()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, 3f))
        {
            {
                string tag = hit.transform.gameObject.tag;
                switch (tag)
                {
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

    private IEnumerator HasInteract()
    {
        hasInteract = true;
        yield return new WaitForSeconds(0.1f);
        hasInteract = !hasInteract;
    }

    public void OnInteract()
    {
        if (hasInteract == false)
        {
            Debug.Log("Press");
            StartCoroutine(HasInteract());
        }
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
            Debug.Log(i / smoothness);
            pivotObj.transform.parent.localRotation = Quaternion.Lerp(startingAngle, endingAngle, i/ smoothness);
            pivotSettings.currentState = !objState;
            yield return new WaitForSeconds(time/smoothness);
        }
        pivotSettings.inUse = false;
    }
}