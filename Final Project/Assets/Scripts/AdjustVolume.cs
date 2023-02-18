using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class AdjustVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider SFXSlider;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";


    void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Update()
    {

    }

    public void SetMusicVolume(float vol)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(vol) * 20);
    }

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

    private void SetUpVolumeSlider()
    {
        float musicVolume = PlayerPrefs.GetFloat(MIXER_MUSIC, 1.0f);
        float SFXVolume = PlayerPrefs.GetFloat(MIXER_SFX, 1.0f);

        musicSlider.value = musicVolume;
        SFXSlider.value = SFXVolume;
    }



    private void SaveVolumeSetting()
    {
        PlayerPrefs.SetFloat(MIXER_MUSIC, musicSlider.value);
        PlayerPrefs.SetFloat(MIXER_SFX, SFXSlider.value);
    }
}
