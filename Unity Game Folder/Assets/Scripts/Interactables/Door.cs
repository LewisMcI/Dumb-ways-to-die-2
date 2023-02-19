using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField]
    PivotSettings openingForward;
    [SerializeField]
    PivotSettings openingBackward;
    private void Start()
    {
        StartCoroutine(fixDoors());
    }

    IEnumerator fixDoors()
    {
        yield return new WaitForSeconds(0.1f);
        if (openingForward.open == true)
        {
            openingBackward.StartingPos = openingForward.endingPos;
            openingBackward.StartingAngle = Quaternion.Euler(openingForward.endingAngle);

            Vector3 startingPos = openingForward.StartingPos;
            openingForward.StartingPos = openingForward.endingPos;
            openingForward.endingPos = startingPos;

            Quaternion startingAngle = openingForward.StartingAngle;
            openingForward.StartingAngle = Quaternion.Euler(openingForward.endingAngle);
            openingForward.endingAngle = startingAngle.eulerAngles;
        }
        else if (openingBackward.open == true)
        {
            openingForward.StartingPos = openingBackward.endingPos;
            openingForward.StartingAngle = Quaternion.Euler(openingBackward.endingAngle);

            Vector3 startingPos = openingBackward.StartingPos;
            openingBackward.StartingPos = openingBackward.endingPos;
            openingBackward.endingPos = startingPos;

            Quaternion startingAngle = openingBackward.StartingAngle;
            openingBackward.StartingAngle = Quaternion.Euler(openingBackward.endingAngle);
            openingBackward.endingAngle = startingAngle.eulerAngles;
        }
    }
    public override void Action()
    {
        // Something is moving
       if (openingBackward.inUse || openingForward.inUse)
        {
            return;
        }
        else if (openingForward.open == true)
        {
            OpenForward();
            return;
        }
        else if (openingBackward.open == true)
        {
            OpenBackward();
            return;
        }

        float dot = Quaternion.Dot(transform.parent.rotation.normalized, PlayerController.Instance.transform.rotation.normalized);
        Debug.Log(dot);
        // Infront of player
        if (dot > -0.7f  && dot < 0.7f)
        {
            OpenForward();
        }
        // Behind Player
        else
        {
            OpenBackward();
        }
    }

    private void OpenForward()
    {
        StartCoroutine(PivotObjectEnumerator(openingForward));
    }

    private void OpenBackward()
    {
        StartCoroutine(PivotObjectEnumerator(openingBackward));
    }


    IEnumerator PivotObjectEnumerator(PivotSettings pivotSettings)
    {
        // If object is in use, Ignores
        if (pivotSettings.inUse == true)
        {
            yield break;
        }

        pivotSettings.open = !pivotSettings.open;
        // Setting up values for object
        pivotSettings.inUse = true;
        bool usingMovement = pivotSettings.usingMovement;
        if (pivotSettings.open)
            Text = "Close";
        else
            Text = "Open";

        Quaternion startingAngle;
        Quaternion endingAngle;
        Vector3 startingPos;
        Vector3 endingPos;
        if (pivotSettings.open == true)
        {
            startingAngle = pivotSettings.StartingAngle;
            endingAngle = Quaternion.Euler(pivotSettings.endingAngle.x, pivotSettings.endingAngle.y, pivotSettings.endingAngle.z);
            startingPos = pivotSettings.StartingPos;
            endingPos = pivotSettings.endingPos;
            //AudioManager.Instance.PlayAudio("DoorOpen");
        }
        else
        {
            endingAngle = pivotSettings.StartingAngle;
            startingAngle = Quaternion.Euler(pivotSettings.endingAngle.x, pivotSettings.endingAngle.y, pivotSettings.endingAngle.z);
            endingPos = pivotSettings.StartingPos;
            startingPos = pivotSettings.endingPos;
            //AudioManager.Instance.PlayAudio("DoorClose");
        }
        int smoothness = pivotSettings.smoothness;
        float time = pivotSettings.timeToOpen;

        for (float i = 0; i <= smoothness; i++)
        {
            if (usingMovement)
            {
                transform.localPosition = Vector3.Lerp(startingPos, endingPos, i / smoothness);
            }
            transform.localRotation = Quaternion.Lerp(startingAngle, endingAngle, i / smoothness);
            pivotSettings.currentState = !pivotSettings.currentState;
            yield return new WaitForSeconds(time / smoothness);
        }
        pivotSettings.inUse = false;
    }
}
