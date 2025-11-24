using UnityEngine;

public class DifficultySetup : MonoBehaviour
{
    // VARIÁVEL GLOBAL (STATIC)
    public static float danoGlobalInimigo = 10f;

    void Awake()
    {
        AplicarDificuldade();
    }

    void AplicarDificuldade()
    {
        int dificuldade = PlayerPrefs.GetInt("DificuldadeJogo", 1);
        Debug.Log(">>> DIFICULDADE CARREGADA: " + dificuldade);

        ArcadeNave_VFinal scriptNave = FindObjectOfType<ArcadeNave_VFinal>();
        SpawnManager scriptSpawner = FindObjectOfType<SpawnManager>();

        if (dificuldade == 0) // --- FÁCIL ---
        {
            if (scriptNave != null) scriptNave.vidaMaxima = 1000f; // Tanque
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 5;

            danoGlobalInimigo = 5f; // Dano baixo
        }
        else if (dificuldade == 1) // --- NORMAL ---
        {
            if (scriptNave != null) scriptNave.vidaMaxima = 100f; // Padrão
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 10;

            danoGlobalInimigo = 10f; // Dano médio
        }
        else if (dificuldade == 2) // --- DIFÍCIL ---
        {
            if (scriptNave != null) scriptNave.vidaMaxima = 50f; // Vida Curta
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 20; // Muitos inimigos

            // ANTES ERA 25 (Muito alto). AGORA É 10.
            // Motivo: Se o inimigo acertar as 4 balas (4x10=40), você ainda sobra com 10 de vida.
            danoGlobalInimigo = 10f; 
        }
    }
}