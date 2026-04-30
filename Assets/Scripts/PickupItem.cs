using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private string itemName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Debug.Log("Picked up: " + itemName);

        // Later apply buffs/effects here
        Destroy(gameObject);
    }
}