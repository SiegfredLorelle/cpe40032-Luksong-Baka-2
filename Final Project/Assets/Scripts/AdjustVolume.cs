using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class AdjustVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider SFXSlider;


    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";


    private AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        //backgroundMusic = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();

        //SetupSlider();

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }
        
    // Update is called once per frame
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




    //public void SetupSlider()
    //{
    //    musicSlider = GetComponent<Slider>();
    //    SFXSlider.value = PlayerPrefs.GetFloat("volume");
    //}

}
