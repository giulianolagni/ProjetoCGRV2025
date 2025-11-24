using UnityEngine;
using System.Collections.Generic;
using TMPro; 
using UnityEngine.SceneManagement;

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
    public GameObject hudPanel;
    public TextMeshProUGUI textoTempoFinal; 
    public string nomeCenaMenu = "Menu";

    private List<GameObject> aneisCriados = new List<GameObject>();
    private int indexAtual = 0;
    private bool rotaAtiva = false;
    private float tempoInicial;
    private ArcadeNave_VFinal scriptNave; 

    void Start()
    {
        tempoInicial = Time.time;
    }

    void Update()
    {
        // CHEAT: Se apertar a tecla "P", ganha o jogo na hora
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("CHEAT ATIVADO: Vitória Instantânea!");
            Vitoria(); // Chama a mesma função de quando passa pelo último anel
        }
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


[ContextMenu("Forcar Vitoria")]
    public void Vitoria()
    {
        if (scriptNave != null) scriptNave.DesativarSistemas();
        
        float tempoTotalPartida = Time.time - tempoInicial;

        // --- LÓGICA DE NOVO RECORDE ---
        // 1. Antes de salvar, vemos qual era o melhor tempo antigo (Posição 0)
        float melhorTempoAntigo = PlayerPrefs.GetFloat("RankTempo_0", 5999f);
        bool ehNovoRecorde = tempoTotalPartida < melhorTempoAntigo;

        // 2. Agora salvamos o seu tempo na lista (se entrar no top 10)
        SalvarNoTop10(tempoTotalPartida, MenuController.nomeDoJogadorAtual);

        // 3. Montamos o texto
        float minutos = Mathf.FloorToInt(tempoTotalPartida / 60);
        float segundos = Mathf.FloorToInt(tempoTotalPartida % 60);

        if (textoTempoFinal != null) 
        {
            textoTempoFinal.text = string.Format("PILOTO: {0}\nTEMPO: {1:00}:{2:00}", MenuController.nomeDoJogadorAtual, minutos, segundos);

            // 4. Se você bateu o Top 1, mostramos a mensagem amarela!
            if (ehNovoRecorde)
            {
                textoTempoFinal.text += "\n<color=yellow>NOVO RECORDE!</color>";
            }
        }

        if (telaVitoria != null) telaVitoria.SetActive(true);
        if (hudPanel != null) hudPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    // --- FUNÇÃO AUXILIAR NOVA PARA O TOP 10 ---
    void SalvarNoTop10(float tempoNovo, string nomeNovo)
    {
        // 1. Carrega a lista atual da memória para listas temporárias
        List<float> tempos = new List<float>();
        List<string> nomes = new List<string>();

        for (int i = 0; i < 10; i++)
        {
            // Se não tiver tempo salvo nessa posição, usa 5999 (infinito)
            tempos.Add(PlayerPrefs.GetFloat("RankTempo_" + i, 5999f));
            nomes.Add(PlayerPrefs.GetString("RankNome_" + i, "---"));
        }

        // 2. Adiciona o novo resultado na lista
        tempos.Add(tempoNovo);
        nomes.Add(nomeNovo);

        // 3. Ordena a lista (Bubble Sort simples, pois são poucos itens)
        // Precisamos ordenar os TEMPOS, mas mover os NOMES junto
        for (int i = 0; i < tempos.Count; i++)
        {
            for (int j = i + 1; j < tempos.Count; j++)
            {
                if (tempos[j] < tempos[i]) // Se o tempo J for menor (melhor)
                {
                    // Troca os tempos de lugar
                    float tempT = tempos[i];
                    tempos[i] = tempos[j];
                    tempos[j] = tempT;

                    // Troca os nomes de lugar junto
                    string tempN = nomes[i];
                    nomes[i] = nomes[j];
                    nomes[j] = tempN;
                }
            }
        }

        // 4. Salva apenas os 10 primeiros de volta na memória
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetFloat("RankTempo_" + i, tempos[i]);
            PlayerPrefs.SetString("RankNome_" + i, nomes[i]);
        }
        
        PlayerPrefs.Save();
    }
    public void SairDoJogo()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }


    public void VoltarAoMenu()
    {
        // 1. Encontra a Nave e desliga ela na hora
        // Isso impede que o OnGUI (HUD de vida/velocidade) seja desenhado nesse último frame
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) 
        {
            player.SetActive(false);
        }

        // 2. Garante que o HUD do Canvas (Minimap) também fique apagado
        if (hudPanel != null) hudPanel.SetActive(false);

        // 3. Agora sim, descongelamos o tempo e trocamos de cena
        Time.timeScale = 1f; 
        SceneManager.LoadScene(nomeCenaMenu); 
    }
}