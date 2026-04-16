using UnityEngine;

public class SpaceShooter : MonoBehaviour
{
    public GameObject Juego;
    void Start()
    {
        Juego.SetActive(false);
    }
    public void abrir()
    {
        if (Juego.activeSelf)
        {
            Juego.SetActive(false);
        }
        else
        {
            Juego.SetActive(true);
        }
    }

        
}
