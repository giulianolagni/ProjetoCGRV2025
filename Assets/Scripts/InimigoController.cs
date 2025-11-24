using UnityEngine;

public class InimigoController : MonoBehaviour
{
    [Header("Atributos de Combate")]
    public float vidaMaxima = 500f;
    private float vidaAtual;

    [Header("Feedback Visual")]
    public GameObject prefabTextoDano; 
    public Transform pontoDeTexto;
    public GameObject prefabExplosao; 

    [Header("Loot / Recompensa")]
    public GameObject prefabLoot; 
    [Range(0, 100)] public int chanceDeDrop = 100; 

    [Header("Atributos de Movimento")]
    public float forcaDeMovimento = 700f; 
    public float velocidadeMaxima = 100f; 
    public float velocidadeDeGiro = 2.0f;
    public float distanciaDeAtaque = 20.0f;

    private Vector3 offsetDeAtaque;
    private Transform jogador;
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private Color corOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 1f; 
        rb.angularDamping = 4f;

        vidaAtual = vidaMaxima;

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if(meshRenderer != null) corOriginal = meshRenderer.material.color;

        GameObject objetoJogador = GameObject.FindGameObjectWithTag("Player");
        if (objetoJogador != null) jogador = objetoJogador.transform;

        offsetDeAtaque = Random.onUnitSphere * distanciaDeAtaque;
        offsetDeAtaque.y = 0; 
    }

    void FixedUpdate()
    {
        if (jogador == null) return;

        Vector3 direcaoParaJogador = (jogador.position - transform.position).normalized;
        if (direcaoParaJogador != Vector3.zero) 
        {
            Quaternion rotacaoParaJogador = Quaternion.LookRotation(direcaoParaJogador);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, rotacaoParaJogador, velocidadeDeGiro * Time.fixedDeltaTime));
        }

        Vector3 posicaoAlvo = jogador.position + offsetDeAtaque;
        if (Vector3.Distance(transform.position, posicaoAlvo) > 2f) 
        {
            if (rb.linearVelocity.magnitude < velocidadeMaxima)
            {
                rb.AddForce((posicaoAlvo - transform.position).normalized * forcaDeMovimento * Time.fixedDeltaTime);
            }
        }
    }

    public void TomarDano(float dano)
    {
        vidaAtual -= dano;

        if(meshRenderer != null) 
        {
            meshRenderer.material.color = Color.red;
            Invoke("ResetarCor", 0.1f);
        }

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void ResetarCor()
    {
        if(meshRenderer != null) meshRenderer.material.color = corOriginal;
    }

    void Morrer()
    {
        if (prefabExplosao != null)
        {
            GameObject explosao = Instantiate(prefabExplosao, transform.position, transform.rotation);
            Destroy(explosao, 3f);
        }

        if (prefabLoot != null)
        {
            int sorteio = Random.Range(0, 101);
            if (sorteio <= chanceDeDrop)
            {
                Instantiate(prefabLoot, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TiroDoPlayer"))
        {
            Destroy(other.gameObject);
            TomarDano(20f); 
        }
    }
}