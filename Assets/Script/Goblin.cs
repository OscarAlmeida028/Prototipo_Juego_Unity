using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : MonoBehaviour
{
    [SerializeField] private float vida;
    public Transform personaje;
    public Transform [] puntosRuta;
    private int indiceRuta;
    private NavMeshAgent agente;
    private bool objetivoDetectado;
    private SpriteRenderer sprite;
    private Transform objetivo;
    private Animator anim;
    private bool isDead = false;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        agente = GetComponent<NavMeshAgent>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
    }

    private void Start()
    {
        agente.updateRotation = false;
        agente.updateUpAxis = false;
    }

    private void Update()
    {
        if (isDead) return;
        this.transform.position = new Vector3(transform.position.x, transform.position.y,0);
        float distancia = Vector3.Distance(personaje.position, this.transform.position);
        
        if(this.transform.position == puntosRuta[indiceRuta].position)
        {
            if(indiceRuta < puntosRuta.Length -1)
            {
                indiceRuta++; 
            }
            else if(indiceRuta == puntosRuta.Length -1)
            {
                indiceRuta =0;
            }
        }
        
        if (distancia < 20)
        {
            objetivoDetectado = true;
        }


        MovimientoGoblin(objetivoDetectado);
        RotarGoblin();
    }

    void MovimientoGoblin(bool esDetectado)
    {
        if(esDetectado)
        {
            agente.SetDestination(personaje.position);
            objetivo = personaje;
        }
        else{
            agente.SetDestination(puntosRuta[indiceRuta].position);
            objetivo = puntosRuta[indiceRuta];
        }
    }

    void RotarGoblin()
    {
        if(this.transform.position.x >objetivo.position.x)
        {
            transform.localScale = new Vector2(-1,1);
        }
        else
        {
            transform.localScale = new Vector2(1,1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                anim.SetTrigger("Ataca");
            }
        }else
        {
            return;
        }
    }

    public void TomarDaño(float daño)
    {
        anim.SetTrigger("Daño");
        vida -= daño;
        if(vida <= 0)
        {
            Muerte();
        }
    }

    public void Muerte()
    {
        anim.SetTrigger("Muerte");
        isDead = true;
        boxCollider.enabled = false;
        agente.enabled = false;
        Destroy(gameObject, 2f);
    }
}
