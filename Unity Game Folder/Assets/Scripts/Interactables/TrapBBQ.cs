using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TrapBBQ : Interactable
{
    #region fields
    [SerializeField]
    private GameObject lighter;
    [SerializeField]
    private GameObject[] chickens;
    [SerializeField]
    private VisualEffect explosionVFX;
    private BoxCollider boxCollider;
    private bool connected;
    #endregion

    #region methods
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public override void Action()
    {
        switch(GameManager.Instance.taskManager.GetTask("Relax Outside").stepsComplete)
        {
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            case 5:

                break;
        }
    }

    public void Connect()
    {
        boxCollider.center = new Vector3(-0.08164346f, 0.5073186f, -0.5437614f);
        boxCollider.size = new Vector3(0.433622f, 0.06625993f, 0.2780444f);
        connected = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == lighter)
        {
            if (connected)
            {
                GameManager.Instance.taskManager.UpdateTaskCompletion("Relax Outside");
                GetComponent<Animator>().SetTrigger("Light");
                foreach (GameObject chicken in chickens)
                {
                    chicken.GetComponent<BBQChicken>().CanInteract = true;
                }
            }
            else
            {
                explosionVFX.Play();
            }
        }
    }
    #endregion
}
