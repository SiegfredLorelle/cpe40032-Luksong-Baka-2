using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmationUI : MonoBehaviour
{
    // Attached UI Manager of Luksong Baka Scene and Menu Scene
    // Open Confirmation UI that asks for confirmation when exiting the game

    public GameObject ConfirmUI;

    private void Update()
    {
        // If user presses escape key, then close confirmation ui
        if (Input.GetKeyDown(KeyCode.Escape))
            DontWantToExitGame();
    }

    // Called when choosing yes when leaving from pause menu
    public void ExitGameFromPause()
    {
        SceneManager.LoadScene(GameManager.SCENE_MENU);
    }

    // Called when choosing yes in confirmation ui from game over menu
    public void ExitGameFromGameOver()
    {
        SceneManager.LoadScene(GameManager.SCENE_MENU);
    }

    // Called when pressing quit in pause menu or game over menu or main menu
    // Opens the confirmation ui
    public void OpenConfirmation()
    {
        ConfirmUI.SetActive(true);
    }

    // Called when pressing escape key or choosing no in confimation ui from pause menu or game over menu
    // Closes confirmation ui
    public void DontWantToExitGame()
    {
        ConfirmUI.SetActive(false);
    }

    // Called when choosing yes in confirmation ui from main menu
    public void ExitGameFromMenu()
    {
        QuitGame();
    }

    // Exits the game applications
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
