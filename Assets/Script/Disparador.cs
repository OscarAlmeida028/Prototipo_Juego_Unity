using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparador : MonoBehaviour
{
    public Transform gun;
    public int speedball;
    Vector3 targetRotation;
    public GameObject ball;
    Vector3 finalTarget;
    private bool muerto = false;

    private void Update()
    {
        if(GameManager.Instance.SinVida())
        {
            muerto = true;
        }

        if(!muerto)
        {
            // Calcula la rotación del arma basada en la posición del ratón
            targetRotation = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(targetRotation.y, targetRotation.x) * Mathf.Rad2Deg;
            gun.rotation = Quaternion.Euler(0, 0, angle);

            // Detecta si se presiona el botón del ratón para disparar
            if (Input.GetMouseButtonDown(1))
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // Instancia la bola en la posición del arma con la rotación adecuada
        var Ball = Instantiate(ball, gun.position, gun.rotation, transform.parent);
        targetRotation.z = 0;
        finalTarget = (targetRotation - transform.position).normalized;
        Ball.GetComponent<Rigidbody2D>().AddForce(finalTarget * speedball, ForceMode2D.Impulse);
        Destroy(Ball, 2f);
    }
}
