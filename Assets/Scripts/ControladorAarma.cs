using UnityEngine;

public class ControladorArma : MonoBehaviour
{
    [Header("--- CONFIGURAÇÃO DA ARMA ---")]
    public GameObject prefabTiro;
    public Transform[] pontosDeSaida;
    public KeyCode teclaDisparo = KeyCode.Space;

    [Header("--- CADÊNCIA ---")]
    public float intervaloEntreTiros = 0.2f;
    private float proximoTempoDisparo = 0f;

    private Rigidbody rbNave;

    void Start()
    {
        rbNave = GetComponent<Rigidbody>();
        if (rbNave == null)
        {
            Debug.LogWarning("ControladorArma: Sem Rigidbody! Tiros não herdarão velocidade.");
        }
    }

    void Update()
    {
        if (Input.GetKey(teclaDisparo) && Time.time >= proximoTempoDisparo)
        {
            Atirar();
        }
    }

    void Atirar()
    {
        proximoTempoDisparo = Time.time + intervaloEntreTiros;

        if (prefabTiro != null && pontosDeSaida != null && pontosDeSaida.Length > 0)
        {
            // Calcula velocidade frontal da nave
            float velocidadeDaNave = 0f;
            if (rbNave != null)
            {
                velocidadeDaNave = Vector3.Dot(rbNave.linearVelocity, transform.forward);
            }

            foreach (Transform ponto in pontosDeSaida)
            {
                if (ponto != null)
                {
                    GameObject novoTiro = Instantiate(prefabTiro, ponto.position, ponto.rotation);
                    TiroSimples scriptTiro = novoTiro.GetComponent<TiroSimples>();
                    
                    // Passa a velocidade da nave para o tiro
                    if (scriptTiro != null)
                    {
                        scriptTiro.SomarVelocidade(Mathf.Max(0, velocidadeDaNave)); 
                    }
                }
            }
        }
    }
}