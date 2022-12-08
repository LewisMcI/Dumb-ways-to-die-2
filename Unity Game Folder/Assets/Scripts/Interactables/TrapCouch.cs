using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TrapCouch : Interactable
{
    #region fields
    [SerializeField]
    private bool spring;

    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool transition, sitting;

    [SerializeField]
    private VideoClip[] clips;
    #endregion

    #region methods
    private void Update()
    {
        // Trigger get up
        if (sitting && Input.GetButtonDown("Interact"))
        {
            StartCoroutine(UnsetSitting());
        }

        if (transition)
        {
            float speed = 4.0f;
            // Sit down transition
            if (!sitting)
            {
                // Change player orientation
                PlayerController.Instance.transform.rotation = Quaternion.Lerp(PlayerController.Instance.transform.rotation, transform.rotation, speed * Time.deltaTime);
                // Change player position
                PlayerController.Instance.transform.position = Vector3.Lerp(PlayerController.Instance.transform.position, transform.GetChild(0).position, speed * Time.deltaTime);
                // Disable collision
                PlayerController.Instance.GetComponent<Rigidbody>().isKinematic = true;
            }
            // Get up transition
            else
            {
                // Reset
                PlayerController.Instance.transform.rotation = Quaternion.Lerp(PlayerController.Instance.transform.rotation, originalRot, speed * Time.deltaTime);
                PlayerController.Instance.transform.position = Vector3.Lerp(PlayerController.Instance.transform.position, originalPos, speed * Time.deltaTime);
                PlayerController.Instance.GetComponent<Rigidbody>().isKinematic = false;
                PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Sit", false);
                GameManager.Instance.EnableControls = true;
                GameManager.Instance.EnableCamera = true;
            }
        }
    }

    public override void Action()
    {
        // Disable controls
        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;
        PlayerController.Instance.transform.GetChild(0).GetComponent<Animator>().SetBool("Sit", true);
        // Save transform
        originalPos = PlayerController.Instance.transform.position;
        originalRot = PlayerController.Instance.transform.rotation;

        if (spring)
            StartCoroutine(TriggerTrap());
        else
            StartCoroutine(SetSitting());
    }

    IEnumerator TriggerTrap()
    {
        transition = true;
        float delay = 1f;
        PlayerController.Instance.GetComponent<PlayerController>().Die(PlayerController.SelectCam.couchCam, delay);
        yield return new WaitForSeconds(delay);
        transition = false;
        // Add upward force
        PlayerController.Instance.AddRagdollForce(new Vector3(0, 100, 0));
        // Enable collision
        PlayerController.Instance.GetComponent<Rigidbody>().isKinematic = false;
        // Play animation
        transform.parent.parent.GetComponent<Animator>().SetTrigger("Activate");
    }

    IEnumerator SetSitting()
    {
        transition = true;
        yield return new WaitForSeconds(1.0f);
        transition = false;
        sitting = true;

        GameObject tv = GameObject.Find("Tv");
        if (tv.GetComponent<WatchTV>() && tv.GetComponent<WatchTV>().enabled)
        {
            if (!tv.transform.GetChild(0).GetComponent<VideoPlayer>().isPlaying)
            {
                int random = Random.Range(0, 4);
                switch (random)
                {
                    case 0:
                        tv.transform.GetChild(0).GetComponent<VideoPlayer>().clip = clips[0];
                        break;
                    case 1:
                        tv.transform.GetChild(0).GetComponent<VideoPlayer>().clip = clips[1];
                        break;
                    case 2:
                        tv.transform.GetChild(0).GetComponent<VideoPlayer>().clip = clips[2];
                        break;
                    case 3:
                        tv.transform.GetChild(0).GetComponent<VideoPlayer>().clip = clips[3];
                        break;
                }
                tv.transform.GetChild(0).GetComponent<VideoPlayer>().Play();
                tv.transform.GetChild(1).gameObject.SetActive(true);
                tv.GetComponent<WatchTV>().type = Type.None;
            }
            else
            {
                tv.transform.GetChild(1).gameObject.SetActive(true);
            }
            if (tv.GetComponent<WatchTV>().trap)
                GameManager.Instance.SetTaskComplete("Watch TV");
            tv.GetComponent<WatchTV>().enabled = false;
        }
        else if (tv.GetComponent<WatchTV>() && !tv.GetComponent<WatchTV>().enabled && !tv.transform.GetChild(1).gameObject.activeSelf)
            tv.transform.GetChild(1).gameObject.SetActive(true);
    }

    IEnumerator UnsetSitting()
    {
        transition = true;
        GameObject tv = GameObject.Find("Tv");
        if (tv.transform.GetChild(1).gameObject.activeSelf)
            tv.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        transition = false;
        sitting = false;
    }
    #endregion
}
