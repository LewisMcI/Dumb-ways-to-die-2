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
    float delay = 1.0f;
    [SerializeField]
    private GameObject tv;

    [SerializeField]
    private VideoClip clip;
    #endregion

    #region methods
    private void Update()
    {
        if (GameManager.Instance.taskManager.AllTasksComplete())
            Text = "Watch TV";
        else
            Text = "I'm too stressed right now.";
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
        if (!GameManager.Instance.taskManager.AllTasksComplete())
            return;
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
        PlayerController.Instance.ThrowPlayerInDirection(new Vector3(0, 100, 0), delay, SelectCam.couchCam);
        yield return new WaitForSeconds(delay);
        transition = false;
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

        if (tv.GetComponent<WatchTV>() && tv.GetComponent<WatchTV>().enabled)
        {
            if (!tv.transform.GetChild(0).GetComponent<VideoPlayer>().isPlaying)
            {
                tv.transform.GetChild(0).GetComponent<VideoPlayer>().clip = clip;
                tv.transform.GetChild(0).GetComponent<VideoPlayer>().Play();
                tv.transform.GetChild(1).gameObject.SetActive(true);
                tv.GetComponent<WatchTV>().CanInteract = false;
            }
            else
            {
                tv.transform.GetChild(1).gameObject.SetActive(true);
            }
            if (tv.GetComponent<WatchTV>().trap)
                GameManager.Instance.taskManager.UpdateTaskCompletion("Sit and Watch TV");
            tv.GetComponent<WatchTV>().enabled = false;
        }
        else if (tv.GetComponent<WatchTV>() && !tv.GetComponent<WatchTV>().enabled && !tv.transform.GetChild(1).gameObject.activeSelf)
            tv.transform.GetChild(1).gameObject.SetActive(true);

        GameManager.Instance.EnableControls = false;
        GameManager.Instance.EnableCamera = false;

        GameManager.Instance.TransitionDay();
    }

    IEnumerator UnsetSitting()
    {
        Debug.Log("Unset");
        transition = true;
        GameObject tv = GameObject.Find("Tv");
        if (tv.transform.GetChild(1).gameObject.activeSelf)
            tv.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        transition = false;
        sitting = false;
        tv.transform.GetChild(0).GetComponent<VideoPlayer>().Stop();

        GameManager.Instance.EnableControls = true;
        GameManager.Instance.EnableCamera = true;
        Destroy(this);
    }
    #endregion
}
