using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameManager gameManagerScript;

    public GameObject pauseMenuUI;
    public AudioSource[] audioSources;

    // Start is called before the first frame update
    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Ensures timescale is normal when reloading scenes
        Time.timeScale = 1.0f;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManagerScript.isGameStopped)
        {
            if (gameManagerScript.isGamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
    public void Resume ()
    {
        gameManagerScript.isGamePaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        // Unpause all sound sources
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.UnPause();
        }
    }

    public void Pause ()
    {
        gameManagerScript.isGamePaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        // Find all audio sources then pause them
        audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Pause();
        }

    }

     public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        
    }
     public void Restart()
    {
        SceneManager.LoadScene("Final Project");
        
    }

    
}
