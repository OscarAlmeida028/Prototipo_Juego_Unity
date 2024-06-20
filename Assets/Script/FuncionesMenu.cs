using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuncionesMenu : MonoBehaviour
{
    public void EmpezarNivel(string NombreNivel)
    {
        SceneManager.LoadScene(NombreNivel);
    }
}
