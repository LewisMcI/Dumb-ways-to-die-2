using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    GameSettings gameSettings;
    private void Awake()
    {
        gameSettings = GameObject.Find("Game Settings").GetComponent<GameSettings>();
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();

        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void SetQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }

    public void SetVFXVolume(Slider slider)
    {
        gameSettings.vfxVolume = (int)slider.value;
    }
    public void SetMusicVolume(Slider slider)
    {
        gameSettings.musicVolume = (int)slider.value;
    }
    public void SetMouseSensitivity(Slider slider)
    {
        // TODO: FIX
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
