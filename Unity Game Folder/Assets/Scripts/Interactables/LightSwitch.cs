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
    private MeshRenderer[] bulbs;
    [SerializeField]
    private Material emissionOn, emissionOff;

    private bool isOn = true;
    #endregion

    #region methods
    private void Awake()
    {
        Action();
    }

    public override void Action()
    {
        // Switch
        isOn = !isOn;

        // Set lights
        foreach (GameObject light in lights)
            light.SetActive(isOn);
        // Set bulbs
        foreach (MeshRenderer bulb in bulbs)
            bulb.material = (isOn) ? emissionOn : emissionOff;

        // Switch scale
        transform.parent.localScale = (lights[0].activeSelf) ? new Vector3(1.0f, -1.0f, 1.0f) : Vector3.one;

        // Player audio
        AudioManager.Instance.PlayAudio("Switch");
    }
    #endregion
}
