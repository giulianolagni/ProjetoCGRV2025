using UnityEngine;

public class TiroSimples : MonoBehaviour
{
    [Header("--- CONFIGURAÇÃO DO TIRO ---")]
    public float velocidadeBase = 200f;
    public float tempoDeVida = 2f;

    private float velocidadeFinal;

    void Awake()
    {
        velocidadeFinal = velocidadeBase;
    }

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidadeFinal * Time.deltaTime);
    }

    public void SomarVelocidade(float velocidadeExtra)
    {
        velocidadeFinal += velocidadeExtra;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inimigo"))
        {
            // Apenas destrói o tiro. O dano é processado no script do Inimigo/Asteroide.
            Destroy(gameObject);
        }
    }
}