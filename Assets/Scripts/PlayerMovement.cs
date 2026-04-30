using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D rb2D;
    private Vector2 movement;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        if (rb2D == null)
            rb2D = gameObject.AddComponent<Rigidbody2D>();

        rb2D.gravityScale = 0f;
        rb2D.freezeRotation = true;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movement * speed * Time.fixedDeltaTime);
    }
}