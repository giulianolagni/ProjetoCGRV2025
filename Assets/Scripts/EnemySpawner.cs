using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject prefabDoInimigo;
    public int totalDeInimigos = 5;
    public float raioDeSpawn = 50f; 

    private GameObject inimigoAtivo;
    private int inimigosSpawnados = 0;
    private Transform player;

    void Start()
    {
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (objPlayer != null) player = objPlayer.transform;
    }

    void Update()
    {
        if (player == null) return;
        if (inimigosSpawnados >= totalDeInimigos) return;

        if (inimigoAtivo == null)
        {
            SpawnarProximoInimigo();
        }
    }

    void SpawnarProximoInimigo()
    {
        // 1. Gera posição 2D
        Vector2 circuloAleatorio = Random.insideUnitCircle.normalized * raioDeSpawn;

        // 2. Força BRUTA no Y: Pega o Y exato do Player
        float yDoPlayer = player.position.y;
        
        // Monta a posição final manualmente
        Vector3 posicaoFinal = new Vector3(
            player.position.x + circuloAleatorio.x, 
            yDoPlayer, // Y IDÊNTICO
            player.position.z + circuloAleatorio.y
        );

        // 3. Cria o inimigo
        inimigoAtivo = Instantiate(prefabDoInimigo, posicaoFinal, Quaternion.identity);
        
        // 4. Debug para provar que nasceu certo (Olhe no Console!)
        Debug.Log($"Spawn Y: {posicaoFinal.y} | Player Y: {yDoPlayer} (Devem ser iguais)");

        // Faz olhar pro player
        inimigoAtivo.transform.LookAt(player);
        
        // GARANTIA EXTRA: Zera a velocidade vertical se ele tiver Rigidbody
        Rigidbody rbInimigo = inimigoAtivo.GetComponent<Rigidbody>();
        if(rbInimigo != null)
        {
            rbInimigo.linearVelocity = Vector3.zero; // Para Unity 6 (ou .velocity em antigos)
        }

        inimigosSpawnados++;
    }
}