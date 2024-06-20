using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Personaje : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private BoxCollider2D colEspada;
    [SerializeField] private float dashingPower = 24f; // Potencia del dash (velocidad)
    [SerializeField] private float dashingTime = 0.2f; // Duración del dash
    [SerializeField] private float dashingCooldown = 1f; // Tiempo de enfriamiento entre dashes
    [SerializeField] private TrailRenderer tr; // Referencia al componente TrailRenderer utilizado para visualizar el dash

    private float posColX = 3.3F;
    private float posColY = 0;
    private Vector2 direccion;
    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer spritePersonaje;

    private bool canDash = true; // Indica si el jugador puede realizar un dash
    private bool isDashing; // Indica si el jugador está actualmente en estado de dash

    private Vector3 lookDirection; // Dirección hacia la cual mira el personaje
    public AudioClip sonidoEspada;
    public AudioClip sonidoAjo;
    public float fuerzaGolpe;
    private bool muerto = false;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spritePersonaje = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if(GameManager.Instance.SinVida())
        {
            gameObject.tag = "Enemigo";
            anim.SetTrigger("Muerte");
            muerto = true;
            StartCoroutine(CargarEscena());
        }

        if (!muerto)
        {
            if (isDashing) // Si el jugador está actualmente en estado de dash, no se procesa otra entrada
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                MovimientoCamara.Instance.MoverCamara(3, 1, 0.5f);
                AudioManager.Instance.RepodroducirSonido(sonidoEspada);
                anim.SetTrigger("Ataca");

            }

            if (Input.GetMouseButtonDown(1))
            {
                AudioManager.Instance.RepodroducirSonido(sonidoAjo);
                MovimientoCamara.Instance.MoverCamara(1, 1, 0.5f);
                // Obtener la posición del mouse en el mundo
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0f; // Asegurar que la Z sea 0 (el mismo plano que el personaje)

                // Calcular la dirección hacia donde mirar
                lookDirection = (mousePos - transform.position).normalized;

                // Voltear el sprite según la dirección
                if (lookDirection.x >= 0)
                {
                    spritePersonaje.flipX = false;
                }
                else
                {
                    spritePersonaje.flipX = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }
        }
        else
        {
            return;
        }



    }

    private void FixedUpdate()
    {
        if (!muerto)
        {
            if (isDashing) // Si el jugador está en estado de dash, no se aplica movimiento normal
            {
                return;
            }

            Movimiento();
        }
        else
        {
            return;
        }

    }

    private void Movimiento()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        direccion = new Vector2(horizontal, vertical).normalized;

        rig.MovePosition(rig.position + direccion * velocidad * Time.fixedDeltaTime);

        float movementMagnitude = direccion.magnitude;
        anim.SetFloat("Camina", movementMagnitude);

        if (!Input.GetMouseButton(1))
        {
            if (horizontal > 0)
            {
                colEspada.offset = new Vector2(posColX, posColY);
                spritePersonaje.flipX = false;
            }
            else if (horizontal < 0)
            {
                colEspada.offset = new Vector2(-posColX, posColY);
                spritePersonaje.flipX = true;
            }
        }
    }

    private IEnumerator Dash()
    {
        canDash = false; // El jugador no puede realizar un dash mientras está en proceso
        isDashing = true; // El jugador está actualmente en estado de dash

        // Guarda la gravedad original del jugador y la establece en cero durante el dash
        float originalGravity = rig.gravityScale;
        rig.gravityScale = 0f;

        // Aplica una velocidad al jugador para realizar el dash
        rig.velocity = new Vector2(direccion.x * dashingPower, direccion.y * dashingPower);

        // Activa el componente TrailRenderer para visualizar el dash
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime); // Espera durante la duración del dash

        // Desactiva el componente TrailRenderer al finalizar el dash
        tr.emitting = false;

        // Restaura la gravedad original del jugador
        rig.gravityScale = originalGravity;

        isDashing = false; // El jugador ha completado el dash
        yield return new WaitForSeconds(dashingCooldown); // Espera el tiempo de enfriamiento entre dashes
        canDash = true; // El jugador puede realizar otro dash después del tiempo de enfriamiento
    }

        public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Goblin"))
        {
            anim.SetTrigger("Daño");
        }
    }

    private IEnumerator CargarEscena()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
