using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseSpeed = 5f;

    [Header("Stats")]
    public int health = 100;
    public int damage = 10;

    private float currentSpeed;

    private Rigidbody2D rb2D;
    private Vector2 movement;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (rb2D == null)
            rb2D = gameObject.AddComponent<Rigidbody2D>();

        rb2D.gravityScale = 0f;
        rb2D.freezeRotation = true;

        currentSpeed = baseSpeed;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    // ===== BUFF FUNCTIONS =====

    public void Heal(int amount)
    {
        if (health < 100)
        {
            health += amount;
            Debug.Log("Health: " + health);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Took damage. Health: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddSpeedTemporary(float amount, float duration)
    {
        StartCoroutine(SpeedBuff(amount, duration));
    }

    public void AddDamageTemporary(int amount, float duration)
    {
        StartCoroutine(DamageBuff(amount, duration));
    }

    private IEnumerator SpeedBuff(float amount, float duration)
    {
        currentSpeed += amount;
        Debug.Log("Speed buff active");

        yield return new WaitForSeconds(duration);

        currentSpeed -= amount;
        Debug.Log("Speed buff ended");
    }

    private IEnumerator DamageBuff(int amount, float duration)
    {
        damage += amount;
        Debug.Log("Damage buff active");

        yield return new WaitForSeconds(duration);

        damage -= amount;
        Debug.Log("Damage buff ended");
    }
}