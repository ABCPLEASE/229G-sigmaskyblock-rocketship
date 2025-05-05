using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // This will handle collisions between bullets and other objects
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collides with another bullet or object
        if (other.gameObject.CompareTag("EnemyBullet") || other.gameObject.CompareTag("PlayerBullet"))
        {
            // Destroy the bullet that collided with this one
            Destroy(other.gameObject);

            // Destroy this bullet
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Meteorite"))
            {
            other.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
