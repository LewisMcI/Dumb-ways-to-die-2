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
    private float enableControlsTimer = 2.5f;
    private GameObject player;

    public List<Task> tasks;

    public TaskManager taskManager;


    public static GameManager Instance;

    public bool gameState = true;

    public GameObject pauseMenu;
    #endregion

    #region properties
    public bool EnableControls
    {
        get { return enableControls; }
        set { enableControls = value; }
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
    }

    private void Update()
    {
        if (!started)
        {
            if (enableControlsTimer <= 0.0f)
            {
                enableControls = true;
                started = true;
            }
            else
                enableControlsTimer -= Time.deltaTime;
        }
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
        if (pauseMenu != null)
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        else
            Debug.Log("Pause Menu Not Setup");

        if (gameState)
        {
            Time.timeScale = 1;
            enableControls = true;
        }
        else
        {
            Time.timeScale = 0;
            enableControls = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
