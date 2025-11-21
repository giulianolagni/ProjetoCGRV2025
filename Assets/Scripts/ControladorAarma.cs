using UnityEngine;

public class ControladorArma : MonoBehaviour
{
    [Header("--- CONFIGURAÇÃO DA ARMA ---")]
    public GameObject prefabTiro;
    public Transform[] pontosDeSaida;
    public KeyCode teclaDisparo = KeyCode.Space;

    [Header("--- CADÊNCIA ---")]
    public float intervaloEntreTiros = 0.2f;
    private float proximoTempoDisparo = 0f;

    // Referência para a física da nave para saber a velocidade atual
    private Rigidbody rbNave;

    void Start()
    {
        // Tenta encontrar o Rigidbody na própria nave automaticamente
        rbNave = GetComponent<Rigidbody>();
        if (rbNave == null)
        {
            Debug.LogWarning("ControladorArma: Não encontrei um Rigidbody na nave! Os tiros não vão herdar a velocidade.");
        }
    }

    void Update()
    {
        if (Input.GetKey(teclaDisparo) && Time.time >= proximoTempoDisparo)
        {
            Atirar();
        }
    }

    void Atirar()
    {
        proximoTempoDisparo = Time.time + intervaloEntreTiros;

        if (prefabTiro != null && pontosDeSaida != null && pontosDeSaida.Length > 0)
        {
            // 1. Calcula a velocidade atual da nave para a frente.
            // Se não tivermos Rigidbody, assumimos que a velocidade extra é 0.
            float velocidadeDaNave = 0f;
            if (rbNave != null)
            {
                // Usamos Vector3.Dot para pegar apenas a velocidade na direção que a nave está a apontar (frente)
                velocidadeDaNave = Vector3.Dot(rbNave.linearVelocity, transform.forward);
                
                // Se preferires que herde a velocidade total (mesmo de lado), usa:
                // velocidadeDaNave = rbNave.velocity.magnitude;
            }

            // 2. Cria os tiros e passa a velocidade
            foreach (Transform ponto in pontosDeSaida)
            {
                if (ponto != null)
                {
                    GameObject novoTiro = Instantiate(prefabTiro, ponto.position, ponto.rotation);
                    
                    // Pega o script do tiro que acabámos de criar
                    TiroSimples scriptTiro = novoTiro.GetComponent<TiroSimples>();
                    
                    // Se o script existir, soma a velocidade da nave
                    if (scriptTiro != null)
                    {
                        // Passamos um pouco mais da velocidade da nave (ex: 10% a mais) para garantir que nunca fica para trás, 
                        // ou apenas a velocidade exata. Vamos tentar a exata primeiro.
                        scriptTiro.SomarVelocidade(Mathf.Max(0, velocidadeDaNave)); 
                        // Mathf.Max(0, ...) garante que não travamos o tiro se a nave estiver a andar para trás.
                    }
                }
            }
        }
    }
}