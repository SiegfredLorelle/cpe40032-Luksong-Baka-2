using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreen : MonoBehaviour
{
    public GameManager gameManagerScript;
    public AnimationCurve curve;
    public float duration = 1f;


    private void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // shakescreen
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {  
            //if (!gameManagerScript.isGamePaused)
            //{ 
                elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(elapsedTime / duration);
                transform.position = startPosition + Random.insideUnitSphere * strength;
                yield return null;
            //}
        }
        transform.position = startPosition;
    }

    public void StartShaking()
    { 
            StartCoroutine(Shaking());

    }
}
