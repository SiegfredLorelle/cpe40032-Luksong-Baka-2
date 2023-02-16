using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackScore : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject scoreMenu;

    //void Start()
    //{
    //    CheckOpenMenus();
    //}
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            CheckOpenMenus();
            if (scoreMenu != null)
            {
                ExitScore();
            }

            else if (optionsMenu != null)
            {
                ExitOptions();
            }
        }

    }

    public void CheckOpenMenus()
    {
        optionsMenu = GameObject.Find("OptionsMenu");
        scoreMenu = GameObject.Find("ScoreMenu");
    }

    public void ExitScore()
    {
        SceneManager.UnloadSceneAsync("Score");
    }
       
    public void ExitOptions()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}


