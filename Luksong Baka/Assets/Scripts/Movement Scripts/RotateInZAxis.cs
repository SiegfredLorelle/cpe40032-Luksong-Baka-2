using UnityEngine;

public class RotateInZAxis : MonoBehaviour
{
    // Attached to bomb and powerup prefabs

    // The rotate speed will be different for each object
    // (base on the rotateSpeed referenced in the objects inspector)
    public float rotateSpeed;

    void FixedUpdate()
    {
        // Rotates the object in z axis
        transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
    }
}
