using UnityEngine;

public enum PotionType
{
    Health,
    Speed,
    Damage,
    Trap
}

public class PotionPickup : MonoBehaviour
{
    [SerializeField] private PotionType potionType;
    [SerializeField] private float value = 10f;
    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        CharacterMovement player = collision.GetComponent<CharacterMovement>();

        if (player == null)
            return;

        switch (potionType)
        {
            case PotionType.Health:
                player.Heal((int)value);
                break;

            case PotionType.Speed:
                player.AddSpeedTemporary(value, duration);
                break;

            case PotionType.Damage:
                player.AddDamageTemporary((int)value, duration);
                break;

            case PotionType.Trap:
                player.TakeDamage((int)value);
                break;
        }

        Destroy(gameObject);
    }
}