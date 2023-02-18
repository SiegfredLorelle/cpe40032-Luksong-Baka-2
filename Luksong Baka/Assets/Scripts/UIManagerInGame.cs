using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerInGame : MonoBehaviour
{
    // Attached to UI Manager in Luksong Baka Scene
    // Manages UI in game, specifically score, game over menu, and restarting game

    public GameManager gameManagerScript;

    public GameObject scoreDisplay;
    public GameObject finalScoreDisplay;
    public GameObject restartButton;
    public GameObject bg;
    public float gameOverMenuDelay = 3.0f;


    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartUI();
    }

    // Called at start method
    // Turn on necessary menus, turn off others
    public void StartUI()
    {
        finalScoreDisplay.SetActive(false);
        restartButton.SetActive(false);
        bg.SetActive(false);
        scoreDisplay.SetActive(true);
        UpdateScoreText();
    }

    // Called when starting ui in start method also in game manager script, when score changes
    // Update the score text
    public void UpdateScoreText()
    {
        scoreDisplay.gameObject.GetComponent<Text>().text = "SCORE: " + gameManagerScript.score;
    }

    // Called in player controller script, when player has 0 lives/game over
    // Opens game over menu with delay
    public void GameOver()
    {
        StartCoroutine("DeathScreen");
    }

    // Called when clicking restart from game over screen
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Turn on game over menu at a delay
    IEnumerator DeathScreen()
    {
        yield return new WaitForSeconds(gameOverMenuDelay);
        scoreDisplay.SetActive(false);
        finalScoreDisplay.SetActive(true);
        finalScoreDisplay.GetComponent<Text>().text = $"FINAL SCORE: {gameManagerScript.score}";
        restartButton.SetActive(true);
        bg.SetActive(true);
    }
}
