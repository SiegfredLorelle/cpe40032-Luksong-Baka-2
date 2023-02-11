using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInYAxis : MonoBehaviour
{
    private float speed = 2700.0f;

    // Update is called once per frame
    void Update()
    {
        // Rotates the dagger, just for effects
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
