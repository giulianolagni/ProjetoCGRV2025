using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida do Player")]
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;

    void Start()
    {
        //currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    void Die()
    {
        // aqui você decide o que acontece quando o player morre
        // pode desativar a nave, tocar explosão, trocar de cena etc
        Debug.Log("Player morreu!");
        // por enquanto só desativa o objeto:
        gameObject.SetActive(false);
    }
}
