using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class Jogador : MonoBehaviour
    {
        [HideInInspector] public int moedas = 0;
        public float jVelocidade = 2;
        public float jVelocidadeMaxima = 2;
        public float forcaPulo = 7;
        public float jVelocidadeMaximaPulo = 10;
        public bool jdireita;
        public Transform groundChecker;
        public Transform projetil;

        [HideInInspector] public float movimento;
        [HideInInspector] public bool pulo = false;

        [HideInInspector] public bool podePular = true;
        [HideInInspector] public bool morto = false;
        Rigidbody2D rigidB;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rigidB = transform.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.Log("está morto " + morto);
            Debug.Log("pode pular " + podePular);
            Debug.Log("Você tem " + moedas + " moedas");
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

            // Logica da movimentação
            movimento = Input.GetAxisRaw("Horizontal") * Time.deltaTime * jVelocidade;
            transform.position += new Vector3(movimento, 0);

            if (movimento > 0)
            {
                transform.eulerAngles = new Vector2(0, 0);
                jdireita = true;
            }

            if (movimento < 0)
            {
                transform.eulerAngles = new Vector2(0, 180);
                jdireita = false;
            }

            //Logica do tiro
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Transform instanciado = Instantiate(projetil);
                instanciado.position = transform.position;
                instanciado.GetComponent<Projetil>().direcao = new Vector2 (jdireita ? 1: -1, 0);
                instanciado.GetComponent<Projetil>().enabled = true;
            }


            // Logica do pulo
            pulo = Input.GetButtonDown("Jump");

            // Forma 2 de pulo: verificando sobreposição de colissões
            podePular = Physics2D.OverlapBox
                (
                groundChecker.position,
                groundChecker.GetComponent<BoxCollider2D>().bounds.size,
                0,
                LayerMask.GetMask("Ground")
                );

            if (pulo && podePular)
            {
                rigidB.AddForce(new Vector2(0, forcaPulo), ForceMode2D.Impulse);
                podePular = false;
            }

            

            // verificação de morte
            if (morto)
            {
                transform.position = new Vector2(0, 0);
                morto = false;
            }
        }
        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Jogador tocou em " + other);

            // Verifica se o objeto colidido está na layer "Moeda"
            if (other.gameObject.layer == LayerMask.NameToLayer("Moedas"))
            {
                // Incrementa o contador de moedas
                moedas++;

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
            }
        }
    }
}