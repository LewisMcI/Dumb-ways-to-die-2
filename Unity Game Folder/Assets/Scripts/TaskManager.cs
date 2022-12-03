using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Task[] GenerateTasks()
    {
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
        // Create Array Of Tasks
        Task[] finalTasks = {todaysMorningTask, todaysMiddayTask, todaysFinalTask};
        return finalTasks;
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
            Debug.Log("Length " + (task.associatedTraps.Length));
            Debug.Log("Random Is " + randomIndex);
            Debug.Log("Trap is " + task.associatedTraps[randomIndex].name);
            newTrap = Instantiate(task.associatedTraps[randomIndex], new Vector3(0, 0, 0), Quaternion.identity, positionObj.transform);
        }
        newTrap.transform.localPosition = Vector3.zero;
        newTrap.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
