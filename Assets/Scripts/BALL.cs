using UnityEngine;

public class BALL : MonoBehaviour{

    [SerializeField] Transform playerTransform;
    
    [SerializeField] PhysicsMaterial2D bounceMaterial;

    [Header("Parámetros de lanzamiento")]
    [SerializeField] float launchSpeed = 7f;      
    [SerializeField] float horizontalTilt = 1.5f; 
    [SerializeField] float detectEpsilon = 0.05f;

    [SerializeField] bool enforceConstantSpeed = true;
    
    [SerializeField] float constantSpeed = 0f;
    
    [SerializeField] bool allowRotation = false;

    [SerializeField] AudioClip bounceClip;
    
    [Range(0f, 1f)]
    [SerializeField] float bounceVolume = 1f;

    
    bool _hasLaunched;
    Rigidbody2D _playerRb;
    Rigidbody2D _rb;
    Collider2D _col;
    AudioSource _audioSource;
    float _prevPlayerX;
    float _prevXSpeed;
    float _computedConstantSpeed;

    void Awake()
    {
        if (playerTransform == null)
        {
            var go = GameObject.FindWithTag("Player");
            if (go != null) playerTransform = go.transform;
        }

        if (playerTransform != null)
        {
            _playerRb = playerTransform.GetComponent<Rigidbody2D>();
            _prevPlayerX = playerTransform.position.x;
            _prevXSpeed = 0f;
        }

        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        if (_col != null && bounceMaterial != null)
        {
            _col.sharedMaterial = bounceMaterial;
        }

        if (bounceClip != null)
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.playOnAwake = false;
                _audioSource.loop = false;
                _audioSource.spatialBlend = 0f; 
            }
        }

        if (_rb != null)
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _rb.simulated = true;

            _rb.linearDamping = 0f;
            _rb.angularDamping = 0f;
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rb.freezeRotation = !allowRotation;
        }

        if (constantSpeed > 0f)
            _computedConstantSpeed = constantSpeed;
        else
            _computedConstantSpeed = Mathf.Sqrt(horizontalTilt * horizontalTilt + launchSpeed * launchSpeed);
    }

    void Update()
    {
        if (_hasLaunched || playerTransform == null) return;

        float currentXVel = 0f;

        if (_playerRb != null)
        {
            currentXVel = _playerRb.linearVelocity.x;
        }
        else
        {
            float deltaX = playerTransform.position.x - _prevPlayerX;
            currentXVel = deltaX / Mathf.Max(Time.deltaTime, 1e-6f);
        }

        bool wasStationary = Mathf.Abs(_prevXSpeed) <= detectEpsilon;
        bool isMovingNow = Mathf.Abs(currentXVel) > detectEpsilon;

        if (wasStationary && isMovingNow)
        {
            LaunchBall(Mathf.Sign(currentXVel));
        }

        _prevPlayerX = playerTransform.position.x;
        _prevXSpeed = currentXVel;
    }

    void LaunchBall(float directionSign)
    {
        if (_rb == null)
        {
            Debug.LogWarning("BALL: No hay Rigidbody2D en la pelota.");
            _hasLaunched = true;
            return;
        }

        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 0f;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.freezeRotation = !allowRotation;

        Vector2 initial = new Vector2(directionSign * Mathf.Abs(horizontalTilt), Mathf.Abs(launchSpeed));
        if (enforceConstantSpeed && _computedConstantSpeed > 0f)
            _rb.linearVelocity = initial.normalized * _computedConstantSpeed;
        else
            _rb.linearVelocity = initial;

        _rb.angularVelocity = 0f;

        _hasLaunched = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasLaunched || _rb == null) return;

        if (bounceClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(bounceClip, bounceVolume);
        }

        if (enforceConstantSpeed)
        {
            Vector2 v = _rb.linearVelocity;
            if (v.sqrMagnitude > 1e-6f)
            {
                _rb.linearVelocity = v.normalized * _computedConstantSpeed;
            }
        }
    }
}