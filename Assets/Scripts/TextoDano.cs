using UnityEngine;

public class TextoDano : MonoBehaviour
{
    public float tempoDeVida = 1f;
    public float velocidadeSubida = 5f;
    
    // Esta função é chamada pelo Inimigo para definir o número
    public void Configurar(float dano)
    {
        // Tenta pegar o componente de texto (pode ser TextMesh ou TextMeshPro)
        TextMesh textMesh = GetComponent<TextMesh>();
        if (textMesh != null)
        {
            textMesh.text = dano.ToString("F0"); // "F0" tira as casas decimais
        }
    }

    void Start()
    {
        // Destrói o texto depois de X segundos para não pesar o jogo
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // 1. Faz o texto subir
        transform.Translate(Vector3.up * velocidadeSubida * Time.deltaTime);

        // 2. Faz o texto olhar sempre para o jogador (Billboard)
        // O truque aqui é fazer ele olhar na direção oposta da câmera invertida, 
        // senão o texto fica espelhado.
        if (Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}