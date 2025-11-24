using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("Interfaces")]
    public GameObject pauseMenuUI; // O painel preto do pause
    public GameObject hudUI;       // O painel do HUD (Vida, Speed, etc)

    [Header("Configurações")]
    public string nomeCenaMenu = "Menu"; 

    public static bool jogoEstaPausado = false;

    void Start()
    {
        // Garante que o jogo comece com o mouse preso e escondido (para pilotar a nave)
        TravarMouse();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (jogoEstaPausado)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Esconde o menu de pause
        if(hudUI != null) hudUI.SetActive(true);   // MOSTRA o HUD de volta
        
        Time.timeScale = 1f;
        jogoEstaPausado = false;

        // Esconde o mouse de novo para voltar a jogar
        TravarMouse();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Mostra o menu de pause
        if(hudUI != null) hudUI.SetActive(false);  // ESCONDE o HUD
        
        Time.timeScale = 0f;
        jogoEstaPausado = true;

        // Libera o mouse para clicar nos botões
        DestravarMouse();
    }

    public void CarregarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCenaMenu);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    // --- FUNÇÕES AUXILIARES DE MOUSE ---

    void TravarMouse()
    {
        Cursor.lockState = CursorLockMode.Locked; // Prende o mouse no centro
        Cursor.visible = false; // Deixa invisível
    }

    void DestravarMouse()
    {
        Cursor.lockState = CursorLockMode.None; // Solta o mouse
        Cursor.visible = true; // Mostra a setinha
    }
}