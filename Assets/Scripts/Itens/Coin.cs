using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool coletado = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (coletado)
        {
            Destroy(this);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Moeda tocou" + other);

        // Verifica se o objeto colidido está na layer "Moeda"
        if (other.gameObject.layer == LayerMask.NameToLayer("Jogador"))
        {
            // Destroi a moeda
            Destroy(transform.gameObject);

        }
    }
}