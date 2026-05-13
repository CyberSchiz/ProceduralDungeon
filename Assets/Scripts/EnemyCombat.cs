using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackCooldown = 1f;

    private CharacterMovement player;
    private Rigidbody2D rb;
    private float attackTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.GetComponent<CharacterMovement>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= attackRange)
        {
            AttackPlayer();
        }
        else if (distance <= detectionRange)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void AttackPlayer()
    {
        if (attackTimer > 0f)
            return;

        player.TakeDamage(damage);
        attackTimer = attackCooldown;
    }
}