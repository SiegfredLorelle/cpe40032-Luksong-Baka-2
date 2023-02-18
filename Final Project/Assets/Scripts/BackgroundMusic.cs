using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour 
{
    private AudioSource audioSrc;

    void Start()
    {
        // Don't destroy music player when swtiching scenes so that other scene can use it as background musisc
        audioSrc = GetComponent<AudioSource>();
        //DontDestroyOnLoad(gameObject);
        //SetupBackgroundMusic();
    }

    //private void SetupBackgroundMusic()
    //{
    //    
    //    audioSrc.volume = PlayerPrefs.GetFloat("volume", 1.0f);
    //}

    public void PlayBackgroundMusic()
    { 
        if (!audioSrc.isPlaying)
        {
            audioSrc.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        audioSrc.Stop();
    }
}
