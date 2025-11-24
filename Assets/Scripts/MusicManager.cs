using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Suas Músicas")]
    public AudioClip[] playlist;
    
    [Header("Configurações")]
    [Range(0f, 1f)] public float volume = 0.5f;
    public bool ordemAleatoria = false;

    private AudioSource audioSource;
    private int indiceAtual = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;

        if (playlist.Length > 0)
        {
            TocarMusica(0);
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            ProximaMusica();
        }
    }

    void TocarMusica(int index)
    {
        if (playlist.Length == 0) return;

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
            indiceAtual = (indiceAtual + 1) % playlist.Length;
        }

        TocarMusica(indiceAtual);
    }
}