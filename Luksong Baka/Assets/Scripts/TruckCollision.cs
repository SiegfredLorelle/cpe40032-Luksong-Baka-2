using UnityEngine;

public class TruckCollision : MonoBehaviour
{
    // Attached to trucks (including truck and two trailer truck)
    // Controls all collision to trucks

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


    void Start()
    {
        moveLeftScript = GetComponentInParent<MoveLeft>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        powerupScript = GameObject.Find("Player").GetComponent<PlayerPowerUp>();
        sfxPlayer = GameObject.Find("SfxPlayer").GetComponent<AudioSource>();

        trailerCollider = gameObject.transform.parent.Find("TrailerCollider").GetComponent<Collider>();
        trailerTopCollider = gameObject.transform.parent.Find("TrailerTopCollider").GetComponent<Collider>();
        truckCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If this truck collided with player
        if (collision.gameObject.CompareTag(GameManager.TAG_PLAYER))
        {
            // Call colliding with obstacles method to play effects, add score or throw obstacle if has strength powerup
            playerControllerScript.CollidingWithObstacle(transform.parent.gameObject);
            // Upon this initial collision with player, ignore succeeding collisions
            IgnoreCollisionWithPlayer(collision.gameObject);
        }

        // Prevents overlapping trucks (happens when consecutive trucks spawn at small interval)
        if (collision.gameObject.name == "TrailerCollider" && !moveLeftScript.isThrown && !moveLeftScript.isWithinScreen)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    // Called upon initial collision with player, prevents this truck from affecting player's movement
    private void IgnoreCollisionWithPlayer(GameObject player)
    {
        Physics.IgnoreCollision(truckCollider, player.GetComponent<Collider>());
        Physics.IgnoreCollision(trailerCollider, player.GetComponent<Collider>());
        Physics.IgnoreCollision(trailerTopCollider, player.GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        // If this truck collided with a projectile (dagger/bomb), then call obstacle projectile collision method passing the sfx to play
        // obstacle projetile collision method will play effects, add score, and delete both objects
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

        // If this truck collided with the within camera sensor, then play truck horn sfx and set is within screen to true
        else if (other.CompareTag(GameManager.TAG_WITHINCAMERA))
        {
            sfxPlayer.PlayOneShot(truckHornSound);
            moveLeftScript.isWithinScreen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If tis truck passes destroy sensor, then destroy this truck
        if (other.CompareTag(GameManager.TAG_DESTROYSENSOR))
        {
            Destroy(transform.parent.gameObject);
        }
        // If tis truck passes point sensor, then add score base on dash
        else if (other.CompareTag(GameManager.TAG_POINTSENSOR))
        {
            moveLeftScript.AddScoreBaseOnDash();
        }
    }
}
