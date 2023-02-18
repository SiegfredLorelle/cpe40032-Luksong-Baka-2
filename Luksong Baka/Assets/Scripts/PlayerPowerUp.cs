using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPowerUp : MonoBehaviour
{
    // Attached to player in Luksong Baka Scene
    // Manages powerups

    // Pop up message canvas
    public GameObject strengthPopUp;
    public GameObject BombPopUp;
    public GameObject DaggerPopUp;

    // Text of pop up message for dagger
    TextMeshProUGUI daggerPopUpText;

    // Powerup Class
    // Holds a name, status, cooldown, number left and its pop up canvas
    public class PowerUp
    {
        public string name;
        public bool isActivated;
        public float cooldown;
        public int numberLeft;
        public GameObject popUpCanvas;

        // Constructor
        public PowerUp(string name, float cooldown, int numberLeft, GameObject popUpCanvas)
        {
            this.name = name;
            this.isActivated = false;
            this.cooldown = cooldown;
            this.numberLeft = numberLeft;
            this.popUpCanvas = popUpCanvas;
        }
    }

    // Reference from other scripts
    GameManager gameManagerScript;
    PlayerController playerControllerScript;

    // Powerups
    public Dictionary<string, PowerUp> powerUps = new Dictionary<string, PowerUp>();
    public PowerUp currentPowerUp;
    public GameObject powerUpIndicator;
    public bool hasPowerUp = false;
    private float powerUpIndicatorBlinkDuration = 2.0f;
    private float blinkInterval = 0.25f;
    public bool powerUpAboutToRunOut = false;

    // Effects
    private AudioSource sfxPlayer;
    public AudioClip powerUpPickUpSound;
    public AudioClip endOfPowerUpSound;
    public GameObject powerUpPickUpEffects;


    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControllerScript = gameObject.GetComponent<PlayerController>();
        sfxPlayer = GameObject.Find("SfxPlayer").GetComponent<AudioSource>();
        daggerPopUpText = DaggerPopUp.GetComponentInChildren<TextMeshProUGUI>();


        // Create all the powerups with their respective attributes/properties
        powerUps.Add("Strength", new PowerUp("Strength", 5.0f, 0, strengthPopUp));
        powerUps.Add("Bomb", new PowerUp("Bomb", 5.0f, 0, BombPopUp));
        powerUps.Add("Dagger", new PowerUp("Dagger", 0f, 5, DaggerPopUp));

        // Ensures powerups are turend off at the start
        TurnOffPowerUp();
    }

    void Update()
    {
        // The powerup indicator will follow the player while consistently at y axis 0
        powerUpIndicator.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    // Called in player controller script when player picked up a powerup
    // Play effects and enable a random powerup
    public void EnablePowerUp(GameObject powerUpBox)
    {
        hasPowerUp = true;
        sfxPlayer.PlayOneShot(powerUpPickUpSound, 0.5f);
        Instantiate(powerUpPickUpEffects, powerUpBox.transform.position, Quaternion.identity);
        Destroy(powerUpBox);

        // Enable a random powerup
        int index = Random.Range(0, powerUps.Count);
        currentPowerUp = powerUps.ElementAt(index).Value;

        // Manages commands base on what powerup was enabled
        switch (currentPowerUp.name)
        {
            case "Strength":
                powerUpIndicator.SetActive(true);
                StartCoroutine("PowerUpCooldown", currentPowerUp.cooldown);
                break;
            case "Bomb":
                powerUpIndicator.SetActive(true);
                StartCoroutine("PowerUpCooldown", currentPowerUp.cooldown);
                break;
            case "Dagger":
                powerUpIndicator.SetActive(true);
                currentPowerUp.numberLeft = 5;
                UpdateNumOfDaggersInUI();
                break;
        }
        // Turn on powerup and the its indicator
        currentPowerUp.isActivated = true;
        currentPowerUp.popUpCanvas.SetActive(true);
    }


    // Routine for the duration of powerups
    IEnumerator PowerUpCooldown(float cooldown)
    {
        while (true)
        {
            // Turn off the powerup when the cooldown is 0 or the game is over
            if (cooldown == 0 || gameManagerScript.isGameStopped)
            {
                TurnOffPowerUp();
                break;
            }

            // If the powerup is about to run out,
            // Then blink the powerup, and slow down if strength is activated
            else if (cooldown == powerUpIndicatorBlinkDuration)
            {
                StartCoroutine("BlinkPowerUpIndicator");
                powerUpAboutToRunOut = true;
                if (powerUps["Strength"].isActivated)
                {
                    SlowDown();
                }
            }
            yield return new WaitForSeconds(1);
            cooldown--;
        }
    }

    // Called when powerup duration has 3 seconds left or 1 dagger is left
    // Routine for blinking the powerup indicator,
    public IEnumerator BlinkPowerUpIndicator()
    {
        // Turn on and off the powerup indicator until time is up
        while (hasPowerUp)
        {
            if (powerUpIndicator.activeSelf)
            {
                powerUpIndicator.SetActive(false);
            }
            else
            {
                powerUpIndicator.SetActive(true);
            }
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // Called when powerup strength is about to run out,
    // Slows down the player if left shift is not hold
    // (Acts as buffer so player has time to react before losing the powerup)
    private void SlowDown()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            playerControllerScript.SlowDown();
        }
    }

    // Called when cooldown is 0 or game is over
    // Turns off all the powerup
    public void TurnOffPowerUp()
    {
        // Set game states
        hasPowerUp = false;
        powerUpAboutToRunOut = false;

        // Turn off all powerups, their pop up canvsas and the indicator
        foreach (PowerUp powerUp in powerUps.Values)
        {
            powerUp.isActivated = false;
            powerUp.popUpCanvas.SetActive(false);
        }
        powerUpIndicator.SetActive(false);

        // If the player lost while having a powerup,
        // don't play the sfx so it will not overlap with game over sfx
        if (!gameManagerScript.isGameStopped)
        {
            sfxPlayer.PlayOneShot(endOfPowerUpSound);
        }
    }

    // Called in player controller when a dagger is thrown
    // Decrement number of daggers and update the number of daggers in the pop up text of dagger
    public void ReduceDagger()
    {
        powerUps["Dagger"].numberLeft--;
        UpdateNumOfDaggersInUI();
        CheckNumOfDaggers();
    }

    // Called when number of daggers is reduced by 1
    // Update the number of daggers in pop up text of dagger
    private void UpdateNumOfDaggersInUI()
    {
        // If dagger count is not 1, then use the 'daggers' instead of 'dagger'
        if (powerUps["Dagger"].numberLeft != 1)
        {
            daggerPopUpText.text = $"Press E to attack the obstacles using {powerUps["Dagger"].numberLeft} daggers";
        }
        // If dagger count is 1, then use the 'dagger' instead of 'daggers'
        else
        {
            daggerPopUpText.text = $"Press E to attack the obstacles using {powerUps["Dagger"].numberLeft} dagger";
        }
    }

    // Called when number of daggers is reduced by 1
    // Checks how many daggers left
    private void CheckNumOfDaggers()
    {
        // If 1 dager left, then blink powerup indicator
        if (powerUps["Dagger"].numberLeft == 1)
        {
            StartCoroutine(BlinkPowerUpIndicator());

        }
        // If no more daggers, then turn off the powerup
        else if (powerUps["Dagger"].numberLeft == 0)
        {
            TurnOffPowerUp();
        }
    }
}