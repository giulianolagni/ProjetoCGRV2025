using UnityEngine;

public class ProjetilInimigo : MonoBehaviour
{
    [SerializeField] private float danoCausado = 5f; 
    [SerializeField] private float tempoDeVida = 5f;

    void Start()
    {
        danoCausado = DifficultySetup.danoGlobalInimigo;
        Destroy(gameObject, tempoDeVida);
    }

    private void OnTriggerEnter(Collider other)
    {
        ArcadeNave_VFinal nave = other.GetComponent<ArcadeNave_VFinal>();

        if (nave == null)
        {
            nave = other.GetComponentInParent<ArcadeNave_VFinal>();
        }

        if (nave != null)
        {
            nave.TomarDano(danoCausado);
            Debug.Log($"<color=red>DANO APLICADO!</color> Vida Restante: {nave.VidaAtualPublica}");
            Destroy(gameObject);
        }
        else
        {
            if (other.GetComponent<InimigoController>() == null)
            {
                Destroy(gameObject);
            }
        }
    }
}