using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("Interfaces")]
    public GameObject pauseMenuUI; 
    public GameObject hudUI;       

    [Header("Configurações")]
    public string nomeCenaMenu = "Menu"; 

    public static bool jogoEstaPausado = false;

    void Start()
    {
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
        pauseMenuUI.SetActive(false);
        if(hudUI != null) hudUI.SetActive(true);
        
        Time.timeScale = 1f;
        jogoEstaPausado = false;

        TravarMouse();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        if(hudUI != null) hudUI.SetActive(false);
        
        Time.timeScale = 0f;
        jogoEstaPausado = true;

        DestravarMouse();
    }

    public void CarregarMenu()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) 
        {
            player.SetActive(false);
        }

        if(hudUI != null) hudUI.SetActive(false);

        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCenaMenu);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    void TravarMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void DestravarMouse()
    {
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }
}