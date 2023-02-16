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

    //private void Awake()
    //{
    //    Instantiate(musicPlayer);
    //}

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

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "Help")
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            break;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Final Project");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void LoadHelp()
    {
        SceneManager.LoadScene("Help", LoadSceneMode.Additive);
    }

     public void LoadScore()
    {
        SceneManager.LoadScene("Score", LoadSceneMode.Additive);
    }
}
