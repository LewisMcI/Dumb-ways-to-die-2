using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WashingMachine : Interactable
{
    #region fields
    [SerializeField]
    private GameObject washingMachine;
    [SerializeField]
    private BoxCollider trigger;
    #endregion

    #region methods
    public override void Action()
    {
        trigger.isTrigger = !trigger.isTrigger;
        washingMachine.SetActive(!washingMachine.activeSelf);
        if (washingMachine.activeSelf == true)
        {
            text = "Turn Off";
            AudioManager.Instance.PlayAudio("Washing Machine");
        }
        else
        {
            text = "Turn On";
            AudioManager.Instance.StopAudio("Washing Machine");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collision");
        if (washingMachine.activeSelf)
        {
            try
            {
                if (Random.Range(0, 10) > 5)
                    other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, -10));
                else
                    other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 20, 10));
            }
            catch { }
        }
    }
    #endregion
}
