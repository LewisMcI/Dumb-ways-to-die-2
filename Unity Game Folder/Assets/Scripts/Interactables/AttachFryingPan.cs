using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttachFryingPan : MonoBehaviour
{
    #region fields
    private FryingPanTrap trap;
    private GameObject fryingPan;
    private LineRenderer laser;
    #endregion

    #region methods
    private void Awake()
    {
        trap = transform.parent.parent.GetComponent<FryingPanTrap>();
        fryingPan = transform.GetChild(0).gameObject;
        laser = transform.parent.parent.GetChild(1).GetComponent<LineRenderer>();

        trap.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Frying Pan")
        {
            // Play fx
            GetComponent<AudioSource>().Play();
            // Attach
            Destroy(other.gameObject);
            fryingPan.SetActive(true);
            trap.enabled = true;
            laser.enabled = true;
        }
    }
    #endregion
}
