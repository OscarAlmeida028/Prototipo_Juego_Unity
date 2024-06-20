using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Espada : MonoBehaviour
{
    private BoxCollider2D colEspada;
    [SerializeField] private float dañoGolpe;
    private void Awake()
    {
        colEspada = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Enemigo"))
        {
            otro.transform.GetComponent<Goblin>().TomarDaño(dañoGolpe);
            MovimientoCamara.Instance.MoverCamara(3, 1, 1f);
        }

    }
}
