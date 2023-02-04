using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // we have this event so that the GameManager can know
    // an object has despawned; despawning an object causes
    // the GameManager to increase the score.
    public delegate void DespawnEvent();
    public static event DespawnEvent Despawn;

    // there are two movespeed variables because of dash mode.

    private float moveSpeed;
    private float modifiedMoveSpeed;


    public bool isThrown;

    // Variables which not all objects has something assigned to it
    public ParticleSystem explosionEffects;
    public Renderer rend;
    public AudioSource audioExplosion;

    // we need to know about the player going into and out of
    // dash mode, so we subscribe to those two events.

    void Start()
    {
        moveSpeed = 20f;
        modifiedMoveSpeed = moveSpeed;
        PlayerController.PlayerStartDashing += SpeedUp;
        PlayerController.PlayerStopDashing += SlowDown;
    }

    // since this script is attached to both the background
    // and the obstacles, it has to act slightly differently for
    // each. for obstacles, when they are enabled, they are placed
    // off of the right side of the screen

    void OnEnable()
    {
        if (gameObject.tag == GameManager.TAG_OBSTACLE)
        {
            transform.position = new Vector3(25, 0, 0);
            //transform.rotation = Quaternion.identity;
        }
    }

    // these are fired when the player uses dash mode.
    // we will move the background and objects twice as
    // fast when the player is dashing

    void SpeedUp() => modifiedMoveSpeed = moveSpeed * 2f;
    void SlowDown() => modifiedMoveSpeed = moveSpeed;

    void FixedUpdate()
    {
        // do nothing if the game is stopped

        if (GameManager.Instance.isGameStopped)
        {
            return;
        }
        else if (!GameManager.Instance.isGameStopped && !isThrown)
        {
            // set our modifiedMoveSpeed according to whether the player
            // is currently dashing or not
            if (GameManager.Instance.playerIsDashing)
            {
                modifiedMoveSpeed = moveSpeed * 2f;
            }
            else
            {
                modifiedMoveSpeed = moveSpeed;
            }

            // do our actual movement, with our modifiedMoveSpeed applied
            transform.Translate(Vector3.left * Time.fixedDeltaTime * modifiedMoveSpeed, relativeTo: Space.World);

        }



        // finally, if we're an obstacle and we've gone out of bounds,
        // despawn self and notify anyone who is interested in this
        // event (specifically, the GameManager)

        if (gameObject.tag == GameManager.TAG_OBSTACLE && (transform.position.x < -10 || transform.position.y < -2 || transform.position.y > 10))
        {
            Despawn?.Invoke();

            Destroy(gameObject);
        }

        else if (gameObject.tag == GameManager.TAG_POWERUP && (transform.position.x < -10))
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag(GameManager.TAG_OBSTACLE) && other.CompareTag(GameManager.TAG_PROJECTILE))
        {
            // Call despawn method from game maanger script, mainly to add a score
            Despawn?.Invoke();

            // Create an explosion effects and destroy the obstacle hit
            Instantiate(explosionEffects, transform.position, Quaternion.identity);
            Destroy(gameObject);

            // Get Audio Source and play the explosion clip assigned to it
            audioExplosion = other.GetComponent<AudioSource>();
            audioExplosion.Play();

            // Turn off render and collider, to prevent it from affecting other objects while the explosion sfx is still playing
            other.gameObject.GetComponent<Renderer>().enabled = false;
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;

            // Destroy bomb as soon as its explosion sfx is finished
            Destroy(other.gameObject, audioExplosion.clip.length);
        }
    }
}
