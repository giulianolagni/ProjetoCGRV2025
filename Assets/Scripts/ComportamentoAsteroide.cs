using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ComportamentoAsteroide : MonoBehaviour
{
    [Header("--- CONFIGURAÇÃO ---")]
    public float velocidadeRotacaoMax = 40f;
    public float velocidadeMovimentoMax = 30f;
    public float tempoDeVida = 60f; // Para limpar asteroides perdidos

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 1. Define uma rotação (torque) aleatória
        Vector3 torqueAleatorio = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        rb.AddTorque(torqueAleatorio.normalized * velocidadeRotacaoMax, ForceMode.Impulse);

        // 2. Define uma direção de movimento (força) aleatória
        Vector3 direcaoAleatoria = Random.onUnitSphere; // Uma direção 3D aleatória
        rb.AddForce(direcaoAleatoria * velocidadeMovimentoMax, ForceMode.Impulse);

        // 3. Autodestruição para não encher a cena
        Destroy(gameObject, tempoDeVida);
    }
}