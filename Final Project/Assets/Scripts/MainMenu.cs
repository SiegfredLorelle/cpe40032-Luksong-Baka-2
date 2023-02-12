using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject musicPlayer;
    private AudioSource backgroundMusic;

    //private void Awake()
    //{
    //    Instantiate(musicPlayer);
    //}

    private void Start()
    {
        backgroundMusic = GameObject.FindGameObjectWithTag("Background Music").GetComponent<AudioSource>();
        backgroundMusic.Play();

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
}
