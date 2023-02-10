using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskManager : MonoBehaviour
{
    // List of Tasks
    [SerializeField]
    private Task[] availableMorningTasks;
    [SerializeField]
    private Task[] availableMiddayTasks;
    [SerializeField]
    private Task[] availableFinalTasks;

    private Task todaysMorningTask;
    private Task todaysMiddayTask;
    private Task todaysFinalTask;

    [SerializeField]
    TextMeshPro[] notepadText = new TextMeshPro[3];
    
    // Todays Current Tasks
    private Task[] todaysTasks;
    public Task[] TodaysTasks { get => todaysTasks; }

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
    public bool GenerateTasks()
    {
        // Reset All Traps
        foreach (var trap in availableMorningTasks)
            trap.Reset();
        foreach (var trap in availableMiddayTasks)
            trap.Reset();
        foreach (var trap in availableFinalTasks)
            trap.Reset();

        // Tasks are initialized?
        if (availableMorningTasks.Length == 0 || availableMiddayTasks.Length == 0 || availableFinalTasks.Length == 0)
        {
            Debug.Log("Tasks not initialized");
            throw new System.Exception("Tasks are not initialized in the TaskManager");
        }

        // Morning Tasks
        if (availableMorningTasks.Length == 1)
        {
            todaysMorningTask = availableMorningTasks[0];
        }
        else
        {
            int randomIndex = Random.Range(0, availableMorningTasks.Length);
            todaysMorningTask = availableMorningTasks[randomIndex];
        }
        CreateTrap(todaysMorningTask);
        // Midday Tasks
        if (availableMiddayTasks.Length == 1)
        {
            todaysMiddayTask = availableMiddayTasks[0];
        }
        else
        {
            int randomIndex = Random.Range(0, availableMiddayTasks.Length);
            todaysMiddayTask = availableMiddayTasks[randomIndex];
        }
        CreateTrap(todaysMiddayTask);
        // Final Tasks
        if (availableFinalTasks.Length == 1)
        {
            todaysFinalTask = availableFinalTasks[0];
        }
        else
        {
            int randomIndex = Random.Range(0, availableFinalTasks.Length);
            todaysFinalTask = availableFinalTasks[randomIndex];
        }

        CreateTrap(todaysFinalTask);

        try
        {
            // Create Array Of Tasks
            Task[] finalTasks = { todaysMorningTask, todaysMiddayTask, todaysFinalTask };
            todaysTasks = finalTasks;
        }
        catch
        {
            return false;
        }
        UpdateNotepad();
        return true;
    }


    void CreateTrap(Task task)
    {
        if (task.nameOfPosition == "")
        {
            return;
        }

        // Traps are initialized?
        if (task.associatedTraps.Length == 0)
        {
            Debug.Log("Tasks not initialized");
            throw new System.Exception("Tasks are not initialized in the TaskManager");
        }

        // Find Position and Destroy temp placement
        GameObject positionObj = GameObject.Find(task.nameOfPosition);
        if (positionObj.transform.childCount != 0)
        {
            for (int i = 0; i < positionObj.transform.childCount; i++)
            {
                Destroy(positionObj.transform.GetChild(i).gameObject);
            }
        }
        // Instantiate Trap
        GameObject newTrap;
        if (task.associatedTraps.Length == 1)
        {
            newTrap = Instantiate(task.associatedTraps[0], new Vector3(0, 0, 0), Quaternion.identity, positionObj.transform);
        }
        else
        {
            int randomIndex = Random.Range(0, task.associatedTraps.Length);
            Debug.Log("New Trap is '" + task.associatedTraps[randomIndex].name + "'");
            newTrap = Instantiate(task.associatedTraps[randomIndex], new Vector3(0, 0, 0), Quaternion.identity, positionObj.transform);
        }
        newTrap.transform.localPosition = Vector3.zero;
        newTrap.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
