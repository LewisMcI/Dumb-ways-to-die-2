using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region fields

    // Managerrs
    public static GameManager Instance;
    public TaskManager taskManager;
    GameSettings settings;

    // Set in inspector Dependencies
    [SerializeField]
    GameUI gameUI;

    // Instatiating variables.
    private bool enableControls = true;
    private bool enableCamera = true;
    private bool gameState = true;

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

    /* InitializeTasks
     * Inits Tasks for the day.
     */
    private void InitTasks()
    {
        if(!taskManager.GenerateTasks())
            throw new Exception("Failed to Initialize Tasks");
        Debug.Log("Breakfast task is: " + taskManager.TodaysTasks[0].name + ", Midday Task is: " + taskManager.TodaysTasks[1].name + ", Final Task is: " + taskManager.TodaysTasks[2].name);
    }


    private void Update()
    {
        // TODO: ektor what is this??????
        // Rotate towards book when paused
        if (!gameState)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0.05f, 1.65f, 0.23f), 5f * Time.deltaTime);
            Camera.main.transform.localRotation = Quaternion.Lerp(Camera.main.transform.localRotation, Quaternion.Euler(-6, -8, -2), 3f * Time.deltaTime);
        }
    }

    /* PauseGame
     * Sets game state equal to false and activates game ui.
     */
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

    /* Restarts Scene
     * 
     */
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}