using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    [Header("Configuração")]
    public Transform alvoPrincipal; // O Objeto real (Inimigo ou Loot) que este ícone representa
    
    [Header("Visual de Altura")]
    public float escalaAcima = 1.5f;   // Tamanho quando está muito acima
    public float escalaNormal = 1.0f;  // Tamanho na mesma altura
    public float escalaAbaixo = 0.5f;  // Tamanho quando está muito abaixo
    
    [Header("Cores (Opcional)")]
    public bool mudarCor = true;
    public Color corAcima = Color.white;       // Brilhante
    public Color corNormal = Color.gray;       // Normal
    public Color corAbaixo = new Color(1,1,1, 0.3f); // Transparente/Escuro

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Vector3 escalaInicial;

    void Start()
    {
        // Encontra o player automaticamente
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (objPlayer != null) player = objPlayer.transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        escalaInicial = transform.localScale;
        
        // Se não definiu o alvo manualmente no inspector, tenta pegar o pai
        if (alvoPrincipal == null) alvoPrincipal = transform.parent;
    }

    void LateUpdate()
    {
        if (player == null || alvoPrincipal == null) return;

        // 1. Trava a Rotação (Para o ícone não girar junto com o inimigo)
        // O ícone fica sempre "em pé" em relação ao mapa
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        // 2. Cálculo da Diferença de Altura (Y)
        float diferencaY = alvoPrincipal.position.y - player.position.y;

        // --- LÓGICA DE TAMANHO E COR ---
        float novaEscala = escalaNormal;
        Color novaCor = corNormal;

        if (diferencaY > 10f) // Está 10 metros ACIMA
        {
            novaEscala = escalaAcima;
            novaCor = corAcima;
        }
        else if (diferencaY < -10f) // Está 10 metros ABAIXO
        {
            novaEscala = escalaAbaixo;
            novaCor = corAbaixo;
        }

        // Aplica o tamanho
        transform.localScale = Vector3.Lerp(transform.localScale, escalaInicial * novaEscala, Time.deltaTime * 5f);

        // Aplica a cor
        if (mudarCor && spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, novaCor, Time.deltaTime * 5f);
        }
    }
}