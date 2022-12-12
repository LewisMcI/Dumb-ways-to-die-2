using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    bool isTutorial = true;
    public void Play()
    {
        if (isTutorial == true)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void ChangeTutorialOption()
    {
        isTutorial = !isTutorial;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();

    }

    public void SetQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index, false);
    }

    public void SetVFXVolume(Slider slider)
    {
        GameSettings.Instance.SetVFXVolume((int)slider.value);
    }
    public void SetMusicVolume(Slider slider)
    {
        GameSettings.Instance.SetMusicVolume((int)slider.value);
    }
    public void SetMouseSensitivity(Slider slider)
    {
    }

    public void ChangeScreenSettings(TMP_Dropdown dropdown)
    {
        if (dropdown.value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if (dropdown.value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
}
