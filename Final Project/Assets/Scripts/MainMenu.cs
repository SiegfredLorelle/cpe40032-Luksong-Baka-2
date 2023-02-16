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
    public GameObject confirmationMenu;


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
        confirmationMenu.SetActive(false);

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "Help")
                SceneManager.UnloadSceneAsync("Help");
            break;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Final Project");
    }

    public void LoadHelp()
    {
        SceneManager.LoadScene("Help", LoadSceneMode.Additive);
    }
}
