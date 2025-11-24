using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 4f;

    void Start()
    {
        // --- AQUI ESTÁ A MÁGICA ---
        // A bala pergunta ao DifficultySetup qual é o dano atual
        damage = DifficultySetup.danoGlobalInimigo;
        // ---------------------------

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tenta pegar o script de vida da nave principal (não o PlayerHealth antigo)
            ArcadeNave_VFinal nave = other.GetComponent<ArcadeNave_VFinal>();
            
            if (nave != null)
            {
                nave.TomarDano(damage);
                // Debug para você conferir se mudou
                Debug.Log($"Inimigo acertou! Dano causado: {damage}");
            }
            // Caso você ainda use o sistema antigo em paralelo:
            else 
            {
                 PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                 if (playerHealth != null) playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}