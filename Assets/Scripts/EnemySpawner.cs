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
        Vector2 circuloAleatorio = Random.insideUnitCircle.normalized * raioDeSpawn;
        float yDoPlayer = player.position.y;
        
        Vector3 posicaoFinal = new Vector3(
            player.position.x + circuloAleatorio.x, 
            yDoPlayer, 
            player.position.z + circuloAleatorio.y
        );

        inimigoAtivo = Instantiate(prefabDoInimigo, posicaoFinal, Quaternion.identity);
        
        Debug.Log($"Spawn Y: {posicaoFinal.y} | Player Y: {yDoPlayer}");

        inimigoAtivo.transform.LookAt(player);
        
        Rigidbody rbInimigo = inimigoAtivo.GetComponent<Rigidbody>();
        if(rbInimigo != null)
        {
            rbInimigo.linearVelocity = Vector3.zero; 
        }

        inimigosSpawnados++;
    }
}