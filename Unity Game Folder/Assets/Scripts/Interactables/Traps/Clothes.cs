using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clothes : MonoBehaviour
{
    #region fields
    [SerializeField]
    private Clothing[] clothings;
    [SerializeField]
    private bool lining;
    #endregion

    #region properties
    public Clothing[] Clothings
    { 
        get { return clothings; } 
    }

    public bool Lining
    {
        get { return lining; }
    }
    #endregion

    #region methods
    public void Check()
    {
        // Mark task as complete if all clothes collected
        GameManager.Instance.taskManager.UpdateTaskCompletion("Get Clothes");
    }

    public void EnableVFX()
    {
        StartCoroutine(PlayVFX());
    }

    IEnumerator PlayVFX()
    {
        foreach (ParticleSystem particleEffect in transform.parent.GetComponentsInChildren<ParticleSystem>())
        {
            particleEffect.Play();
        }
        yield return new WaitForSeconds(0.75f);
        foreach (ParticleSystem particleEffect in transform.parent.GetComponentsInChildren<ParticleSystem>())
        {
            particleEffect.Stop();
        }
    }
    #endregion
}
