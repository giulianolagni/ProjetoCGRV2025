using UnityEngine;

public class CameraArcade : MonoBehaviour
{
    [Header("--- ALVO ---")]
    public Transform alvo;

    [Header("--- POSIÇÃO (SINCRONIZADA) ---")]
    public Vector3 offset = new Vector3(0f, 3f, -12f);
    public float velocidadeSeguir = 40f;
    
    [Tooltip("Limite máximo que a câmara pode se afastar para trás além do offset.z durante acelerações.")]
    public float maxAfastamentoZ = 2f; // Novo parâmetro para limitar o afastamento

    [Header("--- ROTAÇÃO ---")]
    public float suavidadeRotacao = 30f;

    void FixedUpdate()
    {
        if (alvo == null) return;

        // --- 1. POSIÇÃO ---
        // Posição ideal onde a câmara quer estar
        Vector3 posicaoIdeal = alvo.TransformPoint(offset);

        // Interpolação para suavizar o movimento até a posição ideal
        Vector3 novaPosicao = Vector3.Lerp(transform.position, posicaoIdeal, Time.fixedDeltaTime * velocidadeSeguir);

        // --- LIMITAR O AFASTAMENTO NO EIXO Z LOCAL ---
        // Converter a nova posição calculada para o espaço local do alvo
        Vector3 novaPosicaoLocal = alvo.InverseTransformPoint(novaPosicao);

        // Garantir que a posição Z local não seja menor (mais para trás) do que o offset.z - maxAfastamentoZ
        // Mathf.Max escolhe o maior valor, impedindo que vá abaixo do limite definido.
        novaPosicaoLocal.z = Mathf.Max(novaPosicaoLocal.z, offset.z - maxAfastamentoZ);

        // Converter de volta para posição mundial e aplicar à câmara
        transform.position = alvo.TransformPoint(novaPosicaoLocal);

        // --- 2. ROTAÇÃO ---
        Vector3 pontoFoco = alvo.position + (alvo.forward * 100f);
        Quaternion novaRotacao = Quaternion.LookRotation(pontoFoco - transform.position, alvo.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, novaRotacao, Time.fixedDeltaTime * suavidadeRotacao);
    }
}