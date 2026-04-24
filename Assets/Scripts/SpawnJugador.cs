using UnityEngine;

public class SpawnJugador : MonoBehaviour
{
    public GameObject[] personajes;
    public Transform puntoSpawn;
    public CamaraSeguir camara;

    void Start()
    {
        int personajeSeleccionado = PlayerPrefs.GetInt("PersonajeSeleccionado", 0);

        GameObject jugador = Instantiate(
            personajes[personajeSeleccionado],
            puntoSpawn.position,
            Quaternion.identity
        );

        camara.objetivo = jugador.transform;
    }
}