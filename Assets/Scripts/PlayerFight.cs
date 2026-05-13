using UnityEngine;

public class PlayerFight : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private LayerMask enemyLayer;

    private CharacterMovement playerStats;

    private void Start()
    {
        playerStats = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, attackRange, enemyLayer);

        if (enemy != null)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(playerStats.damage);
            }
        }
    }
}