using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    // peguei um script com o chat gpt ele fez em portugues wtf kk

    public SpriteRenderer spriteRenderer;
    public Sprite[] framesDaAnimacao;
    public float velocidadeDaAnimacao = 1.0f;

    private int frameAtual = 0;
    private float tempoDecorrido = 0.0f;

    void Update()
    {
        tempoDecorrido += Time.deltaTime;

        if (tempoDecorrido >= 1.0f / velocidadeDaAnimacao)
        {
            tempoDecorrido = 0.0f;
            frameAtual = (frameAtual + 1) % framesDaAnimacao.Length;
            spriteRenderer.sprite = framesDaAnimacao[frameAtual];
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = framesDaAnimacao[0];
    }

}
