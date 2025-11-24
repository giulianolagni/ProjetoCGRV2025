using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EfeitoJatoMotor : MonoBehaviour
{
    [Header("--- CONFIGURAÇÃO ---")]
    public ArcadeNave_VFinal nave; 

    [Header("--- VALORES DO JATO ---")]
    public float velocidadeParaMaximo = 1300f; 

    [Header("TAMANHO (Grossura)")]
    [Tooltip("O tamanho inicial das partículas quando parado")]
    public float tamanhoMinimo = 0.1f;
    [Tooltip("O tamanho inicial das partículas em turbo")]
    public float tamanhoMaximo = 1.0f;

    [Header("VELOCIDADE (Comprimento do Jato)")]
    [Tooltip("A velocidade inicial das partículas quando parado")]
    public float velocidadeParticulaMin = 5f;
    [Tooltip("A velocidade inicial das partículas em turbo")]
    public float velocidadeParticulaMax = 50f;

    private ParticleSystem ps;
    private ParticleSystem.MainModule mainModule;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main;
        
        if (nave == null)
        {
            nave = GetComponentInParent<ArcadeNave_VFinal>();
        }
    }

    void Update()
    {
        if (nave != null && ps != null)
        {
            float velocidadeAtual = nave.GetComponent<Rigidbody>().linearVelocity.magnitude;
            float ratio = Mathf.Clamp01(velocidadeAtual / velocidadeParaMaximo);

            float novoTamanho = Mathf.Lerp(tamanhoMinimo, tamanhoMaximo, ratio);
            mainModule.startSize = novoTamanho;

            float novaVelocidade = Mathf.Lerp(velocidadeParticulaMin, velocidadeParticulaMax, ratio);
            mainModule.startSpeed = novaVelocidade;
        }
    }
}