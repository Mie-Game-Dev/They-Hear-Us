using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth = 100;

    // Deduct health when hit
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        print($"has been damaged {amount}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Destroy the object when health is 0
    private void Die()
    {
        Destroy(gameObject);  // Destroy the enemy gameObject
    }
}
