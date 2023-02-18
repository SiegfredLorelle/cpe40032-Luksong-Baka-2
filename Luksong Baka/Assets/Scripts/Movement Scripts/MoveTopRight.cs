using UnityEngine;

public class MoveTopRight : MonoBehaviour
{
    // Attached to all obstacles
    // Handles the movement of thrown obstacles (caused by collision with player with strength powerup)

    public MoveLeft moveLeftScript;

    void Start()
    {
        moveLeftScript = GetComponent<MoveLeft>();
    }

    void FixedUpdate()
    {
        // Move the obstacle to the top right if thrown by player
        // Note: If obstacle is thrown then its move left movement is deactivated (not the move left script just the movement within that script)
        if (moveLeftScript.isThrown)
        {
            transform.Translate(new Vector3(1, 1.25f) * 20.0f * Time.deltaTime, relativeTo: Space.World);
            transform.Rotate(new Vector3(0, 0.5f, 1) * 500.0f * Time.deltaTime, relativeTo: Space.World);
        }
    }
}
