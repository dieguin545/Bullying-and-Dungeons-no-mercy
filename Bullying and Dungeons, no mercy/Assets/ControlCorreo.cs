using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Correo
{
    public string texto;
    public bool esBullying;
}

public class ControlCorreo : MonoBehaviour
{
    public Text textoCorreo;
    public Text textoResultado;
    public Text textoDia;

    // Listas por nivel
    public List<Correo> correosFaciles;
    public List<Correo> correosMedios;
    public List<Correo> correosDificiles;

    // Lista actual
    List<Correo> correosActuales;

    int indice = 0;
    int dia = 1;
    bool activo = true;

    void Start()
    {
        CargarCorreos();
    }

    void CargarCorreos()
    {
        if (dia == 1)
        {
            correosActuales = correosFaciles;
        }
        else if (dia == 2)
        {
            correosActuales = correosMedios;
        }
        else
        {
            correosActuales = correosDificiles;
        }

        indice = 0;
        activo = true;

        textoDia.text = "Día " + dia;

        MostrarCorreo();
    }

    void MostrarCorreo()
    {
        if (indice < correosActuales.Count)
        {
            textoCorreo.text = correosActuales[indice].texto;
            textoResultado.text = "";
        }
    }

    public void Evaluar(bool decisionJugador)
{
    Debug.Log("Lista: " + (correosActuales == null));
Debug.Log("TextoResultado: " + (textoResultado == null));
Debug.Log("Indice: " + indice);
    Debug.Log("Evaluando...");
    
    if (!activo)
    {
        Debug.Log("NO activo");
        return;
    }

    if (indice >= correosActuales.Count)
    {
        Debug.Log("Indice fuera");
        return;
    }

    if (decisionJugador == correosActuales[indice].esBullying)
    {
        textoResultado.text = "Correcto";
    }
    else
    {
        textoResultado.text = "Incorrecto";
    }

    Debug.Log("Resultado: " + textoResultado.text);

    SiguienteCorreo();
}

    public void EvaluarReportar()
    {
        Evaluar(true);
        Debug.Log("CLICK REPORTAR");
        Debug.Log("Activo: " + activo);
        Debug.Log("Indice: " + indice);
Debug.Log("Cantidad: " + correosActuales.Count);
    }

    public void EvaluarAceptar()
    {
        Evaluar(false);
        Debug.Log("CLICK ACEPTAR");
        Debug.Log("Activo: " + activo);
        Debug.Log("Indice: " + indice);
Debug.Log("Cantidad: " + correosActuales.Count);
    }

    void SiguienteCorreo()
    {
        indice++;

        if (indice < correosActuales.Count)
        {
            MostrarCorreo();
        }
        else
        {
            activo = false;
            textoCorreo.text = "";
            textoResultado.text = "Fin del día";

            Invoke("SiguienteDia", 2f);
        }
    }

    void SiguienteDia()
    {
        dia++;
        CargarCorreos();
    }
}
