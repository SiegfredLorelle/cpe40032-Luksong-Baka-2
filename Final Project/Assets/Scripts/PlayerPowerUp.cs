using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPowerUp : MonoBehaviour
{

    // Powerups related variables
    //public List<string> powerUps = new List<string>{ "Strength", "Flight" };
    //public List<Dictionary<string, float>> powerUps1 = new List<Dictionary<string, float>> { ("Strength", 7.0f), ("name1", 1.0f) };
    //public Dictionary<string, float> powerUps = new Dictionary<string, float>() {
    //        {"Strength", 7.0f },
    //        {"Bomb", 5.0f },
    //        {"Bullet", 7.0f }
    //};

    public class PowerUp
    {
        public string name;
        public float cooldown;
        public bool isActivated;

        public PowerUp(string name, float cooldown)
        {
            this.name = name;
            this.cooldown = cooldown;
            this.isActivated = false;
        }



    }

    GameManager gameManagerScript;

    public Dictionary<string, PowerUp> powerUps = new Dictionary<string, PowerUp>();



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
        powerUps.Add("Strength", new PowerUp("Strength", 7.0f));
        powerUps.Add("Bomb", new PowerUp("Bomb", 7.0f));
        powerUps.Add("Bullet", new PowerUp("Bullet", 7.0f));






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
        int index = Random.Range(0, 2);
        PowerUp currentPowerUp = powerUps.ElementAt(index).Value;


        // CHANGE POWERUP INDICATOR FOR EACH POWERUP
        switch (currentPowerUp.name)
        {
            case "Strength":
                powerUpIndicator.SetActive(true);
                break;
            case "Bomb":
                powerUpIndicator.SetActive(true);
                break;
            case "Bullet":
                break;

        }
        currentPowerUp.isActivated = true;
        StartCoroutine("PowerUpCooldown", currentPowerUp.cooldown);
        Debug.Log($"PICKED UP {currentPowerUp.name} POWERUP");

    }


    // Routine for the duration of powerups
    IEnumerator PowerUpCooldown(float cooldown)
    {
        while (true)
        {
            if (cooldown == 0 || gameManagerScript.isGameStopped)
            {
                hasPowerUp = false;
                powerUpIndicator.SetActive(false);
                foreach (PowerUp powerUp in powerUps.Values)
                {
                    powerUp.isActivated = false;
                }
                Debug.Log("POWERUP ENDED");
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

}

