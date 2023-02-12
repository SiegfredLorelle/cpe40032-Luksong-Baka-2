﻿using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    ///** BEGIN SINGLETON DECLARATION **/

    //private GameManager _instance;
    //public GameManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            Debug.LogError("The GameManager doesn't exist!");
    //        }
    //        return _instance;
    //    }
    //}

    //private void Awake() => _instance = this;
    /** END SINGLETON DECLARATION **/

    /** GLOBAL CONSTANTS **/

    public PlayerController playerControllerScript;
    public MoveBackground moveBackgroundScript;
    public UIManager UIManagerScript;

    // I really dislike the way having strings right in my function calls
    // looks, so I prefer to use constants for things like this - helps
    // enforce consistency and reduce issues with potential typos, too

    public const string ANIM_JUMP_TRIG = "Jump_trig";
    public const string ANIM_DEATH_B = "Death_b";
    public const string ANIM_SPEED_F = "Speed_f";
    public const string ANIM_INT = "Animation_int";
    public const string STATIC_B = "Static_b";

    public const string TAG_WALKABLE = "Walkable";
    public const string TAG_OBSTACLE = "Obstacle";
    public const string TAG_POWERUP = "Power Up";
    public const string TAG_PLAYER = "Player";
    public const string TAG_PROJECTILE = "Projectile";
    public const string TAG_BACKGROUND = "Background";
    public const string TAG_POINTSENSOR = "Point Sensor";
    public const string TAG_DESTROYSENSOR = "Destroy Sensor";
    public const string TAG_HEART = "Heart";

    public const string NAME_MEAT = "Meat(Clone)";
    public readonly string[] NAME_COWS = {"Brown Cow(Clone)", "White Cow(Clone)"};
    public const string NAME_BOMB = "Bomb(Clone)";
    public const string NAME_DAGGER = "Dagger(Clone)";

    /** END GLOBAL CONSTANTS **/

    public int score;
    public bool isGameStopped;
    public bool isGamePaused;
    //public delegate void RestartAction();
    //public static event RestartAction GameRestart;

    // in a perfect world, i probably wouldn't need the below bool, but
    // i'm running out of time and it's a quick hack

    public bool playerIsDashing;

    // lots of events to subscribe to here - we want to know when the
    // player is dashing so that we know whether to enhance their score,
    // when they've finished the intro animation so we can initialize
    // the game, and when they've hit an obstacle so we can stop the game.

    // we also want to know when an object has despawned itself, as that is
    // the condition that causes the score to increase


    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        moveBackgroundScript = GameObject.Find("Background").GetComponent<MoveBackground>();
        UIManagerScript = GameObject.Find("UIManager").GetComponent<UIManager>();

        score = 0;
        isGamePaused = false;
        isGameStopped = true;
    }


    public void IncreaseScore(int additinalScore)
    {
        score += additinalScore;
    }

    // MoveLeft asks about isGameStopped to know whether it needs to currently
    // be moving or not. FinishedIntro() is fired when the player character
    // has finished the 'walk in' sequence. The background and obstacles will
    // know to start moving now.

    public void FinishedIntro()
    {
        isGameStopped = false;
    }


    // This is fired when the Restart button is pressed. Score is reset,
    // we temporarily stop the game so that the intro can replay, and we
    // let event subscribers know to smash that like button-- I mean
    // re-initialize their states to the start of the game.

    public void RestartGame()
    {
        score = 0;
        isGameStopped = true;
        playerControllerScript.SetupIntro();
        moveBackgroundScript.ResetBackground();
        UIManagerScript.StartUI();


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameRestart?.Invoke();
    }


}
