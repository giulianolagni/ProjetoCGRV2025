using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EfeitoJatoMotor : MonoBehaviour
{
    [Header("--- CONFIGURAÇÃO ---")]
    public ArcadeNave_VFinal nave; 

    [Header("--- VALORES DO JATO ---")]
    // Velocidade turbo da tua nave
    public float velocidadeParaMaximo = 1300f; 

    [Header("TAMANHO (Grossura)")]
    [Tooltip("O tamanho inicial das partículas quando parado (ex: 0.1)")]
    public float tamanhoMinimo = 0.1f;
    [Tooltip("O tamanho inicial das partículas em turbo (ex: 1.0)")]
    public float tamanhoMaximo = 1.0f;

    [Header("VELOCIDADE (Comprimento do Jato)")]
    [Tooltip("A velocidade inicial das partículas quando parado (ex: 5)")]
    public float velocidadeParticulaMin = 5f;
    [Tooltip("A velocidade inicial das partículas em turbo (ex: 50)")]
    public float velocidadeParticulaMax = 50f;


    private ParticleSystem ps;
    private ParticleSystem.MainModule mainModule; // Módulo principal para edição

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        mainModule = ps.main; // Guardamos o módulo principal para o poder editar
        
        if (nave == null)
        {
            nave = GetComponentInParent<ArcadeNave_VFinal>();
        }
    }

    void Update()
    {
        if (nave != null && ps != null)
        {
            // Vamos buscar a velocidade atual diretamente do Rigidbody da nave
            float velocidadeAtual = nave.GetComponent<Rigidbody>().linearVelocity.magnitude;

            // Calcula uma percentagem de 0 a 1 com base na velocidade atual
            float ratio = Mathf.Clamp01(velocidadeAtual / velocidadeParaMaximo);

            // --- AQUI ESTÁ A MUDANÇA ---

            // 1. Interpola o TAMANHO (Grossura) das partículas
            float novoTamanho = Mathf.Lerp(tamanhoMinimo, tamanhoMaximo, ratio);
            mainModule.startSize = novoTamanho;

            // 2. Interpola a VELOCIDADE (Comprimento) do jato
            float novaVelocidade = Mathf.Lerp(velocidadeParticulaMin, velocidadeParticulaMax, ratio);
            mainModule.startSpeed = novaVelocidade;
        }
    }
}