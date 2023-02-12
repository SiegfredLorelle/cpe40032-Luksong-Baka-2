using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AdjustVolume : MonoBehaviour
{
    private AudioSource backgroundMusic;
    private Slider slider;


    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();
        SetupSlider();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVolume(float vol)
    {
        PlayerPrefs.SetFloat("volume", vol);
        backgroundMusic.volume = PlayerPrefs.GetFloat("volume");
    }

    public void SetupSlider()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("volume");
    }

}
