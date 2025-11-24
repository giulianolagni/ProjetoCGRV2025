using UnityEngine;

public class DifficultySetup : MonoBehaviour
{
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

        if (dificuldade == 0) // FÁCIL
        {
            if (scriptNave != null) scriptNave.vidaMaxima = 1000f;
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 5;

            danoGlobalInimigo = 5f;
        }
        else if (dificuldade == 1) // NORMAL
        {
            if (scriptNave != null) scriptNave.vidaMaxima = 100f;
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 10;

            danoGlobalInimigo = 10f;
        }
        else if (dificuldade == 2) // DIFÍCIL
        {
            if (scriptNave != null) scriptNave.vidaMaxima = 50f;
            if (scriptSpawner != null) scriptSpawner.totalDeInimigos = 20;

            danoGlobalInimigo = 10f; 
        }
    }
}