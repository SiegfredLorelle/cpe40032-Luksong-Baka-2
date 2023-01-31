using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Powerups related variables
    public List<string> powerUps = new List<string> { "Strength", "Flight" };
    public GameObject powerUpIndicator;
    public bool hasPowerUp = false;
    public bool hasStrengthPowerUp = false;
    public bool hasFlightPowerUp = false;
    public float StrengthCooldown = 5.0f;
    public float FlightCooldown = 3.0f;
    private float powerUpIndicatorBlinkDuration = 3.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        powerUpIndicator.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void EnablePowerUp()
    {
        hasPowerUp = true;
        // Randomize to include other powerups later, JUST TESTING ONE POWERUP AT A TIEM
        int index = Random.Range(0,1);
        string currentPowerUp = powerUps[index];

        switch (currentPowerUp)
        {
            case "Strength":
                hasStrengthPowerUp = true;
                Debug.Log($"COLLIDED WITH {currentPowerUp} POWERUP");
                StartCoroutine("PowerUpCooldown", StrengthCooldown);
                powerUpIndicator.SetActive(true);
                break;

            case "Flight":
                hasFlightPowerUp = true;
                Debug.Log($"COLLIDED WITH {currentPowerUp} POWERUP");
                StartCoroutine("PowerUpCooldown", FlightCooldown);
                break;

        }
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
