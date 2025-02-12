using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50; // Máu tối đa
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // Khởi tạo máu khi bắt đầu game
    }

    // Gọi hàm này khi Player bị tấn công
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player bị tấn công, máu còn lại: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player đã chết!");
        // Ở đây có thể thêm animation chết hoặc restart game
        gameObject.SetActive(false); // Ẩn Player khi chết
    }

    // Gọi hàm này khi Player hồi máu
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("Player hồi máu, máu hiện tại: " + currentHealth);
    }
}
