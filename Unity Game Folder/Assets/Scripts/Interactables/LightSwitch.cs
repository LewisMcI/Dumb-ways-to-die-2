using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightSwitch : Interactable
{
    #region fields
    [SerializeField]
    private GameObject[] lights;

    #endregion

    #region methods
    public override void Action()
    {
        // Set lights
        foreach (GameObject light in lights)
            light.SetActive(!light.activeSelf);
        // Switch scale
        transform.parent.localScale = (lights[0].activeSelf) ? new Vector3(1.0f, -1.0f, 1.0f) : Vector3.one;

        // Player audio
        AudioManager.Instance.PlayAudio("Switch");
    }
    #endregion
}
