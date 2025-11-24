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

    [Header("Cenas e Painéis")]
    public string nomeDaCenaDoJogo = "JogoPrincipal"; 
    public GameObject painelRanking; 

    private int indiceDificuldade = 1; 
    private string[] nomesDificuldade = { "FÁCIL", "NORMAL", "DIFÍCIL" };

    void Start()
    {
        // REMOVI O PlayerPrefs.DeleteAll() DAQUI POIS ELE RESETAVA TUDO SEMPRE
        
        if (rawImage != null)
        {
            rawImage.SetActive(false);
            animatorRawImage = rawImage.GetComponent<Animator>();
        }
        if (menuOpcoes != null) menuOpcoes.SetActive(false);
        if (painelRanking != null) painelRanking.SetActive(false);

        if (rawImage != null) { rawImage.SetActive(true); if(animatorRawImage != null) animatorRawImage.SetTrigger("fadeIn"); }
        if (videoPlayer != null) videoPlayer.Play();
        if (menuOpcoes != null) menuOpcoes.SetActive(true);

        // Carrega a dificuldade salva
        indiceDificuldade = PlayerPrefs.GetInt("DificuldadeJogo", 1);
        AtualizarTextoDificuldade();
    }

    public void Jogar()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    public void MudarDificuldade()
    {
        indiceDificuldade++;
        if (indiceDificuldade > 2) indiceDificuldade = 0;

        PlayerPrefs.SetInt("DificuldadeJogo", indiceDificuldade);
        PlayerPrefs.Save(); 

        AtualizarTextoDificuldade();
    }

    public void Ranking()
    {
        if (painelRanking != null) painelRanking.SetActive(true);
    }

    public void Sair()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void AtualizarTextoDificuldade()
    {
        if (textoBotaoDificuldade != null)
        {
            textoBotaoDificuldade.text = nomesDificuldade[indiceDificuldade];
        }
    }
}