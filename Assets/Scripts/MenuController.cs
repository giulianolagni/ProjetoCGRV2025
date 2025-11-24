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
    public TMP_InputField campoNomeJogador; 

    [Header("Ranking")]
    public TextMeshProUGUI textoBestTime; 
    public GameObject painelRanking;

    [Header("Cenas")]
    public string nomeDaCenaDoJogo = "JogoPrincipal"; 

    public static string nomeDoJogadorAtual = "Piloto";

    private int indiceDificuldade = 1; 
    private string[] nomesDificuldade = { "FÁCIL", "NORMAL", "DIFÍCIL" };

    void Start()
    {
        if (rawImage != null) { rawImage.SetActive(false); animatorRawImage = rawImage.GetComponent<Animator>(); }
        if (menuOpcoes != null) menuOpcoes.SetActive(false);
        if (painelRanking != null) painelRanking.SetActive(false);

        if (rawImage != null) { rawImage.SetActive(true); if(animatorRawImage != null) animatorRawImage.SetTrigger("fadeIn"); }
        if (videoPlayer != null) videoPlayer.Play();
        if (menuOpcoes != null) menuOpcoes.SetActive(true);

        indiceDificuldade = PlayerPrefs.GetInt("DificuldadeJogo", 1);
        AtualizarTextoDificuldade();
        AtualizarTextoRanking(); 
    }

    public void Jogar()
    {
        // Verifica se o campo não é nulo, não está vazio e não é o texto padrão do bug
        if (campoNomeJogador != null && !string.IsNullOrWhiteSpace(campoNomeJogador.text) && campoNomeJogador.text != "Insira Seu Nome")
        {
            nomeDoJogadorAtual = campoNomeJogador.text;
        }
        else
        {
            // Lógica de incremento (Piloto 1, Piloto 2...)
            int contador = PlayerPrefs.GetInt("ContadorPilotos", 1);
            nomeDoJogadorAtual = "Piloto " + contador;
            
            PlayerPrefs.SetInt("ContadorPilotos", contador + 1);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    public void Sair()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey("RankTempo_" + i);
            PlayerPrefs.DeleteKey("RankNome_" + i);
        }
        
        PlayerPrefs.DeleteKey("MelhorTempoRanking");
        PlayerPrefs.DeleteKey("MelhorNomeRanking");
        
        // Reseta o contador de pilotos ao sair
        PlayerPrefs.DeleteKey("ContadorPilotos");

        Debug.Log("Ranking Resetado!");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void AbrirRanking() { if (painelRanking != null) painelRanking.SetActive(true); }
    public void FecharRanking() { if (painelRanking != null) painelRanking.SetActive(false); }

    void AtualizarTextoRanking()
    {
        if (textoBestTime == null) return;

        textoBestTime.text = "TOP 10 PILOTOS:\n\n";

        for (int i = 0; i < 10; i++)
        {
            float tempo = PlayerPrefs.GetFloat("RankTempo_" + i, 5999f);
            string nome = PlayerPrefs.GetString("RankNome_" + i, "---");

            if (tempo >= 5999f)
            {
                textoBestTime.text += string.Format("{0}. {1}   --:--\n", i + 1, "---");
            }
            else
            {
                float min = Mathf.FloorToInt(tempo / 60);
                float seg = Mathf.FloorToInt(tempo % 60);
                textoBestTime.text += string.Format("{0}. {1}   {2:00}:{3:00}\n", i + 1, nome, min, seg);
            }
        }
    }

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