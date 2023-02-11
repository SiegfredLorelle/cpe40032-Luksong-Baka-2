using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Attached to most objects in the scene including obstacles, powerups, and background

    public GameManager gameManagerScript;
    public GameObject meatPrefab;
    public bool isThrown;

    private float moveSpeed;
    private float modifiedMoveSpeed;

    // Variables used by some but not all objects
    public ParticleSystem explosionEffects;
    public AudioSource audioExplosion;


    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        moveSpeed = 20f;
        modifiedMoveSpeed = moveSpeed;
    }

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

        // Do actual movement, with modifiedMoveSpeed and relative to world (so that objects will always go left with respect to the world/camera regardless of their rotation)
        transform.Translate(Vector3.left * Time.fixedDeltaTime * modifiedMoveSpeed, relativeTo: Space.World);

        // If object is not background and is out of bounds, then destroy it
        if (!CompareTag(GameManager.TAG_BACKGROUND) && (transform.position.x < -10 || transform.position.y < -2 || transform.position.y > 10))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Catches trigger collision between obstacle and projectiles
        if (CompareTag(GameManager.TAG_OBSTACLE) && other.CompareTag(GameManager.TAG_PROJECTILE))
        {
            // If the projectile is a bomb
            if (other.gameObject.name == GameManager.NAME_BOMB)
            {
                // Get Audio Source and play the explosion clip assigned to it
                audioExplosion = other.GetComponent<AudioSource>();
                audioExplosion.Play();

                // Turn off render and collider, to prevent it from affecting other objects while the explosion sfx is still playing
                other.gameObject.GetComponent<CapsuleCollider>().enabled = false;

                // Destroy bomb as soon as its explosion sfx is finished (TODO: bring down below so that it also destroys for dagger)
                Destroy(other.gameObject, audioExplosion.clip.length);
            }

            // If the projectile is a dagger
            else if (other.gameObject.name == GameManager.NAME_DAGGER)
            {
                // Turn off collider, to prevent it from affecting other objects while the explosion sfx is still playing
                other.gameObject.GetComponent<BoxCollider>().enabled = false;

                Destroy(other.gameObject); // TODO: Remove later since the destroy after sfx will be used when dagger has sfx

                // If the obstacle hit by dagger is a cow, then turn it into meat (TODO: Add aditional score when turning cow to meat)
                if (gameManagerScript.NAME_COWS.Contains(gameObject.name))
                {
                    Instantiate(meatPrefab, new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z + 1.5f), meatPrefab.transform.rotation);
                    gameManagerScript.IncreaseScore(1);
                }
            }

            // Turn of render so it appears destroyed, then actually destroy it after playing sfx
            other.gameObject.GetComponent<Renderer>().enabled = false;

            // Add score
            gameManagerScript.IncreaseScore(1);

            // Create an explosion effects and destroy the obstacle hit
            Instantiate(explosionEffects, transform.position, Quaternion.identity);
            Destroy(gameObject);



        }


    }

    private void OnTriggerExit(Collider other)
    {
       if (CompareTag(GameManager.TAG_OBSTACLE) && other.CompareTag(GameManager.TAG_POINTSENSOR))
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
}
