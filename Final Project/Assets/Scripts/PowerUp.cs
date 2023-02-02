using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    // Powerups related variables
    //public List<string> powerUps = new List<string>{ "Strength", "Flight" };
    //public List<Dictionary<string, float>> powerUps1 = new List<Dictionary<string, float>> { ("Strength", 7.0f), ("name1", 1.0f) };
    public Dictionary<string, float> powerUps = new Dictionary<string, float>() {
            {"Strength", 7.0f },
            {"Bomb", 5.0f },
            {"Bullet", 7.0f }
    };


    public GameObject powerUpIndicator;
    public bool hasPowerUp = false;
    public bool hasStrengthPowerUp = false;
    public bool hasFlightPowerUp = false;
    public float StrengthCooldown = 5.0f;
    public float FlightCooldown = 3.0f;
    private float powerUpIndicatorBlinkDuration = 2.0f;

    private AudioSource playerAudio;
    public AudioClip powerUpPickUpSound;
    public GameObject powerUpPickUpEffects;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = gameObject.GetComponent<AudioSource>();

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
        Destroy(powerUpBox);
        Destroy(effects, 1.0f);


    // Randomize to include other powerups later, JUST TESTING ONE POWERUP AT A TIEM
    int index = Random.Range(0, powerUps.Count);
        string currentPowerUp = powerUps.ElementAt(index).Key;

        switch (currentPowerUp)
        {
            case "Strength":
                hasStrengthPowerUp = true;
                powerUpIndicator.SetActive(true);
                break;
            case "Bomb":
                hasFlightPowerUp = true;
                break;
            case "Bullet":
                break;

        }
        StartCoroutine("PowerUpCooldown", powerUps[currentPowerUp]);
        Debug.Log($"COLLIDED WITH {currentPowerUp} POWERUP");

    }


    IEnumerator PowerUpCooldown(float cooldown)
    {
        while (true)
        {
            if (cooldown == 0)
            { 
                hasPowerUp = false;
                hasFlightPowerUp = false;
                hasStrengthPowerUp = false;
                powerUpIndicator.SetActive(false);
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
