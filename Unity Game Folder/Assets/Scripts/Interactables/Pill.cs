using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : Interactable
{
    #region methods
    public override void Action()
    {
        // Play audio
        GetComponent<AudioSource>().Play();
        // Disable renderer
        GetComponent<MeshRenderer>().enabled = false;
        // Disable interaciton
        CanInteract = false;
        Destroy(gameObject, 0.5f);
        GameManager.Instance.taskManager.UpdateTaskCompletion("Take Meds");
    }
    #endregion
}
