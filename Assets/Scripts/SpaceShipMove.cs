using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeNave_VFinal : MonoBehaviour
{
    [Header("--- VELOCIDADE ---")]
    [SerializeField] private float velocidadeCruzeiro = 100f;
    [SerializeField] private float velocidadeTurbo = 1300f;
    [SerializeField] private float velocidadeRe = -100f;
    [SerializeField] private float tempoDeAceleracao = 2.5f;
    private float velocidadeVelocity;

    [Header("--- CONTROLES ---")]
    [SerializeField] private float velocidadeGiroPitchYaw = 80f;
    [SerializeField] private float velocidadeRoll = 200f;
    [SerializeField] private float forcaAutoBanking = 5f;
    [SerializeField] private float tempoParaNivelar = 4f;
    [SerializeField] private float limiteRato = 350f;
    [SerializeField] private float sensibilidadeRato = 1f;

    [Header("--- COMBATE ---")]
    [SerializeField] private Transform pontoDeTiro;
    public float vidaMaxima = 100f;
    [SerializeField] private GameObject telaGameOver; 
    [SerializeField] private GameObject hudGameObject;
    
    [Header("--- MISSÃO (FRAGMENTOS) ---")]
    public int fragmentosTotais = 5;
    public RotaManager rotaManager;
    private int fragmentosColetados = 0;
    private float tempoUltimaColeta = 0f;

    [Header("--- VISUAL & AUDIO ---")]
    [SerializeField] private Camera camaraPrincipal;
    [SerializeField] private AudioSource motorAudioSource;
    [SerializeField] private float fovNormal = 60f;
    [SerializeField] private float fovTurbo = 85f;
    
    private Rigidbody rb;
    private float velocidadeAtual;
    private Vector2 posicaoMira;
    private float tempoSemInput = 0f;
    private float vidaAtual;
    
    private bool estaMorto = false;
    private bool jogoPausado = false;
    
    public float VidaAtualPublica => vidaAtual; 

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
        vidaAtual = vidaMaxima;
        estaMorto = false;
        jogoPausado = false;
        fragmentosColetados = 0;

        Time.timeScale = 1f; 
        if(telaGameOver != null) telaGameOver.SetActive(false);

        if (camaraPrincipal == null) camaraPrincipal = Camera.main;
        if (camaraPrincipal != null) fovNormal = camaraPrincipal.fieldOfView;
        if (pontoDeTiro == null) pontoDeTiro = this.transform;
    }

    void Update()
    {
        if (estaMorto || Time.timeScale == 0) return;

        bool temInput = posicaoMira.magnitude > 15f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f;
        if (temInput) tempoSemInput = 0f; else tempoSemInput += Time.deltaTime;

        posicaoMira.x += Input.GetAxis("Mouse X") * sensibilidadeRato * limiteRato * 0.1f;
        posicaoMira.y += Input.GetAxis("Mouse Y") * sensibilidadeRato * limiteRato * 0.1f;
        posicaoMira = Vector2.ClampMagnitude(posicaoMira, limiteRato);
        if (!temInput) posicaoMira = Vector2.Lerp(posicaoMira, Vector2.zero, Time.deltaTime * 5f);
        
        float alvo = Input.GetKey(KeyCode.W) ? velocidadeTurbo : (Input.GetKey(KeyCode.S) ? (velocidadeAtual > 10 ? 0 : velocidadeRe) : velocidadeCruzeiro);
        velocidadeAtual = Mathf.SmoothDamp(velocidadeAtual, alvo, ref velocidadeVelocity, tempoDeAceleracao);

        if (motorAudioSource != null && !jogoPausado)
        {
            float pitch = velocidadeAtual >= 0 ? Mathf.Lerp(0.8f, 3.5f, Mathf.Pow(velocidadeAtual/velocidadeTurbo, 1.5f)) : 0.8f;
            motorAudioSource.pitch = Mathf.Lerp(motorAudioSource.pitch, pitch, Time.deltaTime * 3f);
        }

        if (camaraPrincipal != null)
        {
            float fovAlvo = Mathf.Lerp(fovNormal, fovTurbo, velocidadeAtual/velocidadeTurbo);
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, fovAlvo, Time.deltaTime * 2f);
        }
    }

    void FixedUpdate()
    {
        if (estaMorto) return;

        rb.linearVelocity = transform.forward * velocidadeAtual; 

        Vector2 input = posicaoMira / limiteRato;
        transform.Rotate(Vector3.right * (-input.y * velocidadeGiroPitchYaw * Time.fixedDeltaTime), Space.Self);
        transform.Rotate(Vector3.up * (input.x * velocidadeGiroPitchYaw * Time.fixedDeltaTime), Space.Self);
        
        float roll = -Input.GetAxis("Horizontal") * velocidadeRoll * Time.fixedDeltaTime;
        float autoBank = -input.x * forcaAutoBanking * Time.fixedDeltaTime;
        transform.Rotate(Vector3.forward * (roll + autoBank), Space.Self);

        if (tempoSemInput >= tempoParaNivelar)
        {
             Vector3 e = transform.localEulerAngles;
             float z = (e.z > 180) ? e.z - 360 : e.z;
             transform.localEulerAngles = new Vector3(e.x, e.y, Mathf.Lerp(z, 0, Time.fixedDeltaTime));
        }
    }

    public void TomarDano(float dano)
    {
        if (estaMorto || jogoPausado) return;
        vidaAtual -= dano;
        if (vidaAtual <= 0) GameOver();
    }

    void GameOver()
    {
        estaMorto = true;
        
        if (telaGameOver != null) telaGameOver.SetActive(true);
        if (hudGameObject != null) hudGameObject.SetActive(false); 

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
    
    public void DesativarSistemas()
    {
        jogoPausado = true;
        if (motorAudioSource != null)
        {
            motorAudioSource.Stop();
            motorAudioSource.volume = 0;
        }
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ColetarFragmento()
    {
        if (Time.time < tempoUltimaColeta + 0.1f) return;
        tempoUltimaColeta = Time.time;

        fragmentosColetados++;
        Debug.Log($"FRAGMENTO: {fragmentosColetados}/{fragmentosTotais}");

        if (fragmentosColetados >= fragmentosTotais)
        {
            if (rotaManager != null) rotaManager.IniciarRotaDeFuga(this.transform);
        }
    }

    void OnCollisionEnter(Collision c) 
    { 
        if(c.relativeVelocity.magnitude > 5) TomarDano(c.relativeVelocity.magnitude * 0.5f); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint") && rotaManager != null)
        {
            rotaManager.CheckpointAlcancado(other.gameObject);
        }
    }

    void OnGUI()
    {
        if (estaMorto || jogoPausado || Time.timeScale == 0 || camaraPrincipal == null) return;

        float cx = Screen.width / 2f;
        float cy = Screen.height / 2f;

        if (pontoDeTiro != null)
        {
            Vector3 posReal = pontoDeTiro.position + (pontoDeTiro.forward * 1000f);
            Vector3 miraTela = camaraPrincipal.WorldToScreenPoint(posReal);
            
            if (miraTela.z > 0)
            {
                GUI.color = new Color(1, 1, 1, 0.5f); 
                GUI.Label(new Rect(miraTela.x - 10, Screen.height - miraTela.y - 10, 20, 20), "+");
            }
        }

        GUI.color = new Color(1, 1, 1, 0.5f);
        GUI.Label(new Rect(cx + posicaoMira.x - 10, cy - posicaoMira.y - 10, 20, 20), "O");

        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(20, 20, 160, 60), Texture2D.whiteTexture);
        GUI.color = Color.white;
        if (GUI.skin != null) { GUI.skin.label.fontSize = 16; GUI.skin.label.fontStyle = FontStyle.Bold; }
        GUI.Label(new Rect(30, 25, 150, 30), $"SPD: {velocidadeAtual:F0}");
        float pctSpd = Mathf.InverseLerp(velocidadeRe, velocidadeTurbo, velocidadeAtual);
        GUI.DrawTexture(new Rect(30, 55, 140 * pctSpd, 10), Texture2D.whiteTexture);

        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(20, 90, 160, 40), Texture2D.whiteTexture);
        float pctVida = vidaAtual / vidaMaxima;
        GUI.color = Color.Lerp(Color.red, Color.green, pctVida);
        GUI.DrawTexture(new Rect(30, 100, 140 * pctVida, 20), Texture2D.whiteTexture);
        GUI.color = Color.white;
        if (GUI.skin != null) GUI.skin.label.fontSize = 14;
        GUI.Label(new Rect(35, 100, 140, 20), $"HP: {vidaAtual:F0}");

        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(20, 140, 200, 30), Texture2D.whiteTexture);
        GUI.color = Color.yellow;
        GUI.Label(new Rect(30, 145, 190, 20), $"MAPA: {fragmentosColetados} / {fragmentosTotais}");
    }
}