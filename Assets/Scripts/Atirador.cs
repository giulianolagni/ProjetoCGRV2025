using UnityEngine;

public class Atirador : MonoBehaviour
{
    [Header("CONFIGURAÇÃO DE DISPARO")]
    [SerializeField] private GameObject prefabProjetil; 
    [SerializeField] private Transform[] pontosDeDisparo;
    [SerializeField] private float forcaDisparo = 1000f; 
    [SerializeField] private float fatorDeAntecipacao = 1.0f; 
    [SerializeField] private float intervaloTiro = 2f; 
    
    private float proximoTiro;
    private Transform jogador;
    private Rigidbody rbJogador;

    void Start()
    {
        proximoTiro = Time.time + intervaloTiro;

        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            jogador = objJogador.transform;
            rbJogador = objJogador.GetComponent<Rigidbody>();
            Debug.Log("SCRIPT ATIRADOR: Player encontrado! Agora vai funcionar.");
        }
        else
        {
            Debug.LogError("SCRIPT ATIRADOR: NÃO ACHEI O PLAYER! Verifique a Tag 'Player'.");
        }
    }

    void Update()
    {
        if (jogador == null) return;

        if (Time.time > proximoTiro)
        {
            Atirar();
            proximoTiro = Time.time + intervaloTiro;
        }
    }

    void Atirar()
    {
        // Verificações de segurança
        if (prefabProjetil == null)
        {
            Debug.LogError("O Prefab do tiro sumiu!");
            return;
        }

        Vector3 posicaoAlvo = jogador.position;
        if (rbJogador != null)
        {
            float distancia = Vector3.Distance(transform.position, jogador.position);
            // Ajuste fino da velocidade se necessário
            float tempoViagem = distancia / (forcaDisparo * 0.02f); 
            
            // SE DER ERRO VERMELHO AQUI, TROQUE 'linearVelocity' POR 'velocity'
            Vector3 velocidadeDoPlayer = rbJogador.linearVelocity; 
            
            posicaoAlvo = jogador.position + (velocidadeDoPlayer * tempoViagem * fatorDeAntecipacao);
        }

        foreach (Transform ponto in pontosDeDisparo)
        {
            if(ponto != null)
            {
                GameObject novoTiro = Instantiate(prefabProjetil, ponto.position, ponto.rotation);
                Rigidbody rbTiro = novoTiro.GetComponent<Rigidbody>();

                if (rbTiro != null)
                {
                    Vector3 direcaoCalculada = (posicaoAlvo - ponto.position).normalized;
                    rbTiro.AddForce(direcaoCalculada * forcaDisparo);
                }
            }
        }
    }
}