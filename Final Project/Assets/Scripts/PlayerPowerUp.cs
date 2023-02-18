using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerPowerUp : MonoBehaviour
{
    public GameObject strengthPopUp;
    public GameObject BombPopUp;
    public GameObject DaggerPopUp;

    TextMeshProUGUI daggerPopUpText;

    public class PowerUp
    {
        public string name;
        public bool isActivated;
        public float cooldown;
        public int numberLeft;
        public GameObject popUpCanvas;

        public PowerUp(string name, float cooldown, int numberLeft, GameObject popUpCanvas)
        {
            this.name = name;
            this.isActivated = false;
            this.cooldown = cooldown;
            this.numberLeft = numberLeft;
            this.popUpCanvas = popUpCanvas;
        }
    }


    public PowerUp currentPowerUp;
    public Dictionary<string, PowerUp> powerUps = new Dictionary<string, PowerUp>();
    GameManager gameManagerScript;
    PlayerController playerControllerScript;

    public GameObject powerUpIndicator;
    public bool hasPowerUp = false;

    private float powerUpIndicatorBlinkDuration;
    public bool powerUpAboutToRunOut;


    private AudioSource playerAudio;
    public AudioClip powerUpPickUpSound;
    public AudioClip endOfPowerUpSound;
    public GameObject powerUpPickUpEffects;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = gameObject.GetComponent<AudioSource>();
        playerControllerScript = gameObject.GetComponent<PlayerController>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        daggerPopUpText = DaggerPopUp.GetComponentInChildren<TextMeshProUGUI>();

        // Create all the powerups with their respective cooldowns
        powerUps.Add("Strength", new PowerUp("Strength", 5.0f, 0, strengthPopUp));
        powerUps.Add("Bomb", new PowerUp("Bomb", 5.0f, 0, BombPopUp));
        powerUps.Add("Dagger", new PowerUp("Dagger", 0f, 5, DaggerPopUp));

        // Set values to other variables
        powerUpIndicatorBlinkDuration = 2.0f;
        powerUpAboutToRunOut = false;

        TurnOffPowerUp();

    }

    // Update is called once per frame
    void Update()
    {
        powerUpIndicator.transform.position = new Vector3(transform.position.x, 0, transform.position.z);


    }

    public void EnablePowerUp(GameObject powerUpBox)
    {
        hasPowerUp = true;
        playerAudio.PlayOneShot(powerUpPickUpSound, 0.5f);
        Instantiate(powerUpPickUpEffects, powerUpBox.transform.position, Quaternion.identity);
        Destroy(powerUpBox);


        // Randomize to include other powerups later, JUST TESTING ONE POWERUP AT A TIEM, CHANGE 2 TO powerUps.Count
        int index = Random.Range(0, powerUps.Count);
        currentPowerUp = powerUps.ElementAt(index).Value;

        //currentPowerUp = powerUps.ElementAt(0).Value; //REMOVE FOR TESTING ONLY



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
        currentPowerUp.isActivated = true;
        currentPowerUp.popUpCanvas.SetActive(true);
    }


    // Routine for the duration of powerups
    IEnumerator PowerUpCooldown(float cooldown)
    {
        while (true)
        {
            if (cooldown == 0 || gameManagerScript.isGameStopped)
            {
                TurnOffPowerUp();
                break;
            }

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

    // Routine for blinking the powerup indicator, called when powerup duration has 3 seconds left or 1 dagger is left
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
            yield return new WaitForSeconds(0.25f);
        }
    }

    // Called when powerup strength is about to run out,
    // slows down the player if left shift is not hold
    // (acts as buffer so player has time to react before losing the powerup)
    private void SlowDown()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            playerControllerScript.SlowDown();
        }
    }

    // Turn off powerup
    public void TurnOffPowerUp()
    {
        hasPowerUp = false;
        powerUpAboutToRunOut = false;
        powerUpIndicator.SetActive(false);
        
        foreach (PowerUp powerUp in powerUps.Values)
        {
            powerUp.isActivated = false;
            powerUp.popUpCanvas.SetActive(false);
        }

        // If the player lost while having a powerup,
        // don't play the sfx so it will not overlap with game over sfx
        if (!gameManagerScript.isGameStopped)
        {
            playerAudio.PlayOneShot(endOfPowerUpSound);
        }

    }

    public void ReduceDagger()
    {
        powerUps["Dagger"].numberLeft--;
        UpdateNumOfDaggersInUI();
        CheckNumOfDaggers();
    }

    private void UpdateNumOfDaggersInUI()
    {
        if (powerUps["Dagger"].numberLeft != 1)
        { 
            daggerPopUpText.text = $"Press E to attack the obstacles using {powerUps["Dagger"].numberLeft} daggers";
        }
        else 
        {
            daggerPopUpText.text = $"Press E to attack the obstacles using {powerUps["Dagger"].numberLeft} dagger";
        }
    }

    private void CheckNumOfDaggers()
    {
        if (powerUps["Dagger"].numberLeft == 1)
        {
            StartCoroutine(BlinkPowerUpIndicator());

        }
        else if (powerUps["Dagger"].numberLeft == 0)
        {
            TurnOffPowerUp();

        }
    }

}