using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bed : Interactable
{
    #region fields

    private Vector3 startingPosition;
    #endregion

    #region methods
    private void Awake()
    {
        startingPosition = new Vector3(transform.position.x, transform.position.y - .1f, transform.position.z);
    }

    private void Update()
    {
        if (GameManager.Instance.taskManager.AllTasksComplete())
            Text = "Sleep";
        else
            Text = "You can't sleep just yet";
    }

    public override void Action()
    {
        if (GameManager.Instance.taskManager.AllTasksComplete())
        {
            GameUI.Instance.ReverseBlink();
            StartCoroutine(GoToSleep());
        }
    }

    IEnumerator GoToSleep()
    {
        Camera.main.GetComponent<CameraController>().FollowHeadTime = 0.0f;
        Vector3 currentPosition = PlayerController.Instance.transform.position;
        float time = 1.0f;
        float iterations = 100;
        for (float i = 0; i < iterations; i++)
        {
            PlayerController.Instance.transform.position = Vector3.Lerp(currentPosition, startingPosition, i / iterations);
            yield return new WaitForSeconds(time / iterations);
        }

        GameManager.Instance.taskManager.ResetAllTraps();
        // TODO: FIX this!!!

        // Advance level
        GameSettings.Instance.currLevel++;
        SceneManager.LoadScene("Loading");
    }
    #endregion
}
