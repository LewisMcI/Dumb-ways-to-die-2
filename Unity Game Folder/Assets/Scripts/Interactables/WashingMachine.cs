using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WashingMachine : Interactable
{
    #region fields
    #endregion
    bool active = false;
    #region methods
    public override void Action()
    {
        active = !active;
        if (active == true)
        {
            Text = "Turn Off";
            AudioManager.Instance.PlayAudio("Washing Machine");
        }
        else
        {
            Text = "Turn On";
            AudioManager.Instance.StopAudio("Washing Machine");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!active) return;
        try
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0.1f, 0.5f), 200.0f, Random.Range(0.1f, 0.5f)));
        }
        catch
        {
            collision.gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0.1f, 0.5f), 2000.0f, Random.Range(0.1f, 0.5f)));
        }
    }
    #endregion
}
