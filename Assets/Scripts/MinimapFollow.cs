using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player; // Arraste sua nave aqui
    public float altura = 100f; // Altura da câmera

    void LateUpdate()
    {
        if (player == null) return;

        // 1. Acompanha a Posição X e Z (Ignora Y do player, mantém altura fixa)
        Vector3 novaPosicao = player.position;
        novaPosicao.y = novaPosicao.y + altura; // A câmera fica sempre X metros acima da nave
        transform.position = novaPosicao;

        // 2. Acompanha a ROTAÇÃO Y da nave
        // Isso faz o mapa girar. O "Norte" do mapa será a frente da nave.
        Vector3 rotacaoAtual = transform.eulerAngles;
        rotacaoAtual.y = player.eulerAngles.y;
        rotacaoAtual.x = 90f; // Garante que olha pra baixo
        transform.eulerAngles = rotacaoAtual;
    }
}