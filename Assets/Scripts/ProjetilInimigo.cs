using UnityEngine;

public class ProjetilInimigo : MonoBehaviour
{
    [SerializeField] private float danoCausado = 5f; 
    [SerializeField] private float tempoDeVida = 5f;

    void Start()
    {
        // Lê a dificuldade global
        danoCausado = DifficultySetup.danoGlobalInimigo;
        
        Destroy(gameObject, tempoDeVida);
    }

    private void OnTriggerEnter(Collider other)
    {
        // IGNORA O PRÓPRIO INIMIGO (Para evitar suicídio caso o passo anterior não tenha resolvido tudo)
        if (other.CompareTag("Untagged") || other.GetComponent<InimigoController>() != null) 
        {
            // Se o tiro for Untagged e bater em algo sem tag ou bater num inimigo, ignoramos (opcional)
            // Mas vamos focar no player:
        }

        // 1. DEBUG: Mostra no console em quem o tiro bateu
        Debug.Log($"TIRO INIMIGO ACERTOU: {other.name} | TAG: {other.tag}");

        // 2. Tenta pegar o script da nave no objeto que bateu
        ArcadeNave_VFinal nave = other.GetComponent<ArcadeNave_VFinal>();

        // 3. SE NÃO ACHOU, tenta pegar no objeto PAI (Caso tenha batido na malha/mesh)
        if (nave == null)
        {
            nave = other.GetComponentInParent<ArcadeNave_VFinal>();
        }

        // 4. Se achou a nave (no objeto ou no pai), aplica dano
        if (nave != null)
        {
            nave.TomarDano(danoCausado);
            Debug.Log($"<color=red>DANO APLICADO!</color> Vida Restante: {nave.VidaAtualPublica}");
            
            // Destrói o tiro
            Destroy(gameObject);
        }
        else
        {
            // Se bateu em algo que não é a nave e não é inimigo (ex: parede), destrói o tiro
            // (Verifique se não está destruindo ao bater no próprio inimigo na saída)
            if (other.GetComponent<InimigoController>() == null)
            {
                Destroy(gameObject);
            }
        }
    }
}