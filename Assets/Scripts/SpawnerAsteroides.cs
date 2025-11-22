// using System.Collections;
// using UnityEngine;

// public class SpawnerAsteroides : MonoBehaviour
// {
//     [Header("--- CONFIGURAÇÃO ---")]
//     public GameObject prefabAsteroide; 
//     public Transform alvo; 
    
//     [Header("Densidade")]
//     public float intervaloSpawn = 0.1f; 
//     public float raioSpawn = 200f; 

//     [Header("Tamanho dos Asteroides")]
//     [Tooltip("Tamanho mínimo. Aumente se estiver muito pequeno.")]
//     public float escalaMinima = 10f; 
//     [Tooltip("Tamanho máximo.")]
//     public float escalaMaxima = 30f;

//     [Header("Performance")]
//     public float tempoDeVida = 30f;

//     void Start()
//     {
//         if (alvo == null)
//         {
//             GameObject player = GameObject.FindGameObjectWithTag("Player");
//             if (player != null) alvo = player.transform;
//         }

//         if (prefabAsteroide != null)
//         {
//             StartCoroutine(RotinaDeSpawn());
//         }
//     }

//     IEnumerator RotinaDeSpawn()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(intervaloSpawn);

//             if (alvo != null)
//             {
//                 Vector3 posAleatoria = Random.onUnitSphere * raioSpawn;
//                 Vector3 posFinal = alvo.position + posAleatoria;
                
//                 GameObject asteroide = Instantiate(prefabAsteroide, posFinal, Random.rotation);

//                 // --- AQUI ESTÁ A CORREÇÃO ---
//                 // Agora usamos os valores que você colocar no Inspector
//                 float tamanhoSorteado = Random.Range(escalaMinima, escalaMaxima);
//                 asteroide.transform.localScale = Vector3.one * tamanhoSorteado;

//                 if (tempoDeVida > 0)
//                 {
//                     Destroy(asteroide, tempoDeVida);
//                 }
//             }
//         }
//     }
// }