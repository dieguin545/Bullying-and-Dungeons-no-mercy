using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    public Transform objetivo;
    public float velocidad = 5f;

    void LateUpdate()
    {
        if (objetivo == null)
        {
            return;
        }

        Vector3 posicionDeseada = new Vector3(
            objetivo.position.x,
            objetivo.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            posicionDeseada,
            velocidad * Time.deltaTime
        );
    }
}
