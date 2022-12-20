using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    #region fields

    private Vector3 startingPosition;
    #endregion

    #region methods
    private void Awake()
    {
        startingPosition = new Vector3(transform.position.x, transform.position.y +.75f, transform.position.z - 4);
    }

    private void Update()
    {
        if (GameManager.Instance.AllTasksComplete())
            text = "Sleep";
        else
            text = "You can't sleep just yet";
    }

    public override void Action()
    {
        if (GameManager.Instance.AllTasksComplete())
        {
            GameUI.Instance.ReverseBlink();
            StartCoroutine(GoToSleep());
        }
    }

    IEnumerator GoToSleep()
    {
        Vector3 currentPosition = PlayerController.Instance.transform.position;
        float time = 1.0f;
        float iterations = 100;
        for (float i = 0; i < iterations; i++)
        {
            PlayerController.Instance.transform.position = Vector3.Lerp(currentPosition, startingPosition, i / iterations);
            yield return new WaitForSeconds(time / iterations);
        }
        GameManager.Instance.Restart();
    }
    #endregion
}
