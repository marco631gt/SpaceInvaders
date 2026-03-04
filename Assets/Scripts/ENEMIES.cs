using UnityEngine;
using UnityEngine.InputSystem;

public class ENEMIES : MonoBehaviour
{
    [Header("Disparo")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float fireRate = 2f;

    [Header("Patrulla")]
    [SerializeField] float patrolDistance = 3f;
    [SerializeField] float moveSpeed = 1f;

    [Header("Stats")]
    [SerializeField] int damage = 50;
    [SerializeField] int health = 100;
    [SerializeField] int scoreOnDeath = 100;

    Rigidbody2D physic;
    float movement = 0f;
    

    // Patrulla
    bool movingRight = true;
    float initialX;
    float leftLimitX;
    float rightLimitX;

    // Disparo
    float fireTimer = 0f;

    

    void Awake()
    {
        physic = GetComponent<Rigidbody2D>();

        initialX = transform.position.x;
        float half = Mathf.Abs(patrolDistance) * 0.5f;
        leftLimitX = initialX - half;
        rightLimitX = initialX + half;
    }

    void OnValidate()
    {
        // Mantener límites actualizados en el editor
        if (!Application.isPlaying)
        {
            initialX = transform != null ? transform.position.x : 0f;
            float half = Mathf.Abs(patrolDistance) * 0.5f;
            leftLimitX = initialX - half;
            rightLimitX = initialX + half;
        }
    }

    void Update()
    {
        Patrol();

        // Disparo periódico (en línea recta)
        if (projectilePrefab != null && spawnPoint != null)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                ShootStraight();
            }
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

    void Patrol()
    {
        float x = transform.position.x;

        if (movingRight && x >= rightLimitX)
            movingRight = false;
        else if (!movingRight && x <= leftLimitX)
            movingRight = true;

        movement = movingRight ? 1f : -1f;
    }

    void ShootStraight()
    {
        if (projectilePrefab == null || spawnPoint == null)
            return;

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.gravityScale = 0f;

            Vector2 dir = spawnPoint.up;
            projectileRb.linearVelocity = dir * projectileSpeed;
        }

        Projectile projComp = projectile.GetComponent<Projectile>();
        if (projComp != null)
        {
            projComp.damage = damage;
            projComp.owner = ProjectileOwner.Enemy;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            if (SCORE_MANAGER.Instance != null)
                SCORE_MANAGER.Instance.AddPoints(scoreOnDeath);
                
                

            Destroy(gameObject);
        }
    }
}