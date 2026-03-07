using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class shooting : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float projectileSpeed = 8f;
    [SerializeField] int health = 100;

    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;

    Rigidbody2D physic;
    float movement = 0f;

    void Awake()
    {
        physic = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    void Update()
    {
        movement = 0f;
        if (Keyboard.current != null)
        {
            // Movimiento
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                movement = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                movement = 1f;
            }

            // Disparo
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Shoot();
            }
        }

        if (SCORE_MANAGER.Instance != null)
        {
            SCORE_MANAGER.Instance.UpdateHealth(health);
        }
    }

    void FixedUpdate()
    {
        if (physic != null)
        {
            physic.linearVelocity = new Vector2(movement * moveSpeed, physic.linearVelocity.y);
        }
        else
        {
            transform.Translate(new Vector3(movement * moveSpeed * Time.fixedDeltaTime, 0f, 0f));
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("shooting: Prefab de proyectil o punto de spawn no asignado.");
            return;
        }

        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.gravityScale = 0f;
            projectileRb.linearVelocity = spawnPoint.up * projectileSpeed;
        }

        Projectile projComp = projectile.GetComponent<Projectile>();
        if (projComp != null)
        {
            projComp.owner = ProjectileOwner.Player;
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log($"Player took {dmg} damage, health now {health}");

        if (health <= 0)
        {
            if (SCORE_MANAGER.Instance != null)
            {
                SCORE_MANAGER.Instance.ResetScore();
            }
            Destroy(gameObject);
            SceneManager.LoadScene("SpaceInvaders");
        }
    }
}