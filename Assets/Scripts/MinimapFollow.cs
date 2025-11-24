using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;
    public float altura = 100f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 novaPosicao = player.position;
        novaPosicao.y = novaPosicao.y + altura; 
        transform.position = novaPosicao;

        Vector3 rotacaoAtual = transform.eulerAngles;
        rotacaoAtual.y = player.eulerAngles.y;
        rotacaoAtual.x = 90f; 
        transform.eulerAngles = rotacaoAtual;
    }
}