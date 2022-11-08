using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private bool trapTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        EventManager.onClicked += KillPlayer;
    }

    private void OnTriggerExit(Collider other)
    {
        EventManager.onClicked -= KillPlayer;
    }

    public GameObject shotgun;

    public GameObject confetti;

    private void KillPlayer()
    {
        if (!trapTriggered)
        {
            trapTriggered = true;
            Debug.Log("Kill Player");
            StartCoroutine(PivotObjectEnumerator(shotgun));
        }
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
            pivotObj.transform.localRotation = Quaternion.Lerp(startingAngle, endingAngle, i / smoothness);
            pivotSettings.currentState = !objState;
            yield return new WaitForSeconds(time / smoothness);
        }
        pivotSettings.inUse = false;
        confetti.SetActive(true);

        yield return new WaitForSeconds(.25f);
        confetti.GetComponent<ParticleSystem>().Stop();
    }
}
