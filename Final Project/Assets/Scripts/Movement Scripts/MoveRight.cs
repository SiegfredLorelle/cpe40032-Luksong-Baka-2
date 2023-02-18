using UnityEngine;

public class MoveRight : MonoBehaviour
{
    // Attached to projectiles, specifically bombs and daggers

    public float moveSpeed = 30.0f;

    void FixedUpdate()
    {
        // Moves the projectile to right
        transform.Translate(Vector3.right * Time.fixedDeltaTime * moveSpeed, relativeTo: Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destroys the projectile upon reaching destroy sensor or projectile x limit sensor
        if (other.CompareTag(GameManager.TAG_DESTROYSENSOR) || other.gameObject.name == GameManager.NAME_PROJECTILEXLIMIT)
        {
            Destroy(gameObject);
        }
    }
}
