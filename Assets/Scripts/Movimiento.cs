using UnityEngine;

public class Movimiento : MonoBehaviour
{
   public float velocidad = 10f;

    void Update()
    {
        float movimientoX = 0f;
        float movimientoY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            movimientoY = 20f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movimientoY = -20f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movimientoX = -20f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movimientoX = 20f;
        }

        Vector3 movimiento = new Vector3(movimientoX, movimientoY, 0f);
        transform.Translate(movimiento * velocidad * Time.deltaTime);
    }
}
