using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AdjustVolume : MonoBehaviour
{
    // Attached to option menu canvas in Menu Scene
    // Adjust volume of background music and sfx
    // Update the sliders' value to reflect current volume

    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider SFXSlider;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";


    void Start()
    {
        // Listen to value changes in both sliders
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Called when value change in music slide
    public void SetMusicVolume(float vol)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(vol) * 20);
    }

    // Called when value change in sfx slider
    public void SetSFXVolume(float vol)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(vol) * 20);
    }

    public void OnDisable()
    {
        SaveVolumeSetting();
    }

    public void OnEnable()
    {
        SetUpVolumeSlider();
    }

    // Called when enabling the options menu canvas
    // Update the slider's value to reflect current volume in the mixer
    private void SetUpVolumeSlider()
    {
        float musicVolume = PlayerPrefs.GetFloat(MIXER_MUSIC, 1.0f);
        float SFXVolume = PlayerPrefs.GetFloat(MIXER_SFX, 1.0f);

        musicSlider.value = musicVolume;
        SFXSlider.value = SFXVolume;
    }

    // Called when disabling options menu canvas
    // Save volume values via player prefs
    private void SaveVolumeSetting()
    {
        PlayerPrefs.SetFloat(MIXER_MUSIC, musicSlider.value);
        PlayerPrefs.SetFloat(MIXER_SFX, SFXSlider.value);
    }
}
