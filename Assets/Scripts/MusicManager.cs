using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Singleton: Garante que só exista UM desse script no jogo todo
    public static MusicManager instance;

    [Header("Suas Músicas")]
    public AudioClip[] playlist; // Arraste suas músicas aqui
    
    [Header("Configurações")]
    [Range(0f, 1f)] public float volume = 0.5f;
    public bool ordemAleatoria = false;

    private AudioSource audioSource;
    private int indiceAtual = 0;

    void Awake()
    {
        // --- LÓGICA PARA NÃO DESTRUIR NO RESTART ---
        if (instance == null)
        {
            // Se eu sou o primeiro DJ, eu assumo o posto
            instance = this;
            DontDestroyOnLoad(gameObject); // A MÁGICA ACONTECE AQUI
        }
        else
        {
            // Se já existe um DJ tocando (de uma cena anterior), 
            // eu (o novo que acabou de carregar) me autodestruo.
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;

        // Começa a tocar se tiver música
        if (playlist.Length > 0)
        {
            TocarMusica(0);
        }
    }

    void Update()
    {
        // Se a música acabou, toca a próxima
        if (!audioSource.isPlaying)
        {
            ProximaMusica();
        }
    }

    void TocarMusica(int index)
    {
        if (playlist.Length == 0) return;

        // Garante que o índice é válido
        indiceAtual = index % playlist.Length;

        audioSource.clip = playlist[indiceAtual];
        audioSource.Play();
    }

    public void ProximaMusica()
    {
        if (playlist.Length == 0) return;

        if (ordemAleatoria)
        {
            indiceAtual = Random.Range(0, playlist.Length);
        }
        else
        {
            // Avança 1, se chegar no fim volta pro 0
            indiceAtual = (indiceAtual + 1) % playlist.Length;
        }

        TocarMusica(indiceAtual);
    }
}