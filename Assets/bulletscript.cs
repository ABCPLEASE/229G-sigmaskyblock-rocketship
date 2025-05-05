using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy both bullets if they collide
        if (other.CompareTag("EnemyBullet") || other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        // Handle meteorite collision
        if (CompareTag("PlayerBullet")&& other.CompareTag("Meteorite"))
        {
            other.gameObject.SetActive(false);
            Destroy(gameObject);

            // Add score when player destroys a meteorite
            ScoreManager.Instance.AddScore(2); // Award 2 points
        }

   
    }
}
