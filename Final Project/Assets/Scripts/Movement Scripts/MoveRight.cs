using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : MonoBehaviour
{
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 30.0f;
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.right * Time.fixedDeltaTime * moveSpeed, relativeTo: Space.World);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_DESTROYSENSOR))
        {
            Destroy(gameObject);
        }
    }
}
