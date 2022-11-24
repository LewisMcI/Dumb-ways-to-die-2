using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Tasks
{
    BrushTeeth = 0,
    MakeToast = 1
}
public class TaskManager : MonoBehaviour
{
    // Tasks for the day
    public List<Tasks> tasks;
    public List<bool> tasksComplete = new List<bool>();

    private void Awake()
    {
        foreach(var task in tasks)
        {
            tasksComplete.Add(false);
        }
    }
    public List<TextMeshPro> notepadText;
    public void CompletedTask(string task)
    {
        Debug.Log("TaskManager");
        int index = -1;
        Tasks currentTask = Tasks.BrushTeeth;
        index = tasks.IndexOf(currentTask);
        if (index != -1)
        {
            foreach (TextMeshPro text in notepadText)
            {
                Debug.Log(text + task);
                if (text.text.Replace(" ", "") == task.Replace(" ", ""))
                {
                    GameUI.Instance.NotifyAnim.SetTrigger("Notify");
                    tasksComplete[index] = true;
                    text.text = "<s>" + text.text + "</s>";
                    continue;
                }
            }
        }
        else
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
