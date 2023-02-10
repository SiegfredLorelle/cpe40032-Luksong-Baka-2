using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInZAxis : MonoBehaviour
{

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        // Determine the speed of rotation based on its tag
        switch (tag)
        {
            case "Projectile":
                speed = 500.0f;
                break;
            case "Power Up":
                speed = 90.0f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotates the powerup, just for effects
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
}
