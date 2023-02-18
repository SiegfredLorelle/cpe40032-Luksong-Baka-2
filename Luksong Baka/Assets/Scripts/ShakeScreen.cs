using System.Collections;
using UnityEngine;

public class ShakeScreen : MonoBehaviour
{
    // Attached to the main camera
    // Shakes the screen when player collided with an obstacle

    public GameManager gameManagerScript;
    public AnimationCurve curve;
    public float duration = 1f;

    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Shake the screen for 1 second
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Don't shake the screen if the game is paused
            if (gameManagerScript.isGamePaused)
                yield return new WaitForEndOfFrame();

            else
            { 
                float strength = curve.Evaluate(elapsedTime / duration);
                transform.position = startPosition + Random.insideUnitSphere * strength;
                yield return null;
            }
        }
        transform.position = startPosition;
    }

    // Called in player controller script when player collided with an obstacle
    // Start the shaking routine
    public void StartShaking()
    { 
        StartCoroutine(Shaking());
    }
}
