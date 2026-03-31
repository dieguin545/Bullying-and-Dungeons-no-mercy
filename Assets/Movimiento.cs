using UnityEngine;

public class Movimiento : MonoBehaviour
{
   public float velocidad = 5f;

    void Update()
    {
        float movimientoX = 0f;
        float movimientoY = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            movimientoY = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movimientoY = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movimientoX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movimientoX = 1f;
        }

        Vector3 movimiento = new Vector3(movimientoX, movimientoY, 0f);
        transform.Translate(movimiento * velocidad * Time.deltaTime);
    }
}
