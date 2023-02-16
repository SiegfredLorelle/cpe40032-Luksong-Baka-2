using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Ensures only one instance of music player exists
    // (prevents recreating another music player when loading menu scene)
    private static AudioManager instance;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    //public AdjustVolume adjustVolumeScript;
    public AudioMixer mixer;

    // Reference to Audio Source component
    //private AudioSource audioSrc;


    //Use this for initialization
    void Start()
    {
        // Don't destroy music player when swtiching scenes so that other scene can use it as background musisc
        
        DontDestroyOnLoad(gameObject);
        SetUpVolume();
        //SetupBackgroundMusic();
    }

    private void SetUpVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(AdjustVolume.MIXER_MUSIC, 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat(AdjustVolume.MIXER_SFX, 1.0f);

        mixer.SetFloat(AdjustVolume.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(AdjustVolume.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);

    }



    //private void SetupBackgroundMusic()
    //{
    //    audioSrc = GetComponent<AudioSource>();
    //    audioSrc.volume = PlayerPrefs.GetFloat("volume", 1.0f);
    //}

    //public void PlayBackgroundMusic()
    //{
    //    if (!audioSrc.isPlaying)
    //    {
    //        audioSrc.Play();
    //    }
    //}

    //public void StopBackgroundMusic()
    //{
    //    audioSrc.Stop();
    //}
}
