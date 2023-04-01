using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCereal : Interactable
{
    #region fields
    [SerializeField]
    private GameObject milk, cereal;
    private int completion;
    #endregion

    #region methods
    public override void Action()
    {
        if (completion >= 2)
        {
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cereal");
            // Play effects
            GetComponent<AudioSource>().Play();
            Camera.main.transform.Find("VFX").transform.Find("Eating Effect").GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Add milk
        if (collision.gameObject == milk)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Destroy(collision.gameObject);
            AudioManager.Instance.PlayAudio("Cloth");
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cereal");
            completion++;
        }
        // Add cereal
        else if (collision.gameObject == cereal && completion > 0)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(collision.gameObject);
            AudioManager.Instance.PlayAudio("Cloth");
            GameManager.Instance.taskManager.UpdateTaskCompletion("Make Cereal");
            Text = "Eat";
            completion++;
        }
    }
    #endregion
}
