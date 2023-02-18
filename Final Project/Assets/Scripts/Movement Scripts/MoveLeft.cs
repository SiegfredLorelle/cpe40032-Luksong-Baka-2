using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Attached to obstacles, powerups, hearts and background

    // References from other scripts
    public GameManager gameManagerScript;
    public SpawnManager spawnManagerScript;

    public GameObject meatPrefab;

    // Effects
    public ParticleSystem explosionEffects;
    public List<AudioClip> mooSounds;
    public AudioSource sfxPlayer;
    public AudioClip explosionSound;
    public AudioClip cowHurtSound;
    public AudioClip metalHitSound;

    public bool isThrown = false;
    public bool isWithinScreen = false;

    private float moveSpeed = 20f;
    private float modifiedMoveSpeed;

    private Vector3 meatSpawnOffset = new Vector3(0, 3.0f, 1.5f);

    // Assign values to variables
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        sfxPlayer = GameObject.Find("SfxPlayer").GetComponent<AudioSource>();

        modifiedMoveSpeed = moveSpeed;
    }

    // Speeds up this object when playing is on dash, else slow down to normal speed
    void Update()
    {
        // Increase or decrease speed base on player dash
        if (gameManagerScript.playerIsDashing)
        {
            SpeedUp();
        }
        else
        {
            SlowDown();
        }
    }

    // Increase or decrease speed (called on update method)
    void SpeedUp()
    {
        modifiedMoveSpeed = moveSpeed * 2f;
    }
    void SlowDown()
    {
        modifiedMoveSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        // Do nothing if the game is stopped or if the object is being thrown (by strength powerup)
        if (gameManagerScript.isGameStopped || isThrown)
        {
            return;
        }
        // Do actual movement, using from relative to world
        // (so that objects will always go left with respect to the world/camera regardless of their rotation)
        transform.Translate(Vector3.left * Time.fixedDeltaTime * modifiedMoveSpeed, Space.World);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // If this obstacle spawned at a position that overlaps the truck (trailer) then delete this object
        if (collision.gameObject.name == "TrailerCollider" && !isThrown && !isWithinScreen)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If obstacle got hit by a projectile (bomb or dagger)
        if (CompareTag(GameManager.TAG_OBSTACLE) && other.CompareTag(GameManager.TAG_PROJECTILE))
        {
            // If this object was hit by a dagger
            if (other.name == GameManager.NAME_DAGGER)
            {
                // this object is a cow/calf 
                if (gameManagerScript.NAME_COWS.Contains(gameObject.name) || gameManagerScript.NAME_CALVES.Contains(gameObject.name))
                {
                    ObstacleProjectileCollision(other.gameObject, cowHurtSound, 2);
                    TurnCowToMeat();
                }
                else
                {
                    ObstacleProjectileCollision(other.gameObject, metalHitSound, 1);

                }
            }
            // If this object was hit by a bomb
            else if (other.name == GameManager.NAME_BOMB)
            {
                ObstacleProjectileCollision(other.gameObject, explosionSound, 1);

            }
        }

        // If this object is a powerup or heart and is not yet within player screen
        else if ((gameObject.CompareTag(GameManager.TAG_POWERUP) || gameObject.CompareTag(GameManager.TAG_HEART)) && !isWithinScreen)
        {
            // If the object that collided with powerup/heart is a truck's trailer (top) then delete the truck
            if ((other.gameObject.name == "TrailerTopCollider" && !other.transform.parent.gameObject.GetComponent<MoveLeft>().isThrown))
            {
                Destroy(other.transform.parent.gameObject);
            }
            // In case of a rare case where a powerup/heart collided with another powerup/heart just destroy it
            else if (other.gameObject.CompareTag(GameManager.TAG_POWERUP) || other.gameObject.CompareTag(GameManager.TAG_HEART))
            {
                Destroy(other.gameObject);
            }
        }

        // If object is hit the within camera sensor, then set it to be within screen and play moo sfx if this object is a cow
        // Note: spawn sfx for trucks (horn) is called in the truck collision script
        else if (other.gameObject.CompareTag(GameManager.TAG_WITHINCAMERA))
        {
            if (gameManagerScript.NAME_COWS.Contains(gameObject.name))
                PlayMooSoundEffects();

            isWithinScreen = true;
        }
    }

    // Plays a random moo sfx, called when cows enter player screen
    // (Note: sfx for trucks when entering screen is in truck collision script)
    public void PlayMooSoundEffects()
    {
        //audioSrc = GetComponent<AudioSource>();
        int index = Random.Range(0, mooSounds.Count);
        sfxPlayer.PlayOneShot(mooSounds[index]);
    }

    // Called whe an obstacle got hit by projectile
    public void ObstacleProjectileCollision(GameObject projectile, AudioClip sfxToPlay, int scoreToAdd)
    {
        // Create an explosion effects and play sfx
        Instantiate(explosionEffects, transform.position, Quaternion.identity);
        sfxPlayer.PlayOneShot(sfxToPlay);

        // Destroy the obstacle and the projectile
        Destroy(gameObject);
        Destroy(projectile);

        // Add score
        gameManagerScript.IncreaseScore(scoreToAdd);
    }

    // Instantiate a meat upon destrying cow to make it sems like the cow turned to meat,
    // Called when hitting cow/calf with dagger
    private void TurnCowToMeat()
    {
        GameObject meat = Instantiate(meatPrefab, transform.position + meatSpawnOffset, meatPrefab.transform.rotation);
        // Scale down the meat by half, if it is from a calf
        if (gameManagerScript.NAME_CALVES.Contains(gameObject.name))
        {
            meat.transform.localScale = meatPrefab.transform.lossyScale * 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Destroy all objects with move left when passing the destroy sensor
        if (other.CompareTag(GameManager.TAG_DESTROYSENSOR))
        {
            Destroy(gameObject);
        }

        // Add score based on dash when obatacle got passed point sensor
        else if (CompareTag(GameManager.TAG_OBSTACLE))
        {
            if (other.CompareTag(GameManager.TAG_POINTSENSOR))
            {
                AddScoreBaseOnDash();
            }
        }
    }

    // Add 2 score when getting passed an object while one dash
    // Else add a regular 1 score
    public void AddScoreBaseOnDash()
    {
        if (gameManagerScript.playerIsDashing)
        {
            gameManagerScript.IncreaseScore(2);
        }
        else
        {
            gameManagerScript.IncreaseScore(1);
        }
    }
}
