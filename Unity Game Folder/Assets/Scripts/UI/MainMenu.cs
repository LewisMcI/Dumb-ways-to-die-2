using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer masterMixer;
    public AudioSource vfxTestNoise;

    bool isTutorial = true;
    public void Play()
    {
        if (isTutorial == true)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
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
        QualitySettings.SetQualityLevel(index, true);
    }

    public void SetVFXVolume(Slider slider)
    {
        if (slider.value == slider.minValue)
        {
            masterMixer.SetFloat("VFXVolume", -1000);
        }
        masterMixer.SetFloat("VFXVolume", slider.value);
        vfxTestNoise.Play();
    }
    public void SetMusicVolume(Slider slider)
    {
        masterMixer.SetFloat("MusicVolume", slider.value);
    }
    public void SetMouseSensitivity(Slider slider)
    {
        // TODO: FIX
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
