using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;

    [Header("Pontos de tiro")]
    public Transform firePointLeft;
    public Transform firePointRight;

    public float bulletSpeed = 50f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePointLeft == null || firePointRight == null)
        {
            Debug.LogWarning("PlayerShooting: algum campo não está configurado!");
            return;
        }

        // Tiro ESQUERDA
        GameObject b1 = Instantiate(bulletPrefab, firePointLeft.position, firePointLeft.rotation);
        Rigidbody rb1 = b1.GetComponent<Rigidbody>();
        if (rb1 != null)
            rb1.linearVelocity = firePointLeft.forward * bulletSpeed;

        // Tiro DIREITA
        GameObject b2 = Instantiate(bulletPrefab, firePointRight.position, firePointRight.rotation);
        Rigidbody rb2 = b2.GetComponent<Rigidbody>();
        if (rb2 != null)
            rb2.linearVelocity = firePointRight.forward * bulletSpeed;
    }
}
