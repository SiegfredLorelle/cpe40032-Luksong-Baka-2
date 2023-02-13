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

    // Start is called before the first frame update
    void Start()
    {
        moveLeftScript = GetComponentInParent<MoveLeft>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        powerupScript = GameObject.Find("Player").GetComponent<PlayerPowerUp>();

        trailerCollider = gameObject.transform.parent.Find("TrailerCollider").GetComponent<Collider>();
        trailerTopCollider = gameObject.transform.parent.Find("TrailerTopCollider").GetComponent<Collider>();
        truckCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            //MoveLeft moveLeftScript = other.gameObject.GetComponent<MoveLeft>();
            playerControllerScript.CollidingWithObstacle(collision.gameObject);
            IgnoreCollisionWithPlayer(collision.gameObject);
        }



        // Prevents overlapping trucks (happens when consecutive trucks spawn at small interval)
        if (collision.gameObject.name == "TrailerCollider")
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
                moveLeftScript.BombExplosion(other.gameObject);
            }
            else if (other.name == GameManager.NAME_DAGGER)
            {
                moveLeftScript.DaggerHitObstacle(other.gameObject);
            }
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
