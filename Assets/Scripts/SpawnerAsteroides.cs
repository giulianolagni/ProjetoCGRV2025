
using System.Collections;
using UnityEngine;

public class SpawnerAsteroides : MonoBehaviour
{
    [Header("--- CONFIGURAÇÃO ---")]
    public GameObject prefabAsteroide; // Arrasta o teu Prefab "Asteroide" aqui
    public Transform alvo; // Arrasta a tua Nave (Jogador) aqui
    
    public float intervaloSpawn = 1.5f; // Tempo entre cada asteroide
    public float raioSpawn = 800f; // Distância a que eles aparecem da nave

    void Start()
    {
        // Se não arrastaste a nave, tenta encontrá-la pela tag
        if (alvo == null)
        {
            // Certifica-te que a tua nave tem a tag "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                alvo = player.transform;
            }
        }

        // Inicia a rotina de spawn
        if (prefabAsteroide != null && alvo != null)
        {
            StartCoroutine(RotinaDeSpawn());
        }
        else
        {
            Debug.LogError("Spawner: Prefab ou Alvo (Nave) não definidos!");
        }
    }

    IEnumerator RotinaDeSpawn()
    {
        // Loop infinito
        while (true)
        {
            // 1. Espera pelo tempo definido
            yield return new WaitForSeconds(intervaloSpawn);

            // 2. Calcula uma posição aleatória num círculo à volta da nave
            // (Random.onUnitSphere dá um ponto numa esfera de raio 1)
            Vector3 posAleatoria = (Random.onUnitSphere * raioSpawn) + alvo.position;
            
            // 3. Cria (Instancia) o asteroide nessa posição
            Instantiate(prefabAsteroide, posAleatoria, Random.rotation);
        }
    }
}