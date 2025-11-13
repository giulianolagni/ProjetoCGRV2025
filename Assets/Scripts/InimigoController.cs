// C# (Unity)
using UnityEngine;

public class InimigoController : MonoBehaviour
{
    [Header("Atributos de Combate")]
    [Tooltip("Como estamos usando 'AddForce', este valor precisa ser BEM MAIS ALTO que antes. Tente 500 ou 1000.")]
    public float forcaDeMovimento = 700f; 
    
    [Tooltip("Qual a velocidade máxima que a nave deve atingir.")]
    public float velocidadeMaxima = 100f; 

    public float velocidadeDeGiro = 2.0f;
    public float distanciaDeAtaque = 20.0f;

    // ---- Variáveis Privadas ----
    private Vector3 offsetDeAtaque;
    private Transform jogador;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 1f;
        rb.angularDamping = 4f;

        // Acha o Jogador
        GameObject objetoJogador = GameObject.FindGameObjectWithTag("Player");
        if (objetoJogador != null)
        {
            jogador = objetoJogador.transform;
        }

        // Sorteia o "Slot" de Ataque (mesma lógica de antes)
        offsetDeAtaque = Random.onUnitSphere * distanciaDeAtaque;
        if (offsetDeAtaque.y < 5f)
        {
            offsetDeAtaque.y = Mathf.Abs(offsetDeAtaque.y) + 5f;
        }
    }

    void FixedUpdate()
    {
        if (jogador == null) return;

        // --- LÓGICA DE ROTAÇÃO (OLHAR PARA O JOGADOR) ---
        Vector3 direcaoParaJogador = (jogador.position - transform.position).normalized;
        Quaternion rotacaoParaJogador = Quaternion.LookRotation(direcaoParaJogador);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotacaoParaJogador, velocidadeDeGiro * Time.fixedDeltaTime));

        // --- LÓGICA DE MOVIMENTO (COM ADDFORCE) ---
        Vector3 posicaoAlvo = jogador.position + offsetDeAtaque;
        float distanciaDoAlvo = Vector3.Distance(transform.position, posicaoAlvo);

        if (distanciaDoAlvo > 2f) 
        {
            Vector3 direcaoParaAlvo = (posicaoAlvo - transform.position).normalized;

            if (rb.linearVelocity.magnitude < velocidadeMaxima)
            {
                rb.AddForce(direcaoParaAlvo * forcaDeMovimento * Time.fixedDeltaTime);
            }
        }
        // Se estiver perto, o linearDamping (freio) faz o trabalho
    }
    

    // --- MUDANÇA PRINCIPAL AQUI ---
    void OnTriggerEnter(Collider other)
    {
        // Se bateu no jogador
        if (other.CompareTag("Player"))
        {
            // Destrói SÓ O JOGADOR.
            Destroy(other.gameObject); 
            // A nave inimiga NÃO se destrói. Ela é "kamikaze".
        }

        // Se levou um tiro (ÚNICA FORMA DE MORRER)
        if (other.CompareTag("TiroDoPlayer"))
        {
            // Destrói o tiro
            Destroy(other.gameObject); 
            // Destrói a si mesmo (a nave inimiga)
            Destroy(gameObject);       
        }
    }

    // Esta função agora está VAZIA.
    // Nada mais pode destruir esta nave (como sair da tela)
    void Update() 
    { 
        // Em branco
    }
}