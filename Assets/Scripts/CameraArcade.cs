using UnityEngine;

public class CameraArcade : MonoBehaviour
{
    [Header("--- ALVO ---")]
    public Transform alvo;

    [Header("--- POSIÇÃO (SINCRONIZADA) ---")]
    public Vector3 offset = new Vector3(0f, 3f, -12f);
    public float velocidadeSeguir = 40f;
    
    [Tooltip("Limite máximo que a câmara pode se afastar para trás além do offset.z")]
    public float maxAfastamentoZ = 2f; 

    [Header("--- ROTAÇÃO ---")]
    public float suavidadeRotacao = 30f;

    void FixedUpdate()
    {
        if (alvo == null) return;

        // Posição
        Vector3 posicaoIdeal = alvo.TransformPoint(offset);
        Vector3 novaPosicao = Vector3.Lerp(transform.position, posicaoIdeal, Time.fixedDeltaTime * velocidadeSeguir);

        // Limitar afastamento Z
        Vector3 novaPosicaoLocal = alvo.InverseTransformPoint(novaPosicao);
        novaPosicaoLocal.z = Mathf.Max(novaPosicaoLocal.z, offset.z - maxAfastamentoZ);

        transform.position = alvo.TransformPoint(novaPosicaoLocal);

        // Rotação
        Vector3 pontoFoco = alvo.position + (alvo.forward * 100f);
        Quaternion novaRotacao = Quaternion.LookRotation(pontoFoco - transform.position, alvo.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, novaRotacao, Time.fixedDeltaTime * suavidadeRotacao);
    }
}