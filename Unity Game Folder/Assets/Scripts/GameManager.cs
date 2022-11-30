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
    private GameObject player;

    public List<Task> tasks;

    public TaskManager taskManager;


    public static GameManager Instance;

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
    public GameObject Player
    {
        get { return player; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        timeLeft = timerForLevel;
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
        timerText.text = timeInMinutes + ":" + timeInSeconds;
    }

    public void CompletedTask(Task task)
    {
        Debug.Log("Completed task");
        taskManager.CompletedTask(task);

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
