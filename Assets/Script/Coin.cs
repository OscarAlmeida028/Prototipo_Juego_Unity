using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int valor = 1;
    public AudioClip sonidoMoneda;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.SumarPuntos(valor);
            Destroy(this.gameObject);
            AudioManager.Instance.RepodroducirSonido(sonidoMoneda);
        }
    }
}
