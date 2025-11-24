using UnityEngine;

public class DifficultySetup : MonoBehaviour
{
    // VARIÁVEL GLOBAL (STATIC): Qualquer script pode ler isso sem precisar de link
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
            // Ajustes da Nave/Spawn
            if (scriptNave != null) scriptNave.vidaMaxima = 1000f;
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 5;

            // Ajuste do Dano do Inimigo
            danoGlobalInimigo = 5f; // Tira pouco dano
        }
        else if (dificuldade == 1) // --- NORMAL ---
        {
            if (scriptNave != null) scriptNave.vidaMaxima = 100f;
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 10;

            danoGlobalInimigo = 10f; // Dano padrão
        }
        else if (dificuldade == 2) // --- DIFÍCIL ---
        {
            if (scriptNave != null) scriptNave.vidaMaxima = 50f;
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 20;

            danoGlobalInimigo = 25f; // Dano altíssimo (2 tiros matam no Difícil)
        }
    }
}