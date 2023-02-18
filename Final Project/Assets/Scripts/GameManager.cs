using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public PlayerController playerControllerScript;
    public MoveBackground moveBackgroundScript;
    public UIManagerInGame UIManagerScript;

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
    public const string TAG_HEIGHTLIMIT = "Height Limit Sensor";
    public const string TAG_WITHINCAMERA = "Within Camera Sensor";
    public const string TAG_HEART = "Heart";

    public const string SCENE_LUKSONGBAKA = "Luksong Baka";
    public const string SCENE_MENU = "Menu";
    public const string SCENE_HELP = "Help";


    public const string NAME_PROJECTILEXLIMIT = "ProjectileXLimitSensor";
    public const string NAME_MEAT = "Meat(Clone)";
    public readonly string[] NAME_COWS = {"Brown Cow(Clone)", "White Cow(Clone)"};
    public readonly string[] NAME_CALVES = { "Brown Calf(Clone)", "White Calf(Clone)" };
    public readonly string[] NAME_TRUCKS = { "Truck(Clone)", "Two Trailer Truck(Clone)" };


    public const string NAME_BOMB = "Bomb(Clone)";
    public const string NAME_DAGGER = "Dagger(Clone)";

    /** END GLOBAL CONSTANTS **/

    public int score;
    public bool isGameStopped;
    public bool isGamePaused;
    public bool recentlyDamaged;
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
        UIManagerScript = GameObject.Find("UIManager").GetComponent<UIManagerInGame>();

        score = 0;
        isGamePaused = false;
        isGameStopped = true;
        recentlyDamaged = false;
    }


    // can also be used to decrease score by using negative argument,
    // called when obstacles hit score sensor and  player taking damage
    public void IncreaseScore(int additinalScore)
    {
        // additional score will be negative if called when taking damage
        // sat recently damaged to true so the next obstacle won't add the score
        if (additinalScore < 0)
        {
            recentlyDamaged = true;
            score += additinalScore;
            // ensures score will not be negative
            if (score < 0)
            {
                score = 0;
            }
        }
        // if not recently damaged, then add the score
        else if (!recentlyDamaged)
        {
            score += additinalScore;
        }
        // if recently damaged, then just set recently damage to false so next obstacle can be added
        else
        {
            recentlyDamaged = false;
        }
    }



    // Decrease score, called when taking damage


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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
