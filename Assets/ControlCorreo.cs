using UnityEngine;
using UnityEngine.UI;

public class ControlCorreo : MonoBehaviour
{
    public Text textoCorreo;
    public Text textoResultado;

    // Correos por nivel
    public string[] correosFaciles;
    public string[] correosMedios;
    public string[] correosDificiles;

    public bool[] bullyingFacil;
    public bool[] bullyingMedio;
    public bool[] bullyingDificil;

    // Variables internas
    string[] correosActuales;
    bool[] bullyingActual;

    int indice = 0;
    int dia = 1;

    bool activo = true; // 🔥 controla si se puede seguir jugando

    void Start()
    {
        CargarCorreos();
    }

    void CargarCorreos()
    {
        if (dia == 1)
        {
            correosActuales = correosFaciles;
            bullyingActual = bullyingFacil;
        }
        else if (dia == 2)
        {
            correosActuales = correosMedios;
            bullyingActual = bullyingMedio;
        }
        else
        {
            correosActuales = correosDificiles;
            bullyingActual = bullyingDificil;
        }

        indice = 0;
        activo = true;


        MostrarCorreo();
    }

    void MostrarCorreo()
    {
        if (indice < correosActuales.Length)
        {
            textoCorreo.text = correosActuales[indice];
            textoResultado.text = "";
        }
    }

    public void Evaluar(bool decisionJugador)
    {
        if (!activo) return;

        if (indice >= bullyingActual.Length) return;

        if (decisionJugador == bullyingActual[indice])
        {
            textoResultado.text = "Correcto";
        }
        else
        {
            textoResultado.text = "Incorrecto";
        }

        SiguienteCorreo();
    }

    public void EvaluarReportar()
    {
        Evaluar(true);
    }

    public void EvaluarAceptar()
    {
        Evaluar(false);
    }

    void SiguienteCorreo()
    {
        indice++;

        if (indice < correosActuales.Length)
        {
            MostrarCorreo();
        }
        else
        {
            // 🔥 bloquea interacción
            activo = false;

            textoCorreo.text = "";
            textoResultado.text = "Fin del día";

            Invoke("SiguienteDia", 2f); // espera 2 segundos
        }
    }

    void SiguienteDia()
    {
        dia++;
        CargarCorreos();
    }
}
