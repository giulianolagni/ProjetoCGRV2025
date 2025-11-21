using UnityEngine;

public class LootItem : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidadeGiro = 100f;
    public float flutuacao = 0.5f;
    public float velocidadeFlutuacao = 2f;

    private Vector3 posInicial;
    
    // --- TRAVA DE SEGURANÇA ---
    private bool jaColetado = false; 

    void Start()
    {
        posInicial = transform.position;
        Destroy(gameObject, 60f);
    }

    void Update()
    {
        transform.Rotate(Vector3.up * velocidadeGiro * Time.deltaTime);
        float novoY = posInicial.y + Mathf.Sin(Time.time * velocidadeFlutuacao) * flutuacao;
        transform.position = new Vector3(transform.position.x, novoY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        // 1. A PERGUNTA DE OURO: Já fui pego?
        if (jaColetado) return; // Se sim, para tudo e não faz nada.

        if (other.CompareTag("Player"))
        {
            // 2. TRAVA IMEDIATAMENTE
            jaColetado = true;

            ArcadeNave_VFinal nave = other.GetComponent<ArcadeNave_VFinal>();
            
            if (nave != null)
            {
                nave.ColetarFragmento();
                
                // Desativa o visual na hora pra sumir instantaneamente (opcional, mas fica melhor)
                GetComponent<Renderer>().enabled = false; 
                GetComponent<Collider>().enabled = false;

                Destroy(gameObject, 0.1f); // Dá um tempinho pro som tocar se tiver
            }
        }
    }
}