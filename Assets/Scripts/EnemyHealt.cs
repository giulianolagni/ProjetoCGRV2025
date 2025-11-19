using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Vida máxima do inimigo")]
    public float maxHealth = 50f;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Aqui você pode colocar animação de explosão, som, etc.
        Destroy(gameObject);
    }
}
