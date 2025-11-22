using UnityEngine;
using System.Collections.Generic;
using TMPro; 

public class RotaManager : MonoBehaviour
{
    [Header("Gerador de Rota")]
    public GameObject prefabCheckpoint;
    public GameObject prefabFinal; 
    public int quantidadeDeAneis = 5;
    public float distanciaEntreAneis = 300f;

    [Header("Caos da Rota")]
    public float intensidadeDaCurva = 40f; 

    [Header("Configuração dos Asteroides")]
    public GameObject prefabAsteroide;
    
    [Tooltip("Em qual anel começam os asteroides? (0 = Desde o início)")]
    public int comecarNoAnel = 2; 
    
    [Tooltip("Em qual anel param os asteroides?")]
    public int terminarNoAnel = 4;

    [Tooltip("Quantos asteroides por trecho. SE AUMENTAR A LARGURA, AUMENTE ISSO TAMBÉM!")]
    public int densidadeDeAsteroides = 50; 
    
    [Tooltip("O quão espalhados eles ficam. Tente 100 ou 150.")]
    public float larguraDoCampo = 100f;

    [Header("UI de Vitória")]
    public GameObject telaVitoria;
    public TextMeshProUGUI textoTempoFinal; 

    private List<GameObject> aneisCriados = new List<GameObject>();
    private int indexAtual = 0;
    private bool rotaAtiva = false;
    private float tempoInicial;
    private ArcadeNave_VFinal scriptNave; 

    void Start()
    {
        tempoInicial = Time.time;
    }

    public void IniciarRotaDeFuga(Transform playerTransform)
    {
        if (prefabCheckpoint == null || rotaAtiva) return;

        rotaAtiva = true;
        indexAtual = 0;
        if(telaVitoria != null) telaVitoria.SetActive(false);

        scriptNave = playerTransform.GetComponent<ArcadeNave_VFinal>();

        Debug.Log("GERANDO ROTA COM OBSTÁCULOS ESPECÍFICOS...");

        Vector3 pontoAnterior = playerTransform.position;
        Vector3 direcaoAtual = playerTransform.forward;

        for (int i = 0; i < quantidadeDeAneis; i++)
        {
            // 1. Curvas
            float rotX = Random.Range(-intensidadeDaCurva, intensidadeDaCurva);
            float rotY = Random.Range(-intensidadeDaCurva, intensidadeDaCurva);
            Quaternion rotacaoAleatoria = Quaternion.Euler(rotX, rotY, 0);
            direcaoAtual = rotacaoAleatoria * direcaoAtual;

            // 2. Posição
            Vector3 posicaoSpawn = pontoAnterior + (direcaoAtual * distanciaEntreAneis);
            Quaternion rotacaoDoAnel = Quaternion.LookRotation(direcaoAtual);

            // 3. Prefab
            GameObject prefabParaUsar = (i == quantidadeDeAneis - 1 && prefabFinal != null) ? prefabFinal : prefabCheckpoint;

            // 4. Instancia
            GameObject novoAnel = Instantiate(prefabParaUsar, posicaoSpawn, rotacaoDoAnel);
            novoAnel.SetActive(false);
            aneisCriados.Add(novoAnel);

            // --- LÓGICA NOVA: CONTROLE DE ONDE APARECEM ---
            // Verifica se o índice atual 'i' está dentro do intervalo que você pediu
            if (i >= comecarNoAnel && i <= terminarNoAnel)
            {
                GerarAsteroidesNoCaminho(pontoAnterior, posicaoSpawn);
            }
            // -----------------------------------------------

            pontoAnterior = posicaoSpawn;
        }

        if (aneisCriados.Count > 0) aneisCriados[0].SetActive(true);
    }

    void GerarAsteroidesNoCaminho(Vector3 inicio, Vector3 fim)
    {
        if (prefabAsteroide == null || densidadeDeAsteroides <= 0) return;

        for (int k = 0; k < densidadeDeAsteroides; k++)
        {
            // Sorteia posição na linha
            float progresso = Random.Range(0.1f, 0.9f);
            Vector3 pontoNaLinha = Vector3.Lerp(inicio, fim, progresso);

            // Espalha usando a largura
            Vector3 posicaoFinal = pontoNaLinha + (Random.insideUnitSphere * larguraDoCampo);

            GameObject asteroide = Instantiate(prefabAsteroide, posicaoFinal, Random.rotation);

            // Tamanho aleatório
            float escala = Random.Range(5f, 25f); 
            asteroide.transform.localScale = Vector3.one * escala;
            
            Destroy(asteroide, 120f); 
        }
    }

    public void CheckpointAlcancado(GameObject checkpointAtingido)
    {
        if (indexAtual < aneisCriados.Count && checkpointAtingido == aneisCriados[indexAtual])
        {
            checkpointAtingido.SetActive(false);
            indexAtual++;

            if (indexAtual < aneisCriados.Count) aneisCriados[indexAtual].SetActive(true);
            else Vitoria();
        }
    }

    void Vitoria()
    {
        if (scriptNave != null) scriptNave.DesativarSistemas();
        
        float tempoTotal = Time.time - tempoInicial;
        float minutos = Mathf.FloorToInt(tempoTotal / 60);
        float segundos = Mathf.FloorToInt(tempoTotal % 60);

        if (textoTempoFinal != null) textoTempoFinal.text = string.Format("TEMPO TOTAL: {0:00}:{1:00}", minutos, segundos);

        if (telaVitoria != null) telaVitoria.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void SairDoJogo()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}