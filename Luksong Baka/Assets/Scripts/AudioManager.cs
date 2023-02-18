using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Set ups the volume of background music and sfx via mixer.

    // Ensures only one instance of Audio Manager exists
    // (prevents recreating another Audio Manager when loading menu scene)
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

    public AudioMixer mixer;

    void Start()
    {
        // Don't destroy audio manager when swtiching scenes so that other scene can use it as well
        DontDestroyOnLoad(gameObject);
        SetUpVolume();
    }

    // Called at the start method
    private void SetUpVolume()
    {
        // Get values saved in player pref and apply to mixer
        float musicVolume = PlayerPrefs.GetFloat(AdjustVolume.MIXER_MUSIC, 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat(AdjustVolume.MIXER_SFX, 1.0f);

        mixer.SetFloat(AdjustVolume.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(AdjustVolume.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
}