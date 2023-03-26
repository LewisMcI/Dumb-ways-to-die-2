using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BookMenu : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    TextMeshProUGUI musicVolSlider;
    [SerializeField]
    TextMeshProUGUI sfxVolSlider;

    [SerializeField]
    string mainMenuSceneName;

    float maxMusicValue = 15;
    float currentMusicValue = 15;
    float maxSFXValue = 15;
    float currentSFXValue = 15;

    void PauseGame()
    {
        GameManager.Instance.PauseGame();
    }

    void ChangeMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    void ExitToMainMenu()
    {
        ChangeScene(mainMenuSceneName);
    }

    void ExitToDesktop()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void ChangeScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch
        {
            Debug.Log("The scene named: '" + sceneName + "' does not exist");
        }
    }

    void IncreaseMusicVol()
    {
        if (currentMusicValue + 1 <= maxMusicValue)
        {
            currentMusicValue++;
            PassMusicVolume();
        }
    }
    void DecreaseMusicVol()
    {
        if (currentMusicValue - 1 >= 0)
        {
            currentMusicValue--;
            PassMusicVolume();
        }
    }

    void PassMusicVolume()
    {
        int newValue = (int)((currentMusicValue / maxMusicValue * 80.0f) - 80.0f);
        Debug.Log(newValue);
        GameSettings.Instance.SetMusicVolume(newValue);
        musicVolSlider.text = "";
        for (int i = 0; i < currentMusicValue; i++)
        {
            musicVolSlider.text += "-";
        }
    }
    void IncreaseSFXVol()
    {
        if (currentSFXValue + 1 <= maxSFXValue)
        {
            currentSFXValue++;
            PassSFXVolume();
        }
    }
    void DecreaseSFXVol()
    {
        if (currentSFXValue - 1 >= 0)
        {
            currentSFXValue--;
            PassSFXVolume();
        }
    }

    void PassSFXVolume()
    {
        int newValue = (int)((currentSFXValue / maxSFXValue * 80.0f) - 80.0f);
        Debug.Log(newValue);
        GameSettings.Instance.SetVFXVolume(newValue);
        sfxVolSlider.text = "";
        for (int i = 0; i < currentSFXValue; i++)
        {
            sfxVolSlider.text += "-";
        }
    }
}
