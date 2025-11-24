using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [Header("Configuração")]
    public Transform alvoPrincipal; 
    
    [Header("Visual de Altura")]
    public float escalaAcima = 1.5f;
    public float escalaNormal = 1.0f;
    public float escalaAbaixo = 0.5f;
    
    [Header("Cores (Opcional)")]
    public bool mudarCor = true;
    public Color corAcima = Color.white;
    public Color corNormal = Color.gray;
    public Color corAbaixo = new Color(1,1,1, 0.3f);

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Vector3 escalaInicial;

    void Start()
    {
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (objPlayer != null) player = objPlayer.transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaInicial = transform.localScale;
        
        if (alvoPrincipal == null) alvoPrincipal = transform.parent;
    }

    void LateUpdate()
    {
        if (player == null || alvoPrincipal == null) return;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        float diferencaY = alvoPrincipal.position.y - player.position.y;

        float novaEscala = escalaNormal;
        Color novaCor = corNormal;

        if (diferencaY > 10f) 
        {
            novaEscala = escalaAcima;
            novaCor = corAcima;
        }
        else if (diferencaY < -10f) 
        {
            novaEscala = escalaAbaixo;
            novaCor = corAbaixo;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, escalaInicial * novaEscala, Time.deltaTime * 5f);

        if (mudarCor && spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, novaCor, Time.deltaTime * 5f);
        }
    }
}