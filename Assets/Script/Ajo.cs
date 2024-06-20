using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ajo : MonoBehaviour
{
    private BoxCollider2D ajo;
    [SerializeField] private float dañoGolpe;
    private void Awake()
    {
        ajo = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.CompareTag("Enemigo"))
        {
            otro.transform.GetComponent<Goblin>().TomarDaño(dañoGolpe);
            MovimientoCamara.Instance.MoverCamara(1, 1, 0.5f);
            Destroy(gameObject);
        }

    }
}