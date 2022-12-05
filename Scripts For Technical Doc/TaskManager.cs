using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TextMeshPro> notepadText;
    public void CompletedTask(Task task)
    {
        string taskName = task.taskName;
        bool changed = false;
        foreach (TextMeshPro text in notepadText)
        {
            if (text.text.Replace(" ", "") == taskName.Replace(" ", ""))
            {
                GameUI.Instance.NotifyAnim.SetTrigger("Notify");
                text.text = "<s>" + text.text + "</s>";
                changed = true;
                task.taskComplete = true;
                continue;
            }
        }
        if (!changed)
        {
            Debug.Log("Invalid Task Complete");
        }

    }

/*    void TasksComplete()
    {
        if (brushTeethTask != null)
        {
            GameManager.Instance.CompletedTask(brushTeethTask);
            if (makeToastTask.taskComplete == true && brushTeethTask.taskComplete == true)
            {
                bed.tag = "Bed";
            }
        }
    }*/
}
