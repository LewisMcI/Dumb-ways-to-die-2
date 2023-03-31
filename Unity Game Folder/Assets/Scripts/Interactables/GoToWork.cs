using System.Collections;
using UnityEngine;

public class GoToWork : Interactable
{
    #region fields
    #endregion

    #region methods

    private void Update()
    {
        if (GameManager.Instance.taskManager.AllTasksComplete())
            Text = "Leave To Work";
        else
            Text = "It's not time for work yet.";
    }

    public override void Action()
    {
        if (GameManager.Instance.taskManager.AllTasksComplete())
        {
            // TODO: Add ReverseBlink animation equivalent for leaving to work.
            GameUI.Instance.ReverseBlink();
            StartCoroutine(LeaveForWork());
        }
    }

    IEnumerator LeaveForWork()
    {
        GameManager.Instance.EnableControls = false;
        Vector3 currentPosition = PlayerController.Instance.transform.position;
        float time = 2.0f;
        yield return new WaitForSeconds(time);
        // TODO: Add Blink animation equivalent for coming home.
        Transform pcTransform = PlayerController.Instance.transform;
        pcTransform.rotation = Quaternion.Euler(pcTransform.rotation.x, 90, pcTransform.rotation.z);
        GameManager.Instance.TransitionDay();
        GameManager.Instance.EnableControls = true;
    }
    #endregion
}
