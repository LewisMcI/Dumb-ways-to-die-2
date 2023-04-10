using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BBQWire : Interactable
{
    #region fields
    [SerializeField]
    private TrapBBQ bbq;
    [SerializeField]
    private VisualEffect gasVFX;
    #endregion

    #region methods
    public override void Action()
    {
        bbq.GetComponent<Animator>().SetTrigger("Connect");
        bbq.Connect();
        gasVFX.Stop();
        GetComponent<AudioSource>().Play();
    }
    #endregion
}
