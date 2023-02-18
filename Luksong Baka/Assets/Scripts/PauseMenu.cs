using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Attached to UI Manager in Luksong Baka Scene
    // Pauses the game and controls command in pause menu

    public GameManager gameManagerScript;

    public GameObject pauseMenuUI;
    public GameObject confirmationMenuUI;
    public AudioSource[] audioSources;

    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Ensures timescale is normal when reloading scenes
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        // If escape key is pressed, and game is still going,
        // And confirmation is closed, and help menu is not loaded
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManagerScript.isGameStopped
            && !confirmationMenuUI.activeSelf && !SceneManager.GetSceneByName(GameManager.SCENE_HELP).isLoaded)
        {
            // Unpause the game if currently paused
            if (gameManagerScript.isGamePaused)
            {
                Resume();
            }
            // Pause the game if currently unpaused
            else
            {
                Pause();
            }
        }
    }

    // Called when pressing escape while game is paused
    // Resumes the game, close pause menu, unapuse all audio sources
    public void Resume()
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

    // Called when pressing escape while game is unpaused
    // Pause the game, open pause menu, pause all audio sources
    public void Pause()
    {
        gameManagerScript.isGamePaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        // Find all audio sources and pause each one of them
        audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Pause();
        }
    }

    // Called when clicking restart button in pause menu
    // Relaods Luksong Baka Scene
    public void Restart()
    {
        SceneManager.LoadScene(GameManager.SCENE_LUKSONGBAKA);

    }

    // Called when clicking help button in pause menu
    // Loads the Help Menu Scene
    public void LoadHelp()
    {
        SceneManager.LoadScene(GameManager.SCENE_HELP, LoadSceneMode.Additive);
    }
}
