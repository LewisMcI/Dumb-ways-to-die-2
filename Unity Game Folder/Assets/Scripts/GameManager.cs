using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region fields
    private bool started;
    private bool enableControls;
    private bool enableCamera;
    private float enableControlsTimer = 2.5f;

    public Task[] tasks;

    public TextMeshPro[] notepadText = new TextMeshPro[3];


    public static GameManager Instance;

    public TaskManager taskManager;
    private Task[] todaysTasks;

    public bool gameState = true;

    [Range(10.0f, 1080.0f)]
    public float timerForLevel = 120;
    private float timeLeft;
    public TextMeshPro timerText;

    public GameSettings settings;

    public GameUI gameUI;
    #endregion

    #region properties
    public bool EnableControls
    {
        get { return enableControls; }
        set { enableControls = value; }
    }
    public bool EnableCamera
    {
        get { return enableCamera; }
        set { enableCamera = value; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        timeLeft = timerForLevel;
        InitializeTasks();
        if (settings == null) 
        {
            settings = GameObject.FindObjectOfType<GameSettings>();
        }
    }

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
    private void InitializeTasks()
    {
        todaysTasks = taskManager.GenerateTasks();
        Debug.Log("Breakfast task is: " + todaysTasks[0].name + ", Midday Task is: " + todaysTasks[1].name + ", Final Task is: " + todaysTasks[2].name);
        UpdateNotepad();
    }

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

    private void Update()
    {
        if (gameState != false)
            timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
            UpdateTimer();
        else
            Restart();

        if (!started)
        {
            if (enableControlsTimer <= 0.0f)
            {
                enableControls = true;
                EnableCamera = true;
                started = true;
            }
            else
                enableControlsTimer -= Time.deltaTime;
        }

        // Rotate towards book when paused
        if (!gameState)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0.05f, 1.65f, 0.23f), 5f * Time.deltaTime);
            Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation, Quaternion.Euler(-6, -8, -2), 3f * Time.deltaTime);
        }
    }

    private void UpdateTimer()
    {
        int timeInMinutes = (int)(timeLeft / 60.0f);
        int timeInSeconds = (int)(timeLeft) - (timeInMinutes * 60); 
        if (timeInSeconds < 10)
            timerText.text = timeInMinutes + ":0" + timeInSeconds;
        else
            timerText.text = timeInMinutes + ":" + timeInSeconds;
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game");
        gameState = !gameState;

        if (gameState)
        {
            enableControls = true;
            EnableCamera = true;
            GameUI.Instance.pauseMenu.SetActive(false);
            GameUI.Instance.settingsMenu.SetActive(false);
            GameUI.Instance.pauseText.SetActive(true);
        }
        else
        {
            GameUI.Instance.pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            enableControls = false;
            EnableCamera = false;
        }

        PlayerController.Instance.Book();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main Scene");
    }
    #endregion
}