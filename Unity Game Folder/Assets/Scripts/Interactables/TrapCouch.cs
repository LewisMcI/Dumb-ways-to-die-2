using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCouch : Interactable
{
    #region fields
    private bool sat;
    #endregion

    #region methods
    private void Update()
    {
        if (sat)
        {
            float speed = 4.0f;
            // Change player orientation
            PlayerController.Instance.transform.rotation = Quaternion.Lerp(PlayerController.Instance.transform.rotation, transform.rotation, speed * Time.deltaTime);
            // Change player position
            PlayerController.Instance.transform.position = Vector3.Lerp(PlayerController.Instance.transform.position, transform.GetChild(0).position, speed * Time.deltaTime);
            // Disable collision
            PlayerController.Instance.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public override void Action()
    {
        sat = true;

        // Disable controls
        GameManager.Instance.EnableControls = false;
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Sit", true);
        StartCoroutine(TriggerTrap());
    }

    IEnumerator TriggerTrap()
    {
        float delay = 1f;
        PlayerController.Instance.GetComponent<PlayerController>().Die(PlayerController.SelectCam.couchCam, delay);
        yield return new WaitForSeconds(delay);
        // Add upward force
        PlayerController.Instance.AddRagdollForce(new Vector3(0, 100, 0));
        // Enable collision
        PlayerController.Instance.GetComponent<Rigidbody>().isKinematic = false;
        // Play animation
        transform.parent.GetComponent<Animator>().SetTrigger("Activate");

        Debug.Log(GameManager.Instance.EnableControls);
        Debug.Log(PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().GetBool("Sit"));

        sat = false;
    }
    #endregion
}
