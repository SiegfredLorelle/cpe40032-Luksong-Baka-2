using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInYAxis : MonoBehaviour
{
    // Attached to dagger prefab and powerup indicator (child of player)

    // The rotate speed will be different for each object
    // (base on the speed entered in the objects inspector)
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        // Rotates the object
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
