using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerInGame : MonoBehaviour
{

    public GameManager gameManagerScript;

    public GameObject scoreDisplay;
    public GameObject finalScoreDisplay;
    public GameObject restartButton;
    public GameObject bg;


    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartUI();
    }

    // when the game is started or restarted, we make sure the death
    // screen is deactivated, and the basic score display is activated

    public void StartUI()
    {
        scoreDisplay.SetActive(true);
        finalScoreDisplay.SetActive(false);
        restartButton.SetActive(false);
        bg.SetActive(false);
    }

    // score is 'how many obstacles we've avoided.' score is kept in
    // GameManager and updated whenever an obstacle fires its own
    // 'despawn' event, which is caused by the obstacle leaving the
    // screen and going out of bounds

    void Update()
    {
        scoreDisplay.gameObject.GetComponent<Text>().text = "SCORE: " + gameManagerScript.score;
    }


    public void GameOver()
    {
        StartCoroutine("DeathScreen");
    }

    // Called when clicking restart from game over screen
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // this is a coroutine that waits a couple of seconds after death, hides
    // the score display at the top, and displays the final score in the middle
    // of the screen along with a restart button

    // the slight delay is there both for fatalistic 'well, darn, I died'
    // flavor and to let the death animation finish playing.

    // without this delay, it's possible for the player to press 'restart' fast
    // enough for silly things to happen, such as the player character sliding
    // back onscreen in the 'intro' but also still in his dying animation.

    IEnumerator DeathScreen()
    {
        yield return new WaitForSeconds(3f);
        scoreDisplay.SetActive(false);
        finalScoreDisplay.SetActive(true);
        finalScoreDisplay.GetComponent<Text>().text = $"FINAL SCORE: {gameManagerScript.score}";
        restartButton.SetActive(true);
        bg.SetActive(true);
    }
}
