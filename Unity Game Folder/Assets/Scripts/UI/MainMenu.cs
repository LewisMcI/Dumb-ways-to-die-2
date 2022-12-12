using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    bool isTutorial = true;

    private void Awake()
    {
        try
        {
            GameSettings.Instance.ResetVolumes();
        }
        catch { }
    }
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
        Debug.Log(slider.value);
        if (slider.value != GameSettings.Instance.vfxVolume && slider.value != 0)
        {
            GameSettings.Instance.SetVFXVolume((int)slider.value);
        }
    }
    public void SetMusicVolume(Slider slider)
    {
        Debug.Log(slider.value);
        if (slider.value != 0)
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
