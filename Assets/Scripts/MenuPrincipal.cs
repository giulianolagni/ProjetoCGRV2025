using UnityEngine;
using TMPro; 

public class MenuPrincipal : MonoBehaviour
{
    // O Painel ou Canvas que segura os botões
    public GameObject painelDoMenu; 

    // O texto dentro do botão de dificuldade
    public TextMeshProUGUI textoDificuldade; 

    private int dificuldadeAtual = 0;
    private bool jogoPausado = true; 

    void Start()
    {
        PausarJogo(); // Começa pausado
        
        // Carrega a dificuldade salva
        dificuldadeAtual = PlayerPrefs.GetInt("DificuldadeSalva", 0);
        AtualizarTextoDificuldade();
    }

    void Update()
    {
        // Tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoPausado)
            {
                Jogar();
            }
            else
            {
                PausarJogo();
            }
        }
    }

    // --- FUNÇÕES ---

    public void Jogar()
    {
        painelDoMenu.SetActive(false); // Esconde menu
        Time.timeScale = 1f; // Descongela tempo
        jogoPausado = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PausarJogo()
    {
        painelDoMenu.SetActive(true); // Mostra menu
        Time.timeScale = 0f; // Congela tempo
        jogoPausado = true;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Sair()
    {
        Debug.Log("Saiu do Jogo!");
        Application.Quit();
    }

    public void MudarDificuldade()
    {
        dificuldadeAtual++;
        
        // AQUI ESTAVA O ERRO, AGORA ESTÁ CORRIGIDO:
        if (dificuldadeAtual > 2)
        {
            dificuldadeAtual = 0;
        }
        
        PlayerPrefs.SetInt("DificuldadeSalva", dificuldadeAtual);
        PlayerPrefs.Save();
        AtualizarTextoDificuldade();
    }

    void AtualizarTextoDificuldade()
    {
        if (textoDificuldade != null)
        {
            switch (dificuldadeAtual)
            {
                case 0: textoDificuldade.text = "Dificuldade: Fácil"; break;
                case 1: textoDificuldade.text = "Dificuldade: Médio"; break;
                case 2: textoDificuldade.text = "Dificuldade: Difícil"; break;
            }
        }
    }
}