using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public int PuntosTotales{get; private set;}
    public HUD hud;
    private int vidas = 3;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Más de un game manager");
        }
    }

    public void SumarPuntos(int puntosASumar)
    {
        PuntosTotales += puntosASumar;
        if(PuntosTotales == 5)
        {
            SceneManager.LoadScene(0);
        }
        hud.ActualizarPuntos(PuntosTotales);
    }

    public void PerderVida()
    {
        if(!SinVida())
        {
            vidas -=1;
            hud.DesactivarVida(vidas);
        }
    }

        public bool RecuperarVida()
    {
        if(vidas == 3)
        {
            return false;
        }
        hud.ActivarVida(vidas);
        vidas +=1;
        return true;
    }

    public bool SinVida()
    {
        if(vidas == 0)
        {
            return true;
        }
        return false;
    }
}
