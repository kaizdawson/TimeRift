using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage, transform);
        }
    }
}
