using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float boostedSpeed = 10f;

    public Transform leftBoundary;
    public Transform rightBoundary;

    // Booster effect
    public GameObject boosterEffect;

    // Projectile related variables
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int maxAmmo = 10;
    private int currentAmmo;

    public BackgroundScroller bgScroller;
    public float normalScrollSpeed = 2f;
    public float boostedScrollSpeed = 5f;

    // --- HP System ---
    public int maxHealth = 5;
    private int currentHealth;
    public float invincibleTime = 1f;
    private bool isInvincible = false;
    public HealthUI healthUI; // Drag the UI script here in Inspector

    // --- SFX ---
    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        healthUI?.UpdateHealth(currentHealth, maxHealth); // Initial update

        if (leftBoundary.position.x > rightBoundary.position.x)
        {
            var temp = leftBoundary;
            leftBoundary = rightBoundary;
            rightBoundary = temp;
        }

        float middleX = (leftBoundary.position.x + rightBoundary.position.x) / 2f;
        transform.position = new Vector3(middleX, transform.position.y, transform.position.z);

        if (boosterEffect != null)
            boosterEffect.SetActive(false);

        if (bgScroller != null)
            bgScroller.scrollSpeed = normalScrollSpeed;

        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool isBoosting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float moveSpeed = isBoosting ? boostedSpeed : normalSpeed;

        float moveInput = Input.GetAxisRaw("Horizontal");
        Vector3 newPosition = transform.position + new Vector3(moveInput * moveSpeed * Time.deltaTime, 0, 0);
        float clampedX = Mathf.Clamp(newPosition.x, leftBoundary.position.x, rightBoundary.position.x);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        if (boosterEffect != null)
            boosterEffect.SetActive(isBoosting);

        if (bgScroller != null)
            bgScroller.scrollSpeed = isBoosting ? boostedScrollSpeed : normalScrollSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && currentAmmo > 0)
        {
            Shoot();
            currentAmmo--;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadAmmo();
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Vector2 targetPosition = firePoint.position + new Vector3(0, 15f, 0);
            float time = 1.0f;
            Vector2 velocity = CalculateProjectileVelocity(firePoint.position, targetPosition, time);

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 1f;
                rb.linearVelocity = velocity;
            }

            Destroy(proj, 2f);
        }

        // Play shoot sound effect
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    Vector2 CalculateProjectileVelocity(Vector2 origin, Vector2 target, float time)
    {
        Vector2 distance = target - origin;
        float vx = distance.x / time;
        float vy = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;
        return new Vector2(vx, vy);
    }

    void ReloadAmmo()
    {
        currentAmmo = maxAmmo;

        // Play reload sound effect
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }
    }

    // --- HP Methods ---
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        healthUI?.UpdateHealth(currentHealth, maxHealth);

        // Play damage sound effect
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invincibility());
        }
    }

    System.Collections.IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player Died!");
        gameObject.SetActive(false); // Or play animation, trigger game over, etc.
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Meteorite") || other.CompareTag("EnemyBullet"))
        {
            TakeDamage(1);
        }
    }
}
