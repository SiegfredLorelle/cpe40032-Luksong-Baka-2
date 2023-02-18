using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Attached to music player which is a child of audio manager
    // Audio manager, including its child, music player, don't get destroyed on load
    // Music player loads on awake and is on loop
    // Play and stop the background music


    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Called in player controller script in intro setup
    // Start playing thebackground music if not yet playing
    public void PlayBackgroundMusic()
    {
        if (!audioSrc.isPlaying)
        {
            audioSrc.Play();
        }
    }

    // Called in player controller when the game is over
    // Stop playing the background music
    public void StopBackgroundMusic()
    {
        audioSrc.Stop();
    }
}
