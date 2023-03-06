using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBattery : Interactable
{
    #region fields
    [SerializeField]
    private Clothes clothes;
    [SerializeField]
    private ParticleSystem[] effects;
    #endregion

    #region methods
    public override void Action()
    {
        // Disable electricity
        foreach (Clothing clothing in clothes.Clothings)
        {
            clothing.Electric = false;
        }
        // Disable effects
        foreach (ParticleSystem effect in effects)
        {
            effect.Stop();
        }
        // Stop sfx
        transform.parent.GetComponent<AudioSource>().Stop();
        // Play animation
        transform.parent.GetComponent<Animator>().SetTrigger("Trigger");
        // Play switch sfx
        AudioManager.Instance.PlayAudio("Switch");
    }
    #endregion
}