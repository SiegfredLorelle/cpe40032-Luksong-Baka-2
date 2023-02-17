using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject musicPlayer;
    private AudioSource backgroundMusic;

    public GameObject mainMenu;
    public GameObject optionMenu;


    private void Start()
    {
        backgroundMusic = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();
        backgroundMusic.Play();

        LoadMainMenuUI();
    }

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

    public void PlayGame()
    {
        SceneManager.LoadScene(GameManager.SCENE_LUKSONGBAKA);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void LoadHelp()
    {
        SceneManager.LoadScene(GameManager.SCENE_HELP, LoadSceneMode.Additive);
    }

    // REMOVE THIS IF LEADERBOARD IS NOT WORKING
    public void LoadScore()
    {
        SceneManager.LoadScene("Score", LoadSceneMode.Additive);
    }
}
