using UnityEngine;

public class GameManager : MonoBehaviour
{
    // These constants ensures consistency and prevents typo issues
    /** START OF GLOBAL STRING CONSTANTS **/
    // Animations
    public const string ANIM_JUMP_TRIG = "Jump_trig";
    public const string ANIM_DEATH_B = "Death_b";
    public const string ANIM_SPEED_F = "Speed_f";
    public const string ANIM_INT = "Animation_int";
    public const string STATIC_B = "Static_b";

    // Tags
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

    // Scene names
    public const string SCENE_LUKSONGBAKA = "Luksong Baka";
    public const string SCENE_MENU = "Menu";
    public const string SCENE_HELP = "Help";

    // Object/prefab names
    public const string NAME_PROJECTILEXLIMIT = "ProjectileXLimitSensor";
    public const string NAME_MEAT = "Meat(Clone)";
    public const string NAME_BOMB = "Bomb(Clone)";
    public const string NAME_DAGGER = "Dagger(Clone)";
    public readonly string[] NAME_COWS = {"Brown Cow(Clone)", "White Cow(Clone)"};
    public readonly string[] NAME_CALVES = { "Brown Calf(Clone)", "White Calf(Clone)" };
    public readonly string[] NAME_TRUCKS = { "Truck(Clone)", "Two Trailer Truck(Clone)" };
    /** END OF GLOBAL STRING CONSTANTS **/

    // Values are assign at the start method
    public PlayerController playerControllerScript;
    public MoveBackground moveBackgroundScript;
    public UIManagerInGame UIManagerScript;

    public int score;
    public bool isGameStopped;
    public bool isGamePaused;
    public bool recentlyDamaged;
    public bool playerIsDashing;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        moveBackgroundScript = GameObject.Find("Background").GetComponent<MoveBackground>();
        UIManagerScript = GameObject.Find("UIManager").GetComponent<UIManagerInGame>();

        score = 0;
        isGamePaused = false;
        isGameStopped = true;
        recentlyDamaged = false;
        playerIsDashing = false;
    }


    // Called when obstacles hit score sensor and player taking damage
    // Can also be used to decrease score by having negative parameters
    public void IncreaseScore(int additinalScore)
    {
        // When called when taking damage, additional score will be negative
        // Set recently damaged to true so the next obstacle passing the point sensor won't receive score
        if (additinalScore < 0)
        {
            recentlyDamaged = true;
            score += additinalScore;
            // Ensures score will not be negative
            if (score < 0)
            {
                score = 0;
            }
        }
        // When called from obstacle passing point sensor or projectile-obstacle collision, then additional score will be positive
        // if not recently damaged, then add the score
        else if (!recentlyDamaged)
        {
            score += additinalScore;
        }
        // if recently damaged, then just set recently damage to false so next score will be added
        else
        {
            recentlyDamaged = false;
        }
    }
}
