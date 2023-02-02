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



    public Dictionary<string, PowerUp> powerUps = new Dictionary<string, PowerUp>();




    public GameObject powerUpIndicator;
    public bool hasPowerUp = false;
    //public bool hasStrengthPowerUp = false;
    //public bool hasFlightPowerUp = false;
    //public float StrengthCooldown = 5.0f;
    //public float FlightCooldown = 3.0f;
    private float powerUpIndicatorBlinkDuration = 2.0f;


    private AudioSource playerAudio;
    public AudioClip powerUpPickUpSound;
    public GameObject powerUpPickUpEffects;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = gameObject.GetComponent<AudioSource>();

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
        GameObject effects = Instantiate(powerUpPickUpEffects, new Vector3(powerUpBox.transform.position.x, powerUpBox.transform.position.y, powerUpBox.transform.position.z), Quaternion.identity);
        Destroy(effects, 1.0f);
        Destroy(powerUpBox);


        // Randomize to include other powerups later, JUST TESTING ONE POWERUP AT A TIEM
        int index = Random.Range(0, powerUps.Count);
        PowerUp currentPowerUp = powerUps.ElementAt(index).Value;

        switch (currentPowerUp.name)
        {
            case "Strength":
                powerUpIndicator.SetActive(true);
                break;
            case "Bomb":
                break;
            case "Bullet":
                break;

        }
        currentPowerUp.isActivated = true;
        StartCoroutine("PowerUpCooldown", currentPowerUp.cooldown);
        Debug.Log($"COLLIDED WITH {currentPowerUp.name} POWERUP");

    }


    IEnumerator PowerUpCooldown(float cooldown)
    {
        while (true)
        {
            if (cooldown == 0)
            {
                hasPowerUp = false;
                powerUpIndicator.SetActive(false);
                foreach(PowerUp powerUp in powerUps.Values)
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

