using UnityEngine;
using UnityEngine.SceneManagement;

public class BackOnEscape : MonoBehaviour
{
    // Attached to UI Manager of Menu and Help Scene

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject helpMenu;

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
