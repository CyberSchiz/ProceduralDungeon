using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 30;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Enemy took damage. Health: " + health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}