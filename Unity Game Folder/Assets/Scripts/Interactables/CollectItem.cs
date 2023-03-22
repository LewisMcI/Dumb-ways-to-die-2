using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : Interactable
{
    public enum ObjectsToPickup
    {
        CarKeys = 0,
        Pills = 1,
        Rubbish = 2
    };

    [SerializeField]
    ObjectsToPickup objectToPickup;

    [SerializeField]
    AudioSource associatedAudio;

    public override void Action()
    {
        if (associatedAudio != null)
            associatedAudio.Play();
        switch (objectToPickup)
        {
            case ObjectsToPickup.CarKeys:
                GameManager.Instance.taskManager.UpdateTaskCompletion("Find Car Keys");
                break;
            case ObjectsToPickup.Pills:
                GameManager.Instance.taskManager.UpdateTaskCompletion("Take Meds");
                break;
            case ObjectsToPickup.Rubbish:
                GameManager.Instance.taskManager.UpdateTaskCompletion("Clean Kitchen");
                break;
            default:
                Debug.Log("This Object does not exist and cannot be picked up");
                break;
        }
        Destroy(gameObject);
    }
}
