using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region fields
    // Managers
    public static GameManager Instance;
    public TaskManager taskManager;
    GameSettings settings;

    // Set in inspector Dependencies
    [SerializeField]
    private GameUI gameUI;

    // Instatiating variables.
    private bool enableControls = true;
    private bool enableCamera = true;
    private bool isPaused;
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
    public bool IsPaused
    {
        get { return isPaused; }
    }
    #endregion

    #region methods
    private void Awake()
    {
        // GameManager Instance
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Get Managers
        try
        {
            taskManager = GetComponent<TaskManager>();
        }
        catch
        {
            throw new Exception("Could not find TaskManager");
        }
        settings = GameObject.FindObjectOfType<GameSettings>();
        if (settings == null)
            throw new Exception("Could not find TaskManager");

        // InitTasks
        InitTasks();
    }

    /// <summary>
    ///  Inits Tasks for the day.
    /// </summary>
    private void InitTasks()
    {
        Task[] tempTasks = taskManager.TodaysTasks;
        if(!tempTasks[0])
            throw new Exception("Failed to Initialize Tasks");
        Debug.Log("Breakfast task is: " + tempTasks[0].name + ", Midday Task is: " + tempTasks[1].name + ", Final Task is: " + tempTasks[2].name);
    }

    /// <summary>
    ///  Sets game state equal to false and activates game ui.
    /// </summary>
    public void PauseGame()
    {
        Debug.Log("Pause Game");
        isPaused = !isPaused;

        // Paused
        if (isPaused)
        {
            GameUI.Instance.pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            enableControls = false;
            EnableCamera = false;
        }
        // Unpaused
        else
        {
            enableControls = true;
            EnableCamera = true;
            GameUI.Instance.pauseMenu.SetActive(false);
            GameUI.Instance.settingsMenu.SetActive(false);
            GameUI.Instance.pauseText.SetActive(true);
        }

        PlayerController.Instance.Book();
    }

    /// <summary>
    ///  Restarts current scene.
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}