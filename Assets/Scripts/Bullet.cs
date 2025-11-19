using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Quanto tempo o projétil fica vivo antes de sumir")]
    public float lifeTime = 3f;

    [Tooltip("Dano que o projétil causa em inimigos")]
    public float damage = 10f;

    void Start()
    {
        // Destroi o projétil depois de um tempo para não lotar a cena
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Só colide com objetos marcados como 'Enemy'
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Destroi o projétil ao acertar o inimigo
            Destroy(gameObject);
        }
    }
}
