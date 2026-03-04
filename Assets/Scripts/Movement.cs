using UnityEngine;
using UnityEngine.InputSystem;

public class MOVEMENT : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    Rigidbody2D physic;
    float movement;

    void Awake()
    {
        physic = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                movement = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                movement = 1f;
            }
            else
            {
                movement = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (physic != null)
        {
            physic.linearVelocity = new Vector2(movement * speed, physic.linearVelocity.y);
        }
        else
        {
            transform.Translate(new Vector3(movement * speed * Time.fixedDeltaTime, 0f, 0f));
        }
    }
}
