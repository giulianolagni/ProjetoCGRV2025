using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Componentes Visuais")]
    public VideoPlayer videoPlayer;
    public GameObject menuOpcoes; // O pai dos botões (Menuopcoes)
    public GameObject rawImage;   // A tela do vídeo
    private Animator animatorRawImage;

    [Header("Configuração do Jogo")]
    public string nomeDaCenaDoJogo = "GameScene"; 
    
    // Painéis extras (opcional)
    public GameObject painelDificuldade;
    public GameObject painelRanking;

    void Start()
    {
        // 1. Pega o componente de animação
        if (rawImage != null)
            animatorRawImage = rawImage.GetComponent<Animator>();

        // 2. INICIA TUDO AUTOMATICAMENTE
        // Liga a tela do vídeo
        if (rawImage != null)
        {
            rawImage.SetActive(true);
            if (animatorRawImage != null) animatorRawImage.SetTrigger("fadeIn");
        }

        // Dá play no vídeo
        if (videoPlayer != null)
            videoPlayer.Play();

        // Mostra os botões do menu imediatamente
        if (menuOpcoes != null)
            menuOpcoes.SetActive(true);
        
        // Garante que os painéis extras comecem fechados
        if (painelDificuldade != null) painelDificuldade.SetActive(false);
        if (painelRanking != null) painelRanking.SetActive(false);
    }

    // O Update agora ficou vazio porque não precisamos mais verificar teclas a todo momento
    void Update()
    {
    }

    // --- FUNÇÕES DOS BOTÕES ---

    public void Jogar()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    public void Dificuldade()
    {
        Debug.Log("Clicou em Dificuldade");
        if (painelDificuldade != null) painelDificuldade.SetActive(true);
    }

    public void Ranking()
    {
        Debug.Log("Clicou em Ranking");
        if (painelRanking != null) painelRanking.SetActive(true);
    }

    public void Sair()
    {
        Debug.Log("Saindo do Jogo..."); // Mostra mensagem no console
        Application.Quit(); // Fecha o jogo compilado (.exe)

        // Esse trecho faz o botão parar o Play dentro do Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}