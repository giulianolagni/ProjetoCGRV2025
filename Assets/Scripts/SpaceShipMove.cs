using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para reiniciar o jogo

public class ArcadeNave_VFinal : MonoBehaviour
{
    [Header("--- VELOCIDADE (CONFIG WT) ---")]
    [SerializeField] private float velocidadeCruzeiro = 100f;
    [SerializeField] private float velocidadeTurbo = 1300f;
    [SerializeField] private float velocidadeRe = -100f;
    [SerializeField] private float tempoDeAceleracao = 2.5f;
    private float velocidadeVelocity;

    [Header("--- CURVAS (MANCHE) ---")]
    [SerializeField] private float velocidadeGiroPitchYaw = 80f;
    [SerializeField] private float velocidadeRoll = 200f;
    [SerializeField] private float forcaAutoBanking = 5f;
    [SerializeField] private float tempoParaNivelar = 4f;

    [Header("--- RATO (MIRA WT) ---")]
    [SerializeField] private float limiteRato = 350f;
    [SerializeField] private float deadzone = 15f;
    [SerializeField] private float sensibilidadeRato = 1f;

    [Header("--- COMBATE (CONFIG) ---")]
    [SerializeField] private Transform pontoDeTiro;
    
    [Header("--- VIDA E GAME OVER ---")]
    [SerializeField] private float vidaMaxima = 100f;
    [SerializeField] private GameObject telaGameOver; 
    private float vidaAtual;
    private bool estaMorto = false;

    [Header("--- MISSÃO / COLETA ---")]
    public int fragmentosTotais = 5;
    private int fragmentosColetados = 0;
    // Variável para impedir contagem dupla (Cooldown de coleta)
    private float tempoUltimaColeta = 0f;

    [Header("--- EFEITOS VISUAIS ---")]
    [SerializeField] private Camera camaraPrincipal;
    [SerializeField] private float fovNormal = 60f;
    [SerializeField] private float fovTurbo = 85f;

    [Header("--- ÁUDIO ---")]
    [SerializeField] private AudioSource motorAudioSource;
    [SerializeField] private float pitchMinimo = 0.8f;
    [SerializeField] private float pitchTurbo = 3.5f;

    private Rigidbody rb;
    private float velocidadeAtual;
    private Vector2 posicaoMira;
    private float tempoSemInput = 0f;

    // Propriedade pública para outros scripts lerem a vida
    public float VidaAtualPublica => vidaAtual; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        // Configurações de física (ajustadas para Unity 6)
        rb.linearDamping = 1f;
        rb.angularDamping = 4f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        velocidadeAtual = velocidadeCruzeiro;
        vidaAtual = vidaMaxima;
        estaMorto = false;
        fragmentosColetados = 0;

        // Garante que o jogo comece rodando
        Time.timeScale = 1f; 
        if(telaGameOver != null) telaGameOver.SetActive(false);

        if (camaraPrincipal == null) camaraPrincipal = Camera.main;
        if (camaraPrincipal != null) fovNormal = camaraPrincipal.fieldOfView;
        if (pontoDeTiro == null) pontoDeTiro = this.transform;
    }

    void Update()
    {
        // Se morreu, para tudo
        if (estaMorto) return;

        // --- INPUTS ---
        bool temInputRoll = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f;
        bool temInputMira = posicaoMira.magnitude > deadzone;
        if (temInputRoll || temInputMira) tempoSemInput = 0f;
        else tempoSemInput += Time.deltaTime;

        // --- MIRA ---
        posicaoMira.x += Input.GetAxis("Mouse X") * sensibilidadeRato * limiteRato * 0.1f;
        posicaoMira.y += Input.GetAxis("Mouse Y") * sensibilidadeRato * limiteRato * 0.1f;
        posicaoMira = Vector2.ClampMagnitude(posicaoMira, limiteRato);
        if (!temInputMira) posicaoMira = Vector2.Lerp(posicaoMira, Vector2.zero, Time.deltaTime * 5f);

        // --- VELOCIDADE ---
        float alvoVelocidade = velocidadeCruzeiro;
        if (Input.GetKey(KeyCode.W)) alvoVelocidade = velocidadeTurbo;
        else if (Input.GetKey(KeyCode.S)) alvoVelocidade = (velocidadeAtual > 10f) ? 0f : velocidadeRe;
        velocidadeAtual = Mathf.SmoothDamp(velocidadeAtual, alvoVelocidade, ref velocidadeVelocity, tempoDeAceleracao);

        // --- ÁUDIO ---
        if (motorAudioSource != null)
        {
            float targetPitch = pitchMinimo;
            if (velocidadeAtual >= 0)
            {
                float ratio = Mathf.InverseLerp(0, velocidadeTurbo, velocidadeAtual);
                targetPitch = Mathf.Lerp(pitchMinimo, pitchTurbo, Mathf.Pow(ratio, 1.5f));
            }
            else
            {
                float ratioRe = Mathf.InverseLerp(0, velocidadeRe, velocidadeAtual);
                targetPitch = Mathf.Lerp(pitchMinimo, pitchMinimo * 1.2f, ratioRe);
            }
            motorAudioSource.pitch = Mathf.Lerp(motorAudioSource.pitch, targetPitch, Time.deltaTime * 3f);
        }

        // --- FOV ---
        if (camaraPrincipal != null)
        {
            float percentagemVelocidade = Mathf.InverseLerp(0, velocidadeTurbo, velocidadeAtual);
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, Mathf.Lerp(fovNormal, fovTurbo, percentagemVelocidade), Time.deltaTime * 2f);
        }
    }

    void FixedUpdate()
    {
        if (estaMorto) return;

        // Aplica movimento
        // NOTA: Se der erro vermelho aqui, troque 'linearVelocity' por 'velocity'
        rb.linearVelocity = transform.forward * velocidadeAtual;

        // Aplica Rotação
        Vector2 inputManche = posicaoMira / limiteRato;
        float pitch = -inputManche.y * velocidadeGiroPitchYaw * Time.fixedDeltaTime;
        float yaw = inputManche.x * velocidadeGiroPitchYaw * Time.fixedDeltaTime;
        float rollManual = -Input.GetAxis("Horizontal") * velocidadeRoll * Time.fixedDeltaTime;
        float autoBank = -inputManche.x * forcaAutoBanking * Time.fixedDeltaTime;

        transform.Rotate(Vector3.right * pitch, Space.Self);
        transform.Rotate(Vector3.up * yaw, Space.Self);
        transform.Rotate(Vector3.forward * (rollManual + autoBank), Space.Self);

        // Auto Nivelamento
        if (tempoSemInput >= tempoParaNivelar)
        {
             Vector3 euler = transform.localEulerAngles;
             float zAtual = (euler.z > 180) ? euler.z - 360 : euler.z;
             transform.localEulerAngles = new Vector3(euler.x, euler.y, Mathf.Lerp(zAtual, 0, Time.fixedDeltaTime));
        }
    }

    // --- SISTEMA DE DANO ---
    public void TomarDano(float dano)
    {
        if (estaMorto) return; 

        vidaAtual -= dano;
        vidaAtual = Mathf.Max(vidaAtual, 0f);
        
        // Debug.Log($"PLAYER ATINGIDO! Vida: {vidaAtual}");

        if (vidaAtual <= 0)
        {
            GameOver();
        }
    }
    
    void GameOver()
    {
        estaMorto = true;
        Debug.Log("<color=red>GAME OVER!</color>");

        if (telaGameOver != null) telaGameOver.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; 
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Dano por impacto físico
        float forcaImpacto = collision.relativeVelocity.magnitude;
        if (forcaImpacto > 5f)
        {
            TomarDano(forcaImpacto * 0.5f);
        }
    }

    // --- SISTEMA DE COLETA (COM A CORREÇÃO) ---
    public void ColetarFragmento()
    {
        // TRAVA DE SEGURANÇA:
        // Se passou menos de 0.1 segundos desde a última coleta, IGNORA.
        if (Time.time < tempoUltimaColeta + 0.1f) return;

        // Atualiza o tempo
        tempoUltimaColeta = Time.time;

        fragmentosColetados++;
        Debug.Log($"<color=cyan>FRAGMENTO COLETADO!</color> Total: {fragmentosColetados}/{fragmentosTotais}");

        if (fragmentosColetados >= fragmentosTotais)
        {
            Debug.Log("VOCÊ COMPLETOU O MAPA ESTELAR!");
            // Aqui você pode chamar: SceneManager.LoadScene("CenaDeVitoria");
        }
    }

    void OnGUI()
    {
        if (estaMorto || camaraPrincipal == null) return;

        float centroX = Screen.width / 2f;
        float centroY = Screen.height / 2f;

        // Mira Boresight
        if (pontoDeTiro != null)
        {
            Vector3 posicaoAlvoReal = pontoDeTiro.position + (pontoDeTiro.forward * 1000f);
            Vector3 miraNoEcra = camaraPrincipal.WorldToScreenPoint(posicaoAlvoReal);
            if (miraNoEcra.z > 0)
            {
                GUI.color = new Color(1, 1, 1, 0.9f);
                GUI.Label(new Rect(miraNoEcra.x - 10, Screen.height - miraNoEcra.y - 10, 20, 20), "+");
            }
        }

        // Mira Mouse
        GUI.color = new Color(1, 1, 1, 0.5f);
        GUI.Label(new Rect(centroX + posicaoMira.x - 10, centroY - posicaoMira.y - 10, 20, 20), "O");

        // Fundo HUD
        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.DrawTexture(new Rect(20, 20, 160, 60), Texture2D.whiteTexture);
        
        // Texto Velocidade
        GUI.color = Color.white;
        if (GUI.skin != null) { GUI.skin.label.fontSize = 16; GUI.skin.label.fontStyle = FontStyle.Bold; }
        GUI.Label(new Rect(30, 25, 150, 30), $"SPD: {velocidadeAtual:F0} km/h");

        // Barra Velocidade
        float pctThrottle = Mathf.InverseLerp(velocidadeRe, velocidadeTurbo, velocidadeAtual);
        GUI.color = Color.Lerp(Color.grey, Color.green, pctThrottle);
        if(velocidadeAtual > velocidadeTurbo * 0.9f) GUI.color = Color.red;
        GUI.DrawTexture(new Rect(30, 55, 140 * pctThrottle, 15), Texture2D.whiteTexture);
        
        // Barra Vida
        float yPos = 90f; 
        float larguraBarra = 140f; 
        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.DrawTexture(new Rect(20, yPos - 5, 160, 30), Texture2D.whiteTexture);

        float pctVida = vidaAtual / vidaMaxima;
        GUI.color = Color.Lerp(Color.red, Color.green, pctVida);
        GUI.DrawTexture(new Rect(30, yPos, larguraBarra * pctVida, 20), Texture2D.whiteTexture);

        GUI.color = Color.white;
        if (GUI.skin != null) GUI.skin.label.fontSize = 14;
        GUI.Label(new Rect(30, yPos, 150, 20), $"HP: {vidaAtual:F0} / {vidaMaxima:F0}");

        // --- HUD COLETA ---
        float yFrag = 130f; 
        GUI.color = new Color(0, 0, 0, 0.5f); 
        GUI.DrawTexture(new Rect(20, yFrag - 5, 300, 30), Texture2D.whiteTexture);

        GUI.color = Color.cyan; 
        GUI.Label(new Rect(30, yFrag, 300, 20), $"MAPA ESTELAR: {fragmentosColetados} / {fragmentosTotais}");
    }
}