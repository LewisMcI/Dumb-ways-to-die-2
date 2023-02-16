using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{

    bool randomTasks = false;
    TextMeshPro[] notepadText = new TextMeshPro[3];
    
    // Todays Current Tasks
    public Task[] todaysTasks = new Task[3];
    public Task[] TodaysTasks { get => todaysTasks; }

    private void Awake()
    {
        if (randomTasks)
        {
            throw new NotImplementedException("RandomTasks not implemented yet");
        }
        if (todaysTasks.Length < 3)
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
        notepadText[0] = GameObject.Find("Notepad Task One").GetComponent<TextMeshPro>();
        notepadText[1] = GameObject.Find("Notepad Task Two").GetComponent<TextMeshPro>();
        notepadText[2] = GameObject.Find("Notepad Task Three").GetComponent<TextMeshPro>();

        GameObject.Find("Notepad").SetActive(false);
        UpdateNotepad();
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
        for (int i = 0; i < 3; i++)
        {
            string newText = todaysTasks[i].taskName;
            if (todaysTasks[i].steps > 1)
                newText = newText + " (" + todaysTasks[i].stepsComplete + " / " + todaysTasks[i].steps + ")";
            if (todaysTasks[i].stepsComplete >= todaysTasks[i].steps)
                newText = "<s>" + newText + "</s>";
            notepadText[i].text = newText;
        }
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

                    foreach (var text in notepadText)
                    {
#pragma warning disable CS0642 // Possible mistaken empty statement
                        if (text.text.Replace(" ", "").Contains(task.name.Replace(" ", "")));
#pragma warning restore CS0642 // Possible mistaken empty statement
                        {
                            GameUI.Instance.NotifyAnim.SetTrigger("Notify");
                            task.taskComplete = true;
                            UpdateNotepad();
                            return;
                        }
                    }
                    throw new Exception("Trying to complete task that is not on notepad");
                }
                UpdateNotepad();
                return;
            }
        }
        throw new Exception("Trying to complete task that does not exist");
    }
}
