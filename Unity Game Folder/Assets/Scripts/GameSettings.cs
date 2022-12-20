using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [HideInInspector]
    public bool tutorial = true;
    [HideInInspector]
    public int vfxVolume;
    public int musicVolume;

    public AudioMixer masterMixer;
    public AudioSource vfxTestNoise;

    public static GameSettings Instance;
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetVolumes()
    {
        ResetMusicVolume();
        ResetVFXVolume();
    }

    public void SetVFXVolume(int value)
    {
        masterMixer.SetFloat("VFXVolume", value);
        vfxVolume = value;
        vfxTestNoise.Play();
    }
    public void SetMusicVolume(int value)
    {
        masterMixer.SetFloat("MusicVolume", value);
        musicVolume = value;
    }

    void ResetVFXVolume()
    {
        Slider[] sliders = Resources.FindObjectsOfTypeAll<Slider>() as Slider[];
        foreach (var slider in sliders)
        {
            if (slider.gameObject.name == "VFX Slider")
            {
                slider.gameObject.SetActive(false);
                slider.value = vfxVolume;
                slider.gameObject.SetActive(true);
                Debug.Log("Found");
                return;
            }
        }
    }

    void ResetMusicVolume()
    {
        Slider[] sliders = Resources.FindObjectsOfTypeAll<Slider>() as Slider[];
        foreach (var slider in sliders)
        {
            if (slider.gameObject.name == "Music Slider")
            {
                slider.gameObject.SetActive(false);
                slider.value = musicVolume;
                slider.gameObject.SetActive(true);
                Debug.Log("Found");
                return;
            }
        }
    }
}
