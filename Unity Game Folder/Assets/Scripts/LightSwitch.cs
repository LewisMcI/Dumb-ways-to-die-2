using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightSwitch : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject[] lights;
    bool active = true;
    #endregion

    #region methods
    public void Switch()
    {
        // Switch
        active = !active;
        // Set lights
        foreach (GameObject light in lights)
            light.SetActive(active);

        // Player audio
        AudioManager.Instance.PlayAudio("Switch");
    }
    #endregion
}
