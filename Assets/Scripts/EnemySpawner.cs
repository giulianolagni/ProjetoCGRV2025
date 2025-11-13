// C# (Unity)
using UnityEngine;
using System.Collections; // Precisa deste para Coroutines!

public class SpawnManager : MonoBehaviour
{
    public GameObject prefabDoInimigo;
    public int navesParaSpawnar = 5;
    public float tempoEntreSpawns = 0.5f; // Meio segundo entre cada nave

    [Header("Área de Spawn")]
    [Tooltip("A largura total da formação. Ex: 40 significa de -20 a +20.")]
    public float larguraDaFormacao = 40f; 
    public float posY = 0.0f;
    public float posZ = 20.0f;

    void Start()
    {
        StartCoroutine(SpawnaEsquadrao());
    }

    // Esta é a Coroutine.
    IEnumerator SpawnaEsquadrao()
    {
        // Loop que roda 5 vezes (passando o 'i' de 0 a 4)
        for (int i = 0; i < navesParaSpawnar; i++)
        {
            // 1. Chama a nossa função de criar UM inimigo,
            // dizendo a ele qual é o seu "número na fila" (o 'i')
            SpawnaInimigo(i);

            // 2. Pausa este script
            yield return new WaitForSeconds(tempoEntreSpawns);
        }
    }

    // --- MUDANÇA PRINCIPAL AQUI ---
    // Agora esta função recebe o 'i' (número da nave)
    void SpawnaInimigo(int i)
    {
        // Se temos menos de 2 naves, não divide por zero
        if (navesParaSpawnar <= 1)
        {
            Vector3 posCentro = new Vector3(0, posY, posZ);
            Instantiate(prefabDoInimigo, posCentro, Quaternion.Euler(0, 180, 0));
            return;
        }

        // 1. Calcula o espaço entre as naves
        // Ex: Largura de 40 / (5 - 1) naves = 10 unidades de espaço
        float espacamento = larguraDaFormacao / (navesParaSpawnar - 1);

        // 2. Calcula a posição X desta nave
        // Ex (i=0): -(40 / 2) + (0 * 10) = -20
        // Ex (i=1): -(40 / 2) + (1 * 10) = -10
        // Ex (i=4): -(40 / 2) + (4 * 10) = +20
        float posX = -(larguraDaFormacao / 2) + (i * espacamento);

        // 3. Define a posição final
        Vector3 posicaoDeSpawn = new Vector3(posX, posY, posZ);

        // 4. Cria a nave
        Instantiate(prefabDoInimigo, posicaoDeSpawn, Quaternion.Euler(0, 180, 0));
    }
}