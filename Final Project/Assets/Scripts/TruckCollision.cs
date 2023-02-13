using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCollision : MonoBehaviour
{
    public MoveLeft moveLeftScript;
    public GameManager gameManagerScript;
    public PlayerController playerControllerScript;
    public PlayerPowerUp powerupScript;

    // Start is called before the first frame update
    void Start()
    {
        moveLeftScript = GetComponentInParent<MoveLeft>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        powerupScript = GameObject.Find("Player").GetComponent<PlayerPowerUp>();
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
            playerControllerScript.CollidingWithObstacles();
        }




        // Prevents overlapping trucks (happens when consecutive trucks spawn at small interval)
        if (collision.gameObject.name == "TrailerCollider")
        {
            Destroy(transform.parent.gameObject);
        }


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
