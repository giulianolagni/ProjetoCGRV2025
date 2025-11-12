using UnityEngine;

public class TiroSimples : MonoBehaviour
{
    [Header("--- CONFIGURAÇÃO DO TIRO ---")]
    public float velocidadeBase = 200f; // Velocidade padrão do tiro
    public float tempoDeVida = 2f;

    private float velocidadeFinal;

    void Awake()
    {
        // Inicialmente, a velocidade final é apenas a base.
        // Usamos Awake para garantir que isto acontece antes de qualquer outra coisa.
        velocidadeFinal = velocidadeBase;
    }

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // Usa a velocidadeFinal que pode ter sido alterada pela nave
        transform.Translate(Vector3.forward * velocidadeFinal * Time.deltaTime);
    }

    // --- NOVO MÉTODO ---
    // Este método será chamado pela nave para "injetar" a velocidade dela no tiro
    public void SomarVelocidade(float velocidadeExtra)
    {
        velocidadeFinal += velocidadeExtra;
    }

<<<<<<< HEAD
    void OnTriggerEnter(Collider other)
    {
         // Adiciona aqui tags para ignorar se necessário (ex: não se destruir na própria nave)
         // if (other.CompareTag("Player")) return;
         
         Destroy(gameObject);
=======
void OnTriggerEnter(Collider other)
    {
        // Se acertar num objeto com a tag "Inimigo"
        if (other.CompareTag("Inimigo"))
        {
            // Destrói o inimigo (o asteroide)
            Destroy(other.gameObject);
            
            // Destrói o tiro (este objeto)
            Destroy(gameObject);
        }
        
        // Se acertar noutra coisa (ex: outro tiro, a própria nave), 
        // podes adicionar lógica aqui. Por agora, apenas o tiro se destrói.
        
        // NOTA: Se o asteroide destruir o tiro, mas o tiro não destruir o asteroide,
        // verifica se aplicaste a tag "Inimigo" corretamente no Passo 1!
>>>>>>> master
    }
}