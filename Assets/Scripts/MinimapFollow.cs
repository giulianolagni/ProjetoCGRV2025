using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player; // Arraste sua nave aqui
    public float altura = 100f; // Altura da câmera

    void LateUpdate()
    {
        if (player == null) return;

        // 1. Segue a posição X e Z do player, mas mantém altura fixa
        Vector3 novaPosicao = player.position;
        novaPosicao.y += altura;
        transform.position = novaPosicao;

        // 2. Acompanha a rotação Y da nave (bússola gira), mas trava X e Z
        // Se quiser o mapa estático (Norte sempre pra cima), apague a linha abaixo.
        Vector3 novaRotacao = transform.eulerAngles;
        novaRotacao.y = player.eulerAngles.y; 
        transform.eulerAngles = novaRotacao;
    }
}