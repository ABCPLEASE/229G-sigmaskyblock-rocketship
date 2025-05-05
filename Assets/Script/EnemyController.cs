using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public LayerMask playerLayer;

    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireCooldown = 2f;

    private float fireTimer = 0f;
    private bool playerDetected = false;

    // --- Health ---
    public int maxHealth = 2;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        DetectPlayer();

        if (playerDetected)
        {
            MoveLeftRight();

            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                ShootAtPlayer();
                fireTimer = fireCooldown;
            }
        }
    }

    void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }

        Debug.DrawRay(transform.position, Vector2.down * detectionRange, playerDetected ? Color.red : Color.green);
    }

    void MoveLeftRight()
    {
        float input = Mathf.Sin(Time.time);
        transform.Translate(Vector2.right * input * moveSpeed * Time.deltaTime);
    }

    void ShootAtPlayer()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            Vector2 targetPosition = player.transform.position;
            float timeToHit = 1f;

            Vector2 velocity = CalculateProjectileVelocity(firePoint.position, targetPosition, timeToHit);

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
                rb.linearVelocity = velocity;
            }

            Destroy(proj, 3f);
        }
    }

    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 distance = target - origin;
        float vx = distance.x / time;
        float vy = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        return new Vector2(vx, vy);
    }

    // --- Damage Logic ---
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meteorite") || other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
        }
    }
}