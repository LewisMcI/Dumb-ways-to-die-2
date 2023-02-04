using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightSwitch : Interactable
{
    #region fields
    [SerializeField]
    private GameObject[] lights;
    [SerializeField]
    bool active;
    #endregion

    #region methods
    public override void Action()
    {
        // Switch
        active = !active;
        // Set lights
        foreach (GameObject light in lights)
            light.SetActive(active);
        // Switch scale
        transform.parent.localScale = (active) ? new Vector3(1.0f, -1.0f, 1.0f) : Vector3.one;

        // Player audio
        AudioManager.Instance.PlayAudio("Switch");
    }
    #endregion
}
