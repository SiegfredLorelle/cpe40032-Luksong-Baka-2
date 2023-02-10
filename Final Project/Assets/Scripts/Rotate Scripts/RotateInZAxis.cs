using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInZAxis : MonoBehaviour
{

    float speed;
    // Update is called once per frame
    void Update()
    {
        // Rotates the powerup, just for effects
        switch (tag)
        {
            case "Projectile":
                speed = 500.0f;
                break;
            case "Power Up":
                speed = 90.0f;
                break;
        }
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
}
