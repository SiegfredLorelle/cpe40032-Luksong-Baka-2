using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour 
{
    // Ensures only one instance of music player exists
    // (prevents recreating another music player when loading menu scene)
    private static BackgroundMusic instance;
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
    
 
    public AdjustVolume adjustVolumeScript;

    // Reference to Audio Source component
    private AudioSource audioSrc;


    //Use this for initialization
   void Start()
   {
        // Don't destroy music player when swtiching scenes so that other scene can use it as background musisc
        DontDestroyOnLoad(gameObject);
        SetupBackgroundMusic();
   }

    private void SetupBackgroundMusic()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = PlayerPrefs.GetFloat("volume", 1.0f);
    }

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
