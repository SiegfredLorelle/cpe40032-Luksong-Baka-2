using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackOnEscape : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject helpMenu;

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
            if (helpMenu != null)
            {
                ExitHelp();
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
        helpMenu = GameObject.Find("HelpMenu");
    }

    public void ExitHelp()
    {
        SceneManager.UnloadSceneAsync("Help");
    }
       
    public void ExitOptions()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
