using UnityEngine;
using UnityEngine.SceneManagement;

public class BackOnEscape : MonoBehaviour
{
    // Attached to UI Manager of Menu Scene and Help Scene
    // Listens to escape key input to close open menu (includes options menu and help menu)

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject helpMenu;

    void Update()
    {
        // If user presses escape key, then close open menus
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

    // Called when escape is pressed
    public void CheckOpenMenus()
    {
        optionsMenu = GameObject.Find("OptionsMenu");
        helpMenu = GameObject.Find("HelpMenu");
    }

    // Called when escape is pressed
    // Close help menu by unloading help scene
    public void ExitHelp()
    {
        SceneManager.UnloadSceneAsync("Help");
    }

    // Called when escape is pressed
    // Close option menu by setting its inactive, and set main menu active
    public void ExitOptions()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
