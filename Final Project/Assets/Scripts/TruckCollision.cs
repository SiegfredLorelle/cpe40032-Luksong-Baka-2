using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCollision : MonoBehaviour
{
    public MoveLeft moveLeftScript;
    public GameManager gameManagerScript;
    public PlayerController playerControllerScript;
    public PlayerPowerUp powerupScript;

    private Collider truckCollider;
    private Collider trailerCollider;
    private Collider trailerTopCollider;

    public AudioSource sfxPlayer;
    public AudioClip truckHornSound;
    public AudioClip metalHitSound;
    public AudioClip explosionSound;


    // Start is called before the first frame update
    void Start()
    {
        moveLeftScript = GetComponentInParent<MoveLeft>();
        sfxPlayer = GameObject.Find("SfxPlayer").GetComponent<AudioSource>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        powerupScript = GameObject.Find("Player").GetComponent<PlayerPowerUp>();

        trailerCollider = gameObject.transform.parent.Find("TrailerCollider").GetComponent<Collider>();
        trailerTopCollider = gameObject.transform.parent.Find("TrailerTopCollider").GetComponent<Collider>();
        truckCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GameManager.TAG_PLAYER))
        {
            // Get the script of the obstacle and set isThrown to true
            // (setting it to true will stop move left movement and enable move top right movement)
            if (powerupScript.powerUps["Strength"].isActivated)
            {
                moveLeftScript.isThrown = true;
            }
            playerControllerScript.CollidingWithObstacle(transform.parent.gameObject);
            IgnoreCollisionWithPlayer(collision.gameObject);
        }

        // Prevents overlapping trucks (happens when consecutive trucks spawn at small interval)
        if (collision.gameObject.name == "TrailerCollider" && !moveLeftScript.isThrown && !moveLeftScript.isWithinScreen)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void IgnoreCollisionWithPlayer(GameObject player)
    {
        Physics.IgnoreCollision(truckCollider, player.GetComponent<Collider>());
        Physics.IgnoreCollision(trailerCollider, player.GetComponent<Collider>());
        Physics.IgnoreCollision(trailerTopCollider, player.GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_PROJECTILE))
        {
            if (other.name == GameManager.NAME_BOMB)
            {
                moveLeftScript.ObstacleProjectileCollision(other.gameObject, explosionSound, 1);
            }
            else if (other.name == GameManager.NAME_DAGGER)
            {
                moveLeftScript.ObstacleProjectileCollision(other.gameObject, metalHitSound, 1);

            }
        }

        else if (other.CompareTag(GameManager.TAG_WITHINCAMERA))
        {
            sfxPlayer.PlayOneShot(truckHornSound);
            moveLeftScript.isWithinScreen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_DESTROYSENSOR))
        {
            Destroy(transform.parent.gameObject);
        }

        else if (other.CompareTag(GameManager.TAG_POINTSENSOR))
        {
            moveLeftScript.AddScoreBaseOnDash();
        }
    }
}
