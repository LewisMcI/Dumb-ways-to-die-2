using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public bool tutorial = true;

    public int vfxVolume;
    public int musicVolume;

    public AudioMixer masterMixer;
    public AudioSource vfxTestNoise;

    public static GameSettings Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneChange;
        DontDestroyOnLoad(gameObject);
    }

    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        SetVFXVolume(vfxVolume);
        SetMusicVolume(musicVolume);
        // Find Music Slider
        // Find VFX Slider
        // Edit currentValue = settingsValue
    }
    public void SetVFXVolume(int value)
    {
        masterMixer.SetFloat("VFXVolume", value);
        vfxTestNoise.Play();
    }
    public void SetMusicVolume(int value)
    {
        masterMixer.SetFloat("MusicVolume", value);
    }
}
