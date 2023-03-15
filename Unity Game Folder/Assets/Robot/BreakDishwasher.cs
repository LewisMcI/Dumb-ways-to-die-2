using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDishwasher : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject destroyer;
    [SerializeField]
    private FryingPan fryingPan;
    [SerializeField]
    private GameObject alertLight;
    [SerializeField]
    private Material redLight;
    private Material metal;
    [SerializeField]
    private GameObject smoke;
    [SerializeField]
    RobotBearTrap robotBearTrap;
    #endregion

    #region methods
    private void Start()
    {
        metal = alertLight.GetComponent<Renderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == destroyer)
        {
            transform.root.GetChild(0).GetComponent<Animator>().SetBool("Frying Pan", false);
            fryingPan.enabled = false;
            smoke.gameObject.SetActive(true);
            Material[] newMats = new Material[2];
            newMats[0] = metal;
            newMats[1] = redLight;
            alertLight.GetComponent<Renderer>().materials = newMats;
            Destroy(robotBearTrap);
        }
    }
    #endregion
}
