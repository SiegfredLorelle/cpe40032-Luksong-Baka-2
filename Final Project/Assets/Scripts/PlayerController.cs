using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References from other scripts
    public PlayerPowerUp powerUpScript;
    public PlayerHealth healthScript;
    public AudioSource backgroundMusic;
    public SpawnManager spawnManagerScript;
    public UIManager UIManagerScript;
    public GameManager gameManagerScript;

    public GameObject bombPrefab;
    public GameObject daggerPrefab;

    // Effects and Sounds
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public AudioClip damageSound;
    private AudioSource playerAudio;

    // Animation
    public Animator playerAnim;
    private string[] idleAnimations = { "Idle_WipeMouth", "Salute", "Idle_CheckWatch" };
    private float endTimeOfAnimation;
    public float runningAnimationSpeed;
    public float modifiedRunningAnimationSpeed;
    public float jumpingAnimationSpeed;
    public float walkingAnimationSpeed;
    public float deathAnimationSpeed;

    // Movement
    private Rigidbody playerRb;
    public float jumpForce;
    public Vector3 gravityConstant;
    public Vector3 introStartPosition;
    private bool isInIntro;
    private bool isOnGround;
    private bool hasDoubleJumped;


    // Start is called before the first frame update
    void Start()
    {
        // Components of other objects
        powerUpScript = GetComponent<PlayerPowerUp>();
        healthScript = GetComponent<PlayerHealth>();
        spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        UIManagerScript = GameObject.Find("UIManager").GetComponent<UIManager>();
        backgroundMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Component of player
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = gameObject.GetComponent<AudioSource>();

        Physics.gravity = gravityConstant;


        SetupIntro();
    }

    // animation speed has to change in dash mode, but we only do this
    // change if we're on the ground and not dead. if we do perform this
    // function while we're in the air or dead, we'd modify the speed
    // of those animations, and we don't want that.

    public void SpeedUp()
    {
        gameManagerScript.playerIsDashing = true;
        modifiedRunningAnimationSpeed = runningAnimationSpeed * 1.5f;
        if (isOnGround)
        {

            playerAnim.speed = modifiedRunningAnimationSpeed;
            var dirtParticleMain = dirtParticle.main;
            dirtParticleMain.simulationSpeed = 1.5f;
        }
    }

    public void SlowDown()
    {
        gameManagerScript.playerIsDashing = false;
        modifiedRunningAnimationSpeed = runningAnimationSpeed;
        if (isOnGround)
        {

            playerAnim.speed = modifiedRunningAnimationSpeed;
            var dirtParticleMain = dirtParticle.main;
            dirtParticleMain.simulationSpeed = 0.65f;
        }
    }

    // when starting the 'walk-in' intro, we put the player off of the left
    // side of the screen and turn on isInIntro so that the player's controls
    // will be locked in Update().

    // we also invoke PlayerStopDashing so that the dash mode doesn't get
    // 'stuck' if the player was holding shift at the time of death in a
    // previous round.

    public void SetupIntro()
    {

        transform.position = introStartPosition;
        //PlayerStopDashing?.Invoke();
        SlowDown();
        //TransitionToWalking();
        PerformIntro();
        isInIntro = true;
        backgroundMusic.Play();


    }



    // until we reach the position at which the player is to begin running
    // and controls become active, the player character will simply translate
    // right at walking speed.
    //
    // when player has finished intro, an event is fired that GameManager,
    // SpawnManager and PlayerController itself all react to in order to get
    // the gameplay going.

    private void PerformIntro()
    {

        playerAnim.SetBool(GameManager.ANIM_DEATH_B, false);

        playerAnim.SetFloat(GameManager.ANIM_SPEED_F, 0);
        int index = Random.Range(0, idleAnimations.Length);
        playerAnim.Play(idleAnimations[index]);
        endTimeOfAnimation = Time.time + 1.10f;





    }

    private void CheckIntroProgress()
    {
        //Debug.Log(!playerAnim.GetCurrentAnimatorStateInfo(2).IsTag("Idle")) + Tie.;

        // If the idle animations is finished playing
        if (!playerAnim.GetCurrentAnimatorStateInfo(2).IsTag("Idle") && Time.time > endTimeOfAnimation)
        {
            // start running animation
            playerAnim.SetFloat(GameManager.ANIM_SPEED_F, 1.0f);

            playerAnim.SetInteger(GameManager.ANIM_INT, 0);


            // if already on running animation and its transitions are finished
            if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Run_Static"))
            {
                //PlayerFinishedIntro?.Invoke();
                ActivatePlayer();
                spawnManagerScript.StartSpawner();
                gameManagerScript.isGameStopped = false;
            }
        }
    }


    // when isInIntro is false, the player controls are unlocked.
    // we grab our initial running speed and start running!

    private void ActivatePlayer()
    {
        //TransitionToWalking();
        isInIntro = false;
        gameManagerScript.isGameStopped = false;
        modifiedRunningAnimationSpeed = runningAnimationSpeed;
        TransitionToRunning();
    }

    // if we're currently in the intro, the player isn't allowed to do
    // anything but watch, so controls are disabled.

    void Update()
    {


        if (isInIntro)
        {
            CheckIntroProgress();
            return;
        }


        MovePlayer();



    }

    private void MovePlayer()
    {
        // if player presses space and is on the ground,
        // OR if player presses space, is NOT on the ground, and has NOT double jumped yet,
        // let them jump as long as they are ALSO not dead and that the game is not paused

        if (((Input.GetKeyDown(KeyCode.Space) && isOnGround)
            || (Input.GetKeyDown(KeyCode.Space) && !isOnGround && !hasDoubleJumped))
            && !gameManagerScript.isGamePaused
            && !playerAnim.GetBool(GameManager.ANIM_DEATH_B))
        {

            if (!isOnGround)
            {
                PerformDoubleJump();
            }
            else
            {
                PerformJump();
            }
        }

        // when the player PRESSES DOWN on left shift, begin dash mode,
        // as long as he is NOT DEAD.

        // not allowed to speedrun the dramatic dying animation.

        // when the player LETS GO of shift, stop dash mode, as long
        // as he is NOT DEAD - not allowed to make the dying animation
        // go even slower, either.

        // GetKey() is another way to check for whether a player is holding
        // the key down, but we only want to fire these events once each per
        // keypress of shift, so we check instead for the discrete 'down'
        // and 'up'.

        if ((Input.GetKeyDown(KeyCode.LeftShift) && !gameManagerScript.isGameStopped) || powerUpScript.powerUps["Strength"].isActivated)
        {
            SpeedUp();
        }
        else if ((Input.GetKeyUp(KeyCode.LeftShift) && !gameManagerScript.isGameStopped))
        {
            SlowDown();

        }


        // Throw projectile when pressing E
        if (Input.GetKeyDown(KeyCode.E) && !gameManagerScript.isGameStopped && !gameManagerScript.isGamePaused)
        {
            // Throw bomb if bomb powerup is activated
            if (powerUpScript.powerUps["Bomb"].isActivated)
            {
                ThrowBomb();
            }
            // Throw dagger if dagger powerup is enabled and there are daggers left
            else if (powerUpScript.powerUps["Dagger"].isActivated && powerUpScript.powerUps["Dagger"].numberLeft > 0)
            {
                ThrowDagger();
                powerUpScript.powerUps["Dagger"].numberLeft--;
                Debug.Log($"NUMBER OF DAGGERS LEFT: {powerUpScript.powerUps["Dagger"].numberLeft}");

                if (powerUpScript.powerUps["Dagger"].numberLeft == 0)
                {
                    powerUpScript.TurnOffPowerUp();
                }
            }
        }
    }

    // all this does is add the force. this is fired in reaction to the event
    // invoked by the player pressing space.
    //
    // if we're doing a double jump, the force is stronger than a regular
    // jump, to help counteract the downward velocity if the player is
    // currently falling towards the ground. if we didn't add the extra
    // force, trying to do a double jump after you've reached the apex of
    // the regular jump would be fairly useless.
    //
    // the player can jump VERY VERY HIGH if they double-jump while traveling
    // upwards in the first jump, because of how velocity stacks on itself.
    // however, i have decided not to constrain this behavior, because it also
    // makes it so that the player doesn't really know when they're actually
    // going to land. this 'super jump' is thus a very risky move, and it's up
    // to the player if they want to take that risk.

    private void PerformJump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // this is fired when the player presses space but they are already in
    // the air. it simply resets the jump animation trigger so that we can
    // hear the sound again when we jump, and also sets hasDoubleJumped to
    // true so that the player cannot jump again until they hit the ground.
    //
    // with these values set, we just do the normal jump again, along with
    // the jump transition.
    //
    // because of the way animation transitions work, we don't really see
    // the complete jump animation from the beginning on a double jump,
    // but it's 'good enough' in this case, and we do get to hear the sound
    // a second time.

    private void PerformDoubleJump()
    {
        playerAnim.ResetTrigger(GameManager.ANIM_JUMP_TRIG);
        hasDoubleJumped = true;
        PerformJump();
        TransitionToJumping();
    }

    // this handles all of the animation and sound functionality for
    // when we jump.

    private void TransitionToJumping()
    {
        playerAnim.SetBool(GameManager.STATIC_B, true);
        playerAnim.speed = jumpingAnimationSpeed;
        isOnGround = false;
        dirtParticle.Stop();
        playerAnim.SetTrigger(GameManager.ANIM_JUMP_TRIG);
        playerAudio.PlayOneShot(jumpSound);
    }

    // this handles the animation and particles for transitioning
    // from walking to running, or from jumping to running. in
    // addition, this is only called when the player has collided
    // with the ground or right after the intro, so we know that
    // we will be on the ground and our double jump is reset. these
    // bools are set accordingly.

    private void TransitionToRunning()
    {
        playerAnim.SetBool(GameManager.STATIC_B, true);
        playerAnim.ResetTrigger(GameManager.ANIM_JUMP_TRIG);
        playerAnim.SetFloat(GameManager.ANIM_SPEED_F, 1);
        playerAnim.SetBool(GameManager.ANIM_DEATH_B, false);
        playerAnim.speed = modifiedRunningAnimationSpeed;
        isOnGround = true;
        hasDoubleJumped = false;
        dirtParticle.Play();
    }


    // this handles the animation, particles and sound for death.
    //
    // by default, the death animation is very slow, because I
    // accidentally caused it to be that way once and I thought it
    // was a funny William-Shatner-dramatic flair, so I decided to
    // intentionally leave it that way.
    //
    // SlowDown() is added at the beginning in case the player
    // was holding shift at the time of death. it seems that without
    // this, 'turbo speed' animation is still applied, and he does
    // the death animation too fast.

    private void TransitionToDeath()
    {
        SlowDown();
        playerAnim.speed = deathAnimationSpeed;
        playerAnim.SetBool(GameManager.ANIM_DEATH_B, true);
        dirtParticle.Stop();
        explosionParticle.Play();
        playerAudio.PlayOneShot(crashSound);
    }

    private void ThrowBomb()
    {
        GameObject newBomb = Instantiate(bombPrefab, new Vector3(transform.position.x + 1.5f, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
    }

    private void ThrowDagger()
    {
        GameObject newDagger = Instantiate(daggerPrefab, new Vector3(transform.position.x + 1.5f, transform.position.y + 1.5f, transform.position.z), daggerPrefab.transform.rotation);
    }



    private void OnCollisionEnter(Collision other)
    {
        // if the other object that the player has collided with is the ground,
        // AND the player is NOT in the death animation,
        // AND the player is NOT in the intro,
        // fire the 'player has hit the ground' event.
        //
        // PlayerController itself is interested in this event because it is
        // used to control animation.
        //
        // If we are dead or in the intro right now, an animation is already
        // playing of its own accord, and we don't want to interrupt it.

        if (other.gameObject.tag == GameManager.TAG_WALKABLE && !playerAnim.GetBool(GameManager.ANIM_DEATH_B) && !isInIntro)
        {
            TransitionToRunning();
        }

        // If the player has hit an obstacle,
        // raise the event that broadcasts this fact.
        //
        // Spoiler alert: This kills the player.

        if (other.gameObject.CompareTag(GameManager.TAG_OBSTACLE))
        {


            if (powerUpScript.powerUps["Strength"].isActivated)
            {
                // Get the script of the obstacle and set isThrown to true
                // (setting it to true will stop move left movement and enable move top right movement)
                MoveLeft moveLeftScript = other.gameObject.GetComponent<MoveLeft>();
                moveLeftScript.isThrown = true;

                gameManagerScript.IncreaseScore(1);
            }


            else
            {
                healthScript.TakeDamage();
                playerAudio.PlayOneShot(damageSound);
                gameManagerScript.IncreaseScore(-3);
                if (healthScript.currentLives == 0)
                {
                    GameOver();
                }
            }
        }
    }

    public void GameOver()
    {
        gameManagerScript.isGameStopped = true;
        TransitionToDeath();
        spawnManagerScript.GameOver();
        UIManagerScript.GameOver();
        backgroundMusic.Stop();
        playerAnim.SetFloat(GameManager.ANIM_SPEED_F, 0);
        powerUpScript.TurnOffPowerUp();

    }

    // the only thing we care about exiting collision with is the ground,
    // and when we do, fire the event that announces this fact, as long
    // as the player is NOT dead and is NOT in the intro sequence

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == GameManager.TAG_WALKABLE &&
            !playerAnim.GetBool(GameManager.ANIM_DEATH_B) && !isInIntro)
        {
            TransitionToJumping();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Enable powerup when collided with one
        if (other.gameObject.CompareTag(GameManager.TAG_POWERUP))
        {

            powerUpScript.EnablePowerUp(other.gameObject);

        }
    }
}
