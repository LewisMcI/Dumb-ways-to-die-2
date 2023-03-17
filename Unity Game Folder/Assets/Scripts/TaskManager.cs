using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{

    bool randomTasks = false;

    TextMeshPro notepadText;
    
    // Todays Current Tasks
    public Task[] todaysTasks;
    public Task[] TodaysTasks { get => todaysTasks; }

    private void Awake()
    {
        ResetAllTraps();
        if (randomTasks)
        {
            throw new NotImplementedException("RandomTasks not implemented yet");
        }
        if (todaysTasks.Length == 0)
        {
            throw new Exception("Not Enough Traps in GameManager");
        }
    }

    public void ResetAllTraps()
    {
        foreach(var task in todaysTasks)
        {
            task.Reset();
        }
    }
    public void FindNotepadText()
    {
        notepadText = GameObject.Find("Notepad Text").GetComponent<TextMeshPro>();
        Debug.Log("Found Notepad Text");
        GameObject.Find("Notepad").SetActive(false);
        UpdateNotepad();
        Debug.Log("Set Notepad Text");
    }

    /* AllTasksComplete
    * Are tasks complete? Returns true/false if all tasks excluding final tasks are complete.
    */
    public bool AllTasksComplete()
    {
        foreach(var task in todaysTasks)
        {
            if (task.taskComplete == false && !task.isDependent)
            {
                return false;
            }
        }
        return true;
    }
    /* UpdateNotepad
    * Updates notepad with any new text added
    */
    private void UpdateNotepad()
    {
        string newText = "";
        // First Task
        if (todaysTasks[0].stepsComplete >= todaysTasks[0].steps)
            newText = newText + "<s>";
         newText = newText + "1. " + todaysTasks[0].taskName;
 
        if (todaysTasks[0].steps > 1)
            newText = newText + " (" + todaysTasks[0].stepsComplete + " / " + todaysTasks[0].steps + ")";
        if (todaysTasks[0].stepsComplete >= todaysTasks[0].steps)
            newText = newText + "</s>";
        // New Tasks
        for (int i = 1; i < todaysTasks.Length; i++)
        {
            newText = newText + "\n";
            if (todaysTasks[i].stepsComplete >= todaysTasks[i].steps)
                newText = newText + "<s>";
            newText = newText + i + ". " + todaysTasks[i].taskName;
            if (todaysTasks[i].steps > 1)
                newText = newText + " (" + todaysTasks[i].stepsComplete + " / " + todaysTasks[i].steps + ")";
            if (todaysTasks[i].stepsComplete >= todaysTasks[i].steps)
                newText = newText + "</s>";
        }
        Debug.Log(todaysTasks[0].taskName);
        Debug.Log(newText);
        notepadText.text = newText;
    }

    /* UpdateTaskCompletion
     * Takes in the task name and updates the task, throws exception if task does not exist on 
     * either the notepad or the current task list.
     */
    public void UpdateTaskCompletion(string taskName)
    {
        foreach (var task in todaysTasks)
        {
            if (taskName == task.taskName)
            {
                task.stepsComplete++;
                if (task.stepsComplete >= task.steps)
                {
                    // Advance time
                    DynamicSky.Instance.AdvanceTime();
                    // Play time pass SFX
                    DynamicSky.Instance.transform.GetComponent<AudioSource>().Play();
#pragma warning disable CS0642 // Possible mistaken empty statement
                    if (notepadText.text.Replace(" ", "").Contains(task.taskName.Replace(" ", ""))) ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                    {
                        GameUI.Instance.NotifyAnim.SetTrigger("Notify");
                        task.taskComplete = true;
                        UpdateNotepad();
                        return;
                    }
                        throw new Exception("Trying to complete task that is not on notepad");
                }
                UpdateNotepad();
                return;
            }
        }
        throw new Exception("Trying to complete task that does not exist");
    }

    public override string ToString()
    {
        return "";
    }
}
