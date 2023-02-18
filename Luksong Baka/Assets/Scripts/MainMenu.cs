using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Attached to UI Manager of Menu Scene
    // Manages UI in main menu

    // Music
    public GameObject musicPlayer;
    private AudioSource backgroundMusic;

    // UI
    public GameObject mainMenu;
    public GameObject optionMenu;


    private void Start()
    {
        // Ensures the background music is playing upon starting
        backgroundMusic = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();
        backgroundMusic.Play();

        LoadMainMenuUI();
    }

    // Called at the start method
    // Turn on main menu canvas, turn off option menu canvas, and unload help scene if loaded
    private void LoadMainMenuUI()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        if (SceneManager.GetSceneByName(GameManager.SCENE_HELP).isLoaded)
        {
            SceneManager.UnloadSceneAsync(GameManager.SCENE_HELP);
        }
    }

    // Called when pressing play button
    // Load game, Luksong Baka Scene
    public void PlayGame()
    {
        SceneManager.LoadScene(GameManager.SCENE_LUKSONGBAKA);
    }

    // Called when pressing help button
    // Loads the Help Scene additively
    public void LoadHelp()
    {
        SceneManager.LoadScene(GameManager.SCENE_HELP, LoadSceneMode.Additive);
    }
}
