using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Powerups related variables
    public List<string> powerUps = new List<string> { "Strength", "Flight" };
    public bool hasPowerUp = false;
    public bool hasStrengthPowerUp = false;
    public bool hasFlightPowerUp = false;
    public float StrengthCooldown = 5.0f;
    public float FlightCooldown = 3.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        yield return new WaitForSeconds(cooldown);
        Debug.Log("POWERUP ENDED");
        hasPowerUp = false;
        hasFlightPowerUp = false;
        hasStrengthPowerUp = false;
    }
}
