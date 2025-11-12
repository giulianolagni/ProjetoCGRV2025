using UnityEngine;

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

    [Header("--- COMBATE (NOVO) ---")]
    [Tooltip("Arrasta aqui um objeto vazio posicionado no bico da nave (opcional)")]
    [SerializeField] private Transform pontoDeTiro;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.linearDamping = 1f;
        rb.angularDamping = 4f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        velocidadeAtual = velocidadeCruzeiro;

        if (camaraPrincipal == null) camaraPrincipal = Camera.main;
        if (camaraPrincipal != null) fovNormal = camaraPrincipal.fieldOfView;

        // Se não definiste um ponto de tiro, usa a própria posição da nave
        if (pontoDeTiro == null) pontoDeTiro = this.transform;
    }

    void Update()
    {
        // 1. INPUTS
        bool temInputRoll = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f;
        bool temInputMira = posicaoMira.magnitude > deadzone;
        if (temInputRoll || temInputMira) tempoSemInput = 0f;
        else tempoSemInput += Time.deltaTime;

        // 2. MIRA (MOUSE AIM)
        posicaoMira.x += Input.GetAxis("Mouse X") * sensibilidadeRato * limiteRato * 0.1f;
        posicaoMira.y += Input.GetAxis("Mouse Y") * sensibilidadeRato * limiteRato * 0.1f;
        posicaoMira = Vector2.ClampMagnitude(posicaoMira, limiteRato);
        if (!temInputMira) posicaoMira = Vector2.Lerp(posicaoMira, Vector2.zero, Time.deltaTime * 5f);

        // 3. VELOCIDADE
        float alvoVelocidade = velocidadeCruzeiro;
        if (Input.GetKey(KeyCode.W)) alvoVelocidade = velocidadeTurbo;
        else if (Input.GetKey(KeyCode.S)) alvoVelocidade = (velocidadeAtual > 10f) ? 0f : velocidadeRe;
        velocidadeAtual = Mathf.SmoothDamp(velocidadeAtual, alvoVelocidade, ref velocidadeVelocity, tempoDeAceleracao);

        // 4. ÁUDIO V10
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

        // 5. FOV
        if (camaraPrincipal != null)
        {
            float percentagemVelocidade = Mathf.InverseLerp(0, velocidadeTurbo, velocidadeAtual);
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, Mathf.Lerp(fovNormal, fovTurbo, percentagemVelocidade), Time.deltaTime * 2f);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * velocidadeAtual;

        Vector2 inputManche = posicaoMira / limiteRato;
        float pitch = -inputManche.y * velocidadeGiroPitchYaw * Time.fixedDeltaTime;
        float yaw = inputManche.x * velocidadeGiroPitchYaw * Time.fixedDeltaTime;
        float rollManual = -Input.GetAxis("Horizontal") * velocidadeRoll * Time.fixedDeltaTime;
        float autoBank = -inputManche.x * forcaAutoBanking * Time.fixedDeltaTime;

        transform.Rotate(Vector3.right * pitch, Space.Self);
        transform.Rotate(Vector3.up * yaw, Space.Self);
        transform.Rotate(Vector3.forward * (rollManual + autoBank), Space.Self);

        if (tempoSemInput >= tempoParaNivelar)
        {
             Vector3 euler = transform.localEulerAngles;
             float zAtual = (euler.z > 180) ? euler.z - 360 : euler.z;
             transform.localEulerAngles = new Vector3(euler.x, euler.y, Mathf.Lerp(zAtual, 0, Time.fixedDeltaTime));
        }
    }

void OnGUI()
    {
        // Se houver algum erro grave, não faz nada para não travar o jogo
        if (camaraPrincipal == null) return;

        float centroX = Screen.width / 2f;
        float centroY = Screen.height / 2f;

        // --- MIRA REALISTA (BORESIGHT) ---
        // Só tenta desenhar se tivermos um ponto de tiro definido
        if (pontoDeTiro != null)
        {
            Vector3 posicaoAlvoReal = pontoDeTiro.position + (pontoDeTiro.forward * 1000f);
            Vector3 miraNoEcra = camaraPrincipal.WorldToScreenPoint(posicaoAlvoReal);

            // Verifica se está na frente da câmera
            if (miraNoEcra.z > 0)
            {
                GUI.color = new Color(1, 1, 1, 0.9f);
                // Inverte Y porque o GUI começa do topo, mas o ScreenPoint começa de baixo
                GUI.Label(new Rect(miraNoEcra.x - 10, Screen.height - miraNoEcra.y - 10, 20, 20), "+");
            }
        }

        // --- MOUSE AIM (Círculo Flutuante) ---
        GUI.color = new Color(1, 1, 1, 0.5f);
        GUI.Label(new Rect(centroX + posicaoMira.x - 10, centroY - posicaoMira.y - 10, 20, 20), "O");

        // --- HUD VELOCIDADE ---
        GUI.color = new Color(0, 0, 0, 0.5f);
        GUI.DrawTexture(new Rect(20, 20, 160, 60), Texture2D.whiteTexture);
        
        GUI.color = Color.white;
        // Verifica se GUI.skin não é nulo antes de usar (raro falhar, mas possível)
        if (GUI.skin != null)
        {
            GUI.skin.label.fontSize = 16;
            GUI.skin.label.fontStyle = FontStyle.Bold;
        }
        GUI.Label(new Rect(30, 25, 150, 30), $"SPD: {velocidadeAtual:F0} km/h");

        float pctThrottle = Mathf.InverseLerp(velocidadeRe, velocidadeTurbo, velocidadeAtual);
        GUI.color = Color.Lerp(Color.grey, Color.green, pctThrottle);
        if(velocidadeAtual > velocidadeTurbo * 0.9f) GUI.color = Color.red;
        
        GUI.DrawTexture(new Rect(30, 55, 140 * pctThrottle, 15), Texture2D.whiteTexture);
        GUI.color = Color.white;
    }
}