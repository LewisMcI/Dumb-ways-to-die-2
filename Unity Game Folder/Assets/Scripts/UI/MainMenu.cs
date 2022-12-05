using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer masterMixer;
    public AudioSource vfxTestNoise;
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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
