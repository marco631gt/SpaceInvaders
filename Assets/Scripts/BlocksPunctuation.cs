using System.Collections;
using UnityEngine;

public class BLOCKS_PUNCTUATION : MonoBehaviour
{
    [SerializeField] int points = 100;
    Collider2D _col;
    SpriteRenderer _sr;
    bool _destroying;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_destroying) return;

        if (collision.collider.GetComponent<BALL>() != null)
        {
            StartCoroutine(DestroyAfterBounce());
        }
    }

    IEnumerator DestroyAfterBounce()
    {
        _destroying = true;

        
        if (_col != null) _col.enabled = false;
        if (_sr != null) _sr.enabled = false;

        yield return new WaitForFixedUpdate();

        if (SCORE_MANAGER.Instance != null)
            SCORE_MANAGER.Instance.AddPoints(points);
        else
            Debug.LogWarning("BLOCKS_PUNCTUATION: SCORE_MANAGER no encontrado en la escena.");

        
        Destroy(gameObject);
    }
}