using UnityEngine;

public class AsteroideController : MonoBehaviour
{
    [Header("Resistência")]
    public float vidaMaxima = 60f;
    private float vidaAtual;

    [Header("Dano na Nave")]
    public float danoAoColidir = 20f;

    [Header("Feedback Visual")]
    public GameObject prefabExplosao; 
    public GameObject prefabTextoDano; 

    void Start()
    {
        vidaAtual = vidaMaxima;
        
        if(prefabExplosao == null) 
            Debug.LogError($"O Asteroide '{gameObject.name}' está sem a EXPLOSÃO no Inspector!");
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
            
            // Aumenta escala da explosão
            explosao.transform.localScale = Vector3.one * 50f; 
            
            Destroy(explosao, 3f);
        }
        else
        {
            Debug.Log("Sem prefab de explosão!");
        }

        Destroy(gameObject);
    }

    // Colisão com o tiro
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TiroDoPlayer"))
        {
            Destroy(other.gameObject); 
            TomarDano(20f); 
        }
    }

    // Colisão com a nave
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