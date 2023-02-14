using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInZAxis : MonoBehaviour
{
    // Attached to bomb and powerup prefabs

    // The rotate speed will be different for each object
    // (base on the rotateSpeed entered in the objects inspector)
    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        // Rotates the object, just for effects
        transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
    }
}
