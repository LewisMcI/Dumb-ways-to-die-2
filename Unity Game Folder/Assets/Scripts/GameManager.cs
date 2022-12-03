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
    public TextMeshProUGUI timerText;

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
    }

    private void InitializeTasks()
    {
        todaysTasks = taskManager.GenerateTasks();
        Debug.Log("Breakfast task is: " + todaysTasks[0].name + ", Midday Task is: " + todaysTasks[1].name + ", Final Task is: " + todaysTasks[2].name);
        for (int i = 0; i < 3; i++)
        {
            notepadText[i].text = todaysTasks[i].name;
        }
    }
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            UpdateTimer();
        }
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

    public void CompletedTask(Task task)
    {
        Debug.Log("Completed task");
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

    public void PauseGame()
    {
        Debug.Log("Pause Game");
        gameState = !gameState;
        GameObject pauseMenu = null;
        if (gameUI != null)
            pauseMenu = gameUI.pauseMenu;
        if (pauseMenu != null)
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        else
            Debug.Log("Pause Menu Not Setup");

        if (gameState)
        {
            Time.timeScale = 1;
            enableControls = true;
            EnableCamera = true;
        }
        else
        {
            Time.timeScale = 0;
            enableControls = false;
            EnableCamera = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
