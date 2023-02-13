using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreen : MonoBehaviour
{
    // Shakescreen
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }
    // shakescreen
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = startPosition;
    }
}
