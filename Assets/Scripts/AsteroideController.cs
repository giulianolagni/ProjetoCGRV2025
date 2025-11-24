using UnityEngine;

public class AsteroideController : MonoBehaviour
{
    [Header("Resistência")]
    [Tooltip("Vida do asteroide. Se o tiro tira 20, coloque 60.")]
    public float vidaMaxima = 60f;
    private float vidaAtual;

    [Header("Dano na Nave")]
    public float danoAoColidir = 20f;

    [Header("Feedback Visual")]
    [Tooltip("Arraste o Prefab da partícula de explosão aqui")]
    public GameObject prefabExplosao; 
    
    [Tooltip("Opcional: Arraste o texto de dano aqui")]
    public GameObject prefabTextoDano; 

    void Start()
    {
        vidaAtual = vidaMaxima;
        
        // Aviso de segurança no console 
        if(prefabExplosao == null) 
            Debug.LogError($"ERRO CRÍTICO: O Asteroide '{gameObject.name}' está sem a EXPLOSÃO no Inspector!");
    }

    public void TomarDano(float dano)
    {
        vidaAtual -= dano;


        if (vidaAtual <= 0)
        {
            Explodir();
        }
    }

    void Explodir()
    {
        if (prefabExplosao != null)
        {
            GameObject explosao = Instantiate(prefabExplosao, transform.position, transform.rotation);
            
            // --- TESTE DE ESCALA GIGANTE ---
            // Força a explosão a ser 50 vezes o tamanho original dela.
            // Se isso não aparecer na tela, o problema é no Asset da partícula.
            explosao.transform.localScale = Vector3.one * 50f; 
            
            Destroy(explosao, 3f);
        }
        else
        {
            Debug.Log("Tentei explodir, mas não tinha prefab de explosão!");
        }

        // Destrói a pedra
        Destroy(gameObject);
    }

    // Detecção do Tiro (Trigger)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TiroDoPlayer"))
        {
            Destroy(other.gameObject); // Some com o tiro
            TomarDano(20f); // Aplica dano fixo de 20
        }
    }

    // Detecção da Nave (Colisão Física)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ArcadeNave_VFinal nave = collision.gameObject.GetComponent<ArcadeNave_VFinal>();
            if (nave != null)
            {
                nave.TomarDano(danoAoColidir);
            }
        }
    }
}