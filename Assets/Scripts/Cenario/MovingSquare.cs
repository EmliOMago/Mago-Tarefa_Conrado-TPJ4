using UnityEngine;
using System.Collections.Generic;

public class MovingSquare : MonoBehaviour
{
    public float amplitude = 2.0f;
    public float speed = 1.0f;
    public bool vertical = true;
    public int moedas = 0;

    [Header("Configurações de Movimento Conjunto")]
    public bool movimentoConjuntoAtivo = true;

    private Vector3 initialPosition;
    private float movementOffset;
    private List<Transform> objetosGrudados = new List<Transform>();
    private Vector3 posicaoAnterior;
    [HideInInspector]public Vector3 novaPosicao;

    void Start()
    {
        initialPosition = transform.position;
        posicaoAnterior = transform.position;
    }
    
    void Update()
    {
        // Calcula o movimento
        movementOffset = Mathf.Sin(Time.time * speed) * amplitude;

        ;
        if (vertical)
        {
            novaPosicao = initialPosition + Vector3.up * movementOffset;
        }
        else
        {
            novaPosicao = initialPosition + Vector3.right * movementOffset;
        }

        // Move este objeto
        transform.position = novaPosicao;

        // Move os objetos grudados manualmente
        if (movimentoConjuntoAtivo && objetosGrudados.Count > 0)
        {
            MoverObjetosGrudados();
        }

    }
    void LateUpdate()
    {
        // Atualiza a posição anterior
        posicaoAnterior = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Moeda"))
        {
            moedas++;
            Destroy(other.gameObject);
            Debug.Log("Moedas coletadas: " + moedas);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (movimentoConjuntoAtivo)
        {
            Transform objeto = collision.transform;

            // Verifica se não é já um objeto grudado e se tem Rigidbody2D
            Rigidbody2D rb = objeto.GetComponent<Rigidbody2D>();
            if (rb != null && !objetosGrudados.Contains(objeto))
            {
                objetosGrudados.Add(objeto);

                // Opcional: Desativa a física para movimento preciso
                rb.simulated = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (movimentoConjuntoAtivo)
        {
            Transform objeto = collision.transform;
            if (objetosGrudados.Contains(objeto))
            {
                // Reativa a física ao sair do contato
                Rigidbody2D rb = objeto.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.simulated = true;
                }

                objetosGrudados.Remove(objeto);
            }
        }
    }

    private void MoverObjetosGrudados()
    {
        Vector3 movimentoAtual = 2 * (transform.position - posicaoAnterior);

        foreach (Transform objeto in objetosGrudados)
        {
            if (objeto != null)
            {
                // Move o objeto exatamente na mesma quantidade
                objeto.position += movimentoAtual;
            }
        }

        // Limpa objetos nulos
        objetosGrudados.RemoveAll(obj => obj == null);
    }

    void OnDestroy()
    {
        // Garante que todos os objetos tenham física reativada
        foreach (Transform objeto in objetosGrudados)
        {
            if (objeto != null)
            {
                Rigidbody2D rb = objeto.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.simulated = true;
                }
            }
        }
    }
}