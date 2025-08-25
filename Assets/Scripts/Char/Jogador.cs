using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class Jogador : MonoBehaviour
    {
        [HideInInspector] public int moedas;
        public float jVelocidade = 10;
        public float jVelocidadeMaxima = 10;
        public float forcaPulo = 150;
        public float jVelocidadeMaximaPulo = 10;

        public Transform groundChecker;

        [HideInInspector] public float jHorizontal;
        [HideInInspector] public float movimento;
        [HideInInspector] public bool pulo = false;

        [HideInInspector] public bool podePular = true;
        [HideInInspector] public bool morto = false;
        Rigidbody2D rigidB;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            moedas = 0;
            rigidB = transform.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            movimento = Input.GetAxisRaw("Horizontal") * jVelocidade;
            pulo = Input.GetAxis("Jump") != 0;
        }

        // Update is called once per frame
        void Update()
        {
            morto = Physics2D.OverlapBox
                (
                groundChecker.position,
                groundChecker.GetComponent<BoxCollider2D>().bounds.size,
                0,
                LayerMask.GetMask("Morte")
                );

            Debug.Log(morto);
            Debug.Log(podePular);

            // Logica da movimentação
            float movimeto = Input.GetAxisRaw("Horizontal") * Time.deltaTime * jVelocidade;
            transform.position += new Vector3(movimeto, 0);

            // Logica do pulo
            bool pulo = Input.GetButtonDown("Jump");

            if (pulo && podePular)
            {
                rigidB.AddForce(new Vector2(0, forcaPulo), ForceMode2D.Impulse);
                podePular = false;
            }

            // Forma 2 de pulo: verificando sobreposição de colissões
            podePular = Physics2D.OverlapBox
                (
                groundChecker.position,
                groundChecker.GetComponent<BoxCollider2D>().bounds.size,
                0,
                LayerMask.GetMask("Ground")
                );

            // verificação de morte
            if (morto)
            {
                transform.position = new Vector2(0, 0);
                morto = false;
            }


        }
        void OnTriggerEnter2D(Collider2D other)
        {
            // Verifica se o objeto colidido está na layer "Moeda"
            if (other.gameObject.layer == LayerMask.NameToLayer("Moeda"))
            {
                // Incrementa o contador de moedas
                moedas++;

                // Destroi a moeda
                Destroy(other.gameObject);

                Debug.Log("Moedas coletadas: " + moedas);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Morto"))
            {

                Debug.Log("Você morreu com" + moedas + " Moedas coletadas");
                morto = true;
                // Reseta o contador de moedas
                moedas = 0;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Final"))
            {
                Debug.Log("Você venceu com" + moedas + " Moedas coletadas");
                // Reseta o contador de moedas
                moedas = 0;
            }
        }
    }
}