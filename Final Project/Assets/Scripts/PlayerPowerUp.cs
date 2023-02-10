using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPowerUp : MonoBehaviour
{
    public class PowerUp
    {
        public string name;
        public bool isActivated;
        public float cooldown;
        public int numberLeft;

        public PowerUp(string name, float cooldown)
        {
            this.name = name;
            this.isActivated = false;
            this.cooldown = cooldown;
            this.numberLeft = 0;
        }



    }


    public Dictionary<string, PowerUp> powerUps = new Dictionary<string, PowerUp>();

    GameManager gameManagerScript;

    public GameObject powerUpIndicator;
    public bool hasPowerUp = false;

    private float powerUpIndicatorBlinkDuration = 2.0f;


    private AudioSource playerAudio;
    public AudioClip powerUpPickUpSound;
    public GameObject powerUpPickUpEffects;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = gameObject.GetComponent<AudioSource>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();


        // Create all the powerups with their respective cooldowns
        powerUps.Add("Strength", new PowerUp("Strength", 5.0f));
        powerUps.Add("Bomb", new PowerUp("Bomb", 7.0f));
        powerUps.Add("Dagger", new PowerUp("Dagger", 0.0f));






    }

    // Update is called once per frame
    void Update()
    {
        powerUpIndicator.transform.position = new Vector3(transform.position.x, 0, transform.position.z);


    }

    public void EnablePowerUp(GameObject powerUpBox)
    {
        hasPowerUp = true;
        playerAudio.PlayOneShot(powerUpPickUpSound);
        Instantiate(powerUpPickUpEffects, new Vector3(powerUpBox.transform.position.x, powerUpBox.transform.position.y, powerUpBox.transform.position.z), Quaternion.identity);
        Destroy(powerUpBox);


        // Randomize to include other powerups later, JUST TESTING ONE POWERUP AT A TIEM, CHANGE 2 TO powerUps.Count
        int index = Random.Range(0, powerUps.Count);
        PowerUp currentPowerUp = powerUps.ElementAt(index).Value;

        //currentPowerUp = powerUps.ElementAt(2).Value; //REMOVE FOR TESTING ONLY



        // CHANGE POWERUP INDICATOR FOR EACH POWERUP
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
                break;

        }
        currentPowerUp.isActivated = true;
        Debug.Log($"PICKED UP {currentPowerUp.name} POWERUP");

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
                StartCoroutine("BlinkPowerUpIndicator", powerUpIndicatorBlinkDuration);
            }

            yield return new WaitForSeconds(1);
            cooldown--;
        }

    }

    // Routine for blinking the powerup indicator, called when powerup duration has 3 seconds left
    IEnumerator BlinkPowerUpIndicator(float duration)
    {
        float endTime = Time.time + duration;

        // Turn on and off the powerup indicator until time is up
        while (Time.time < endTime)
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

    // Turn off powerup
    public void TurnOffPowerUp()
    {
        hasPowerUp = false;
        powerUpIndicator.SetActive(false);
        foreach (PowerUp powerUp in powerUps.Values)
        {
            powerUp.isActivated = false;
        }
        Debug.Log("POWERUP ENDED");
    }

}



