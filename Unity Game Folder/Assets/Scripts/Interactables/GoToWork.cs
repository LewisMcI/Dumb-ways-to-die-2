using System.Collections;
using UnityEngine;

public class GoToWork : Interactable
{
    #region fields
    #endregion

    #region methods

    private void Update()
    {
        if (GameManager.Instance.AllTasksComplete())
            Text = "Go To Work";
        else
            Text = "You can't Go To Work just yet";
    }

    public override void Action()
    {
        if (GameManager.Instance.AllTasksComplete())
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
        float time = 1.0f;
        yield return new WaitForSeconds(time);
        // TODO: Add Blink animation equivalent for coming home.
        GameManager.Instance.Restart();
    }
    #endregion
}
