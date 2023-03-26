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
    GameObject qualitySettingsCircle;
    [SerializeField]
    GameObject displaySettingsCircle;

    [SerializeField]
    GameObject[] qualitySettingsObjs;
    [SerializeField]
    GameObject[] displaySettingsObjs;


    [SerializeField]
    string mainMenuSceneName;

    float maxMusicValue = 15;
    float currentMusicValue = 15;
    float maxSFXValue = 15;
    float currentSFXValue = 15;

    public void Reset(float musicVolume, float sfxVolume, int qualitySetting, int displaySetting)
    {
        currentMusicValue = (musicVolume + 80.0f) / 80.0f * maxMusicValue;
        currentSFXValue = (sfxVolume + 80.0f) / 80.0f * maxSFXValue;
        PassMusicVolume();
        PassSFXVolume();
        ChangeQualityLevel(qualitySetting);
        ChangeScreenSettings(displaySetting);
    }

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

    void LowQualityLevel()
    {
        ChangeQualityLevel(0);
    }
    void MedQualityLevel()
    {
        ChangeQualityLevel(1);
    }
    void HighQualityLevel()
    {
        ChangeQualityLevel(2);
    }

    public void ChangeQualityLevel(int index)
    {
        qualitySettingsCircle.transform.position = qualitySettingsObjs[index].transform.position;
        GameSettings.Instance.SetQualitySettings(index);
        QualitySettings.SetQualityLevel(index, false);
    }
    void Windowed()
    {
        ChangeScreenSettings(0);
    }
    void Fullscreen()
    {
        ChangeScreenSettings(1);
    }
    void ExclusiveFullscreen()
    {
        ChangeScreenSettings(2);
    }

    public void ChangeScreenSettings(int index)
    {
        displaySettingsCircle.transform.position = displaySettingsObjs[index].transform.position;
        GameSettings.Instance.SetDisplayMode(index);
        if (index == 0)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (index == 1)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
    }
}
