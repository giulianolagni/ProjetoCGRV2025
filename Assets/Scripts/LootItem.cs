using UnityEngine;

public class LootItem : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidadeGiro = 100f;
    public float flutuacao = 0.5f;
    public float velocidadeFlutuacao = 2f;

    private Vector3 posInicial;
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
        if (jaColetado) return;

        if (other.CompareTag("Player"))
        {
            jaColetado = true;

            ArcadeNave_VFinal nave = other.GetComponent<ArcadeNave_VFinal>();
            
            if (nave != null)
            {
                nave.ColetarFragmento();
                
                GetComponent<Renderer>().enabled = false; 
                GetComponent<Collider>().enabled = false;

                Destroy(gameObject, 0.1f);
            }
        }
    }
}