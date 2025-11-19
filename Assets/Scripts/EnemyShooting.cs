using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Configuração de Tiro")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float bulletSpeed = 40f;
    public float fireRate = 1.5f; // um tiro a cada 1.5 segundos
    private float fireCooldown = 0f;

    private Transform target; // a nave do jogador

    void Start()
    {
        // encontra o player usando tag
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (target == null) return;

        // faz o inimigo olhar para o player
        transform.LookAt(target.position);

        // controla o cooldown
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("EnemyShooting: bulletPrefab ou firePoint não configurados!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = firePoint.forward * bulletSpeed;
    }
}
