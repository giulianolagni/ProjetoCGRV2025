using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro; 

public class MenuController : MonoBehaviour
{
    [Header("Componentes da Intro")]
    public VideoPlayer videoPlayer;
    public GameObject menuOpcoes;
    public GameObject rawImage;
    private Animator animatorRawImage;

    [Header("Configuração dos Botões")]
    public TextMeshProUGUI textoBotaoDificuldade;
    public TMP_InputField campoNomeJogador; // <<< ARRASTE O INPUT FIELD AQUI

    [Header("Ranking")]
    public TextMeshProUGUI textoBestTime; 
    public GameObject painelRanking;

    [Header("Cenas")]
    public string nomeDaCenaDoJogo = "JogoPrincipal"; 

    // VARIÁVEL GLOBAL PARA GUARDAR O NOME (Para o outro script ler)
    public static string nomeDoJogadorAtual = "Piloto";

    private int indiceDificuldade = 1; 
    private string[] nomesDificuldade = { "FÁCIL", "NORMAL", "DIFÍCIL" };

    void Start()
    {
        // Intro
        if (rawImage != null) { rawImage.SetActive(false); animatorRawImage = rawImage.GetComponent<Animator>(); }
        if (menuOpcoes != null) menuOpcoes.SetActive(false);
        if (painelRanking != null) painelRanking.SetActive(false);

        // Iniciar
        if (rawImage != null) { rawImage.SetActive(true); if(animatorRawImage != null) animatorRawImage.SetTrigger("fadeIn"); }
        if (videoPlayer != null) videoPlayer.Play();
        if (menuOpcoes != null) menuOpcoes.SetActive(true);

        // Carregar dados
        indiceDificuldade = PlayerPrefs.GetInt("DificuldadeJogo", 1);
        AtualizarTextoDificuldade();
        AtualizarTextoRanking(); // Mostra o ranking atualizado
    }

    public void Jogar()
    {
        // 1. SALVA O NOME ANTES DE ENTRAR NO JOGO
        if (campoNomeJogador != null && campoNomeJogador.text.Length > 0)
        {
            nomeDoJogadorAtual = campoNomeJogador.text;
        }
        else
        {
            nomeDoJogadorAtual = "Piloto Anônimo";
        }

        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

public void Sair()
    {
        // Limpa as 10 posições do Ranking
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey("RankTempo_" + i);
            PlayerPrefs.DeleteKey("RankNome_" + i);
        }
        
        // Limpa as chaves antigas se tiverem sobrado
        PlayerPrefs.DeleteKey("MelhorTempoRanking");
        PlayerPrefs.DeleteKey("MelhorNomeRanking");

        Debug.Log("Ranking Resetado!");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // --- RANKING ---
    public void AbrirRanking() { if (painelRanking != null) painelRanking.SetActive(true); }
    public void FecharRanking() { if (painelRanking != null) painelRanking.SetActive(false); }

void AtualizarTextoRanking()
    {
        if (textoBestTime == null) return;

        textoBestTime.text = "TOP 10 PILOTOS:\n\n";

        // Loop para ler as 10 posições
        for (int i = 0; i < 10; i++)
        {
            float tempo = PlayerPrefs.GetFloat("RankTempo_" + i, 5999f);
            string nome = PlayerPrefs.GetString("RankNome_" + i, "---");

            if (tempo >= 5999f)
            {
                // Se for tempo vazio, mostra traços
                textoBestTime.text += string.Format("{0}. {1}   --:--\n", i + 1, "---");
            }
            else
            {
                float min = Mathf.FloorToInt(tempo / 60);
                float seg = Mathf.FloorToInt(tempo % 60);
                // Exemplo: 1. Capitão - 01:20
                textoBestTime.text += string.Format("{0}. {1}   {2:00}:{3:00}\n", i + 1, nome, min, seg);
            }
        }
    }

    // --- DIFICULDADE (Mantido Igual) ---
    public void MudarDificuldade()
    {
        indiceDificuldade++;
        if (indiceDificuldade > 2) indiceDificuldade = 0;
        PlayerPrefs.SetInt("DificuldadeJogo", indiceDificuldade);
        PlayerPrefs.Save(); 
        AtualizarTextoDificuldade();
    }

    void AtualizarTextoDificuldade()
    {
        if (textoBotaoDificuldade != null) textoBotaoDificuldade.text = nomesDificuldade[indiceDificuldade];
    }
}