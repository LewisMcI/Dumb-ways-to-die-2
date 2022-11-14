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

    public List<TextMeshPro> notepadText;

    public static GameManager Instance;
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
        if (tasks.Contains(task))
        {
            foreach (TextMeshPro text in notepadText)
            {
                if (text.text.Replace(" ", "") == task.taskName.Replace(" ", ""))
                {
                    task.taskComplete = true;
                    tasks.Remove(task);
                    text.text = "<s>" + text.text + "</s>";
                    continue;
                }
            }
        }
        else
        {
            Debug.Log("Invalid Task Complete");
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
