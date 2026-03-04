using UnityEngine;

public enum ProjectileOwner { Player, Enemy, Neutral }

public class Projectile : MonoBehaviour
{
    public int damage = 50;

    public ProjectileOwner owner = ProjectileOwner.Neutral;

    bool hasHit = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }

    void HandleHit(GameObject other)
    {
        if (hasHit) return;
        hasHit = true;

        if (owner == ProjectileOwner.Player)
        {
            ENEMIES enemy = other.GetComponent<ENEMIES>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }
        else if (owner == ProjectileOwner.Enemy)
        {
            shooting player = other.GetComponent<shooting>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        Destroy(gameObject);
    }
}