using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{

    private RaycastHit hit;
    private bool interactHeldDown = false;
    void Update()
    {
        //Debug.Log("Update");
        if (Input.GetButtonDown("Interact") && Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out hit, Mathf.Infinity))
        {
            interactHeldDown = true;
            Debug.Log("Interact");
            string tag = hit.transform.gameObject.tag;
            switch (tag)
            {
                case "Door":
                    OpenDoor(hit.transform.gameObject);
                    break;
                case "Pivot":
                    Debug.Log("Pivot");
                    PivotObject(hit.transform.gameObject);
                    break;
                default:
                    break;
            }
        }
        if (Input.GetButtonUp("Interact"))
        {
            interactHeldDown = false;
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
        Vector3 startingPos = pivotSettings.startingPos;
        Vector3 endingPos = pivotSettings.endingPos;
        Quaternion startingAngle = pivotSettings.startingAngle;
        Quaternion endingAngle = pivotSettings.endingAngle;

        while (interactHeldDown)
        {
            float newQuat = Quaternion.Dot(transform.rotation, pivotObj.transform.rotation);
            Debug.Log(newQuat);
            pivotObj.transform.position = Vector3.Lerp(startingPos, endingPos, newQuat);
            pivotObj.transform.rotation = Quaternion.Lerp(startingAngle, endingAngle, newQuat);
            yield return new WaitForSeconds(0.05f);
        }
    }
}