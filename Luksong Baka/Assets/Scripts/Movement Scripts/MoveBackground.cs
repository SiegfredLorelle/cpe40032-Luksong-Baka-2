using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    // Attached to background
    // Move left is also attached to background, this script main purpose is to reset the background on half

    private Vector3 startPos = new Vector3(45, 8.1f, 10);

    void Update()
    {
        // reset position if we've moved further than half our background width
        if (transform.position.x < startPos.x - gameObject.GetComponent<SpriteRenderer>().bounds.extents.x)
        {
            transform.position = startPos;
        }
    }
}
