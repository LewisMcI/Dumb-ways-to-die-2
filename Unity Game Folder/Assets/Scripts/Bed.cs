using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    #region fields
    [SerializeField]
    private Task makeToastTask;
    [SerializeField]
    private Task brushTeethTask;

    private Vector3 startingPosition;
    #endregion

    #region methods
    private void Awake()
    {
        startingPosition = new Vector3(transform.position.x, transform.position.y + .15f, transform.transform.position.z + 1);
    }

    private void Update()
    {
        if (makeToastTask.taskComplete == true && brushTeethTask.taskComplete == true)
            text = "Sleep";
        else
            text = "";
    }

    public override void Action()
    {
        if (makeToastTask.taskComplete == true && brushTeethTask.taskComplete == true)
        {
            GameUI.Instance.ReverseBlink();
            StartCoroutine(GoToSleep());
        }
    }

    IEnumerator GoToSleep()
    {
        Vector3 currentPosition = GameManager.Instance.Player.transform.position;
        float time = 1.0f;
        float iterations = 100;
        for (float i = 0; i < iterations; i++)
        {
            GameManager.Instance.Player.transform.position = Vector3.Lerp(currentPosition, startingPosition, i / iterations);
            yield return new WaitForSeconds(time / iterations);
        }
        GameManager.Instance.Restart();
    }
    #endregion
}
