using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class Correo
{
    public string remitente;
    public string asunto;

    [TextArea(3, 10)]
    public string texto;

    public bool esBullying;

}

[System.Serializable]
public class DatosPartida
{
    public int dineroTotal;
    public int diasTrabajados;
    public int ultimoSueldo;
    public int ultimoDia;
    public int errores;
    public int correctos;
    public int correosClasificados;
}

public class ControlCorreo : MonoBehaviour
{
    public Text textoRemitente;
    public Text textoAsunto;
    public Text textoCorreo;
    public Text textoResultado;
    public Text textoDia;
    public Text textoErrores;

    public GameObject panelAviso;
    public Text textoAviso;

    public GameObject panelFinDia;
    public CanvasGroup canvasFinDia;
    public Text textoTituloFinDia;
    public Text textoResumenFinDia;

    public List<Correo> correosFaciles;
    public List<Correo> correosMedios;
    public List<Correo> correosDificiles;

    List<Correo> correosActuales;

    int indice = 0;
    int dia = 1;
    int errores = 0;
    int correctos = 0;
    int correosClasificados = 0;
    int sueldoBase = 100;
    bool activo = true;
    string rutaArchivo = "";

    bool modoLecturaActivado = false;

    int tamRemitenteNormal;
    int tamAsuntoNormal;
    int tamCorreoNormal;
    int tamResultadoNormal;
    int tamDiaNormal;
    int tamErroresNormal;

    public int dineroTotal = 0;
    public int diasTrabajados = 0;
    public int ultimoSueldo = 0;

    public bool tieneCafe = false;
    public bool tieneSeguro = false;
    public bool seguroUsado = false;
    public bool tieneFiltroSpam = false;
    public bool tieneTeclado = false;

    void Start()
    {
         panelAviso.SetActive(false);
         panelFinDia.SetActive(false);

         rutaArchivo = Path.Combine(Application.persistentDataPath, "partida_bullying.json");

         CargarArchivo();
         CargarCorreos();
         ActualizarErrores();
         GuardarTamanosNormales();



    }

    void CargarCorreos()
    {
        if (dia == 1)
        {
            correosActuales = new List<Correo>(correosFaciles);
        }
        else if (dia == 2)
        {
            correosActuales = new List<Correo>(correosMedios);
        }
        else
        {
            correosActuales = new List<Correo>(correosDificiles);
        }

        MezclarCorreos();

        indice = 0;
        correosClasificados = 0;
        activo = true;

        textoDia.text = "Día " + dia;

        MostrarCorreo();
    }

    void MezclarCorreos()
    {
        for (int i = 0; i < correosActuales.Count; i++)
        {
            Correo temporal = correosActuales[i];
            int random = Random.Range(i, correosActuales.Count);

            correosActuales[i] = correosActuales[random];
            correosActuales[random] = temporal;
        }
    }

    void MostrarCorreo()
    {
        if (indice < correosActuales.Count)
        {
            string remitente = correosActuales[indice].remitente;

            if (tieneFiltroSpam)
            {
                if (!remitente.Contains("@uninorte.edu.co"))
                {
                    remitente += " [SOSPECHOSO]";
                }
            }

            textoRemitente.text = remitente;
            textoAsunto.text = correosActuales[indice].asunto;
            textoCorreo.text = correosActuales[indice].texto;
            textoResultado.text = "";
        }
    }

    public void Evaluar(bool decisionJugador)
    {
        if (!activo)
        {
            return;
        }

        if (indice >= correosActuales.Count)
        {
            return;
        }

        if (decisionJugador == correosActuales[indice].esBullying)
        {
            textoResultado.text = "Correcto";
            correctos++;
        }
        else
        {
            if (tieneSeguro && !seguroUsado)
            {
                seguroUsado = true;
                textoResultado.text = "Seguro utilizado";
                MostrarAviso("El seguro evitó un error");
                correctos++;
                correosClasificados++;

                float tiempoEsperaSeguro = 0.5f;

                if (tieneTeclado)
                {
                    tiempoEsperaSeguro = 0.2f;
                }

                Invoke("SiguienteCorreo", tiempoEsperaSeguro);
                return;
            }

            textoResultado.text = "Incorrecto";
            errores++;
            ActualizarErrores();
            MostrarAviso("Clasificaste mal este correo");

            int limiteErrores = 10;

            if (tieneCafe)
            {
                limiteErrores = 11;
            }

            if (errores >= limiteErrores)
            {
                activo = false;
                CancelInvoke("SiguienteCorreo");
                Invoke("MostrarPantallaFinDia", 1f);
                return;
            }
        }

        correosClasificados++;

        float tiempoEspera = 0.5f;

        if (tieneTeclado)
        {
            tiempoEspera = 0.2f;
        }

        Invoke("SiguienteCorreo", tiempoEspera);
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

        if (correosClasificados >= 15)
        {
            activo = false;
            textoRemitente.text = "";
            textoAsunto.text = "";
            textoCorreo.text = "";
            textoResultado.text = "";

            Invoke("MostrarPantallaFinDia", 1f);
            return;
        }

        if (indice < correosActuales.Count)
        {
            MostrarCorreo();
        }
        else
        {
            indice = 0;
            MezclarCorreos();
            MostrarCorreo();
        }
    }

    void MostrarPantallaFinDia()
    {
        panelFinDia.SetActive(true);

        int sueldoFinal;
        string mensajeSueldo;

        int limiteErrores = 10;

        if (tieneCafe)
        {
            limiteErrores = 11;
        }

        if (errores >= limiteErrores)
        {
            sueldoFinal = 0;
            mensajeSueldo = "Perdiste todo el sueldo por llegar al límite de errores.";
        }
        else
        {
            int descuentos = errores / 2;
            sueldoFinal = sueldoBase - (descuentos * 10);
            mensajeSueldo = "Descuentos aplicados: " + descuentos;
        }

        ultimoSueldo = sueldoFinal;
        dineroTotal += sueldoFinal;
        diasTrabajados++;

        GuardarArchivo();

        textoTituloFinDia.text = "Día " + dia + " terminado";

        textoResumenFinDia.text =
            "Correos clasificados: " + correosClasificados +
            "\nCorreos correctos: " + correctos +
            "\nCorreos incorrectos: " + errores +
            "\nSueldo base: $" + sueldoBase +
            "\nSueldo final: $" + sueldoFinal +
            "\n" + mensajeSueldo;

        StartCoroutine(FadeFinDia());
    }

    IEnumerator FadeFinDia()
    {
        canvasFinDia.alpha = 0;

        while (canvasFinDia.alpha < 1)
        {
            canvasFinDia.alpha += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(7f);

        while (canvasFinDia.alpha > 0)
        {
            canvasFinDia.alpha -= Time.deltaTime;
            yield return null;
        }

        panelFinDia.SetActive(false);

        tieneCafe = false;
        tieneSeguro = false;
        seguroUsado = false;

        dia++;
        errores = 0;
        correctos = 0;
        correosClasificados = 0;

        ActualizarErrores();
        CargarCorreos();
    }

    void ActualizarErrores()
    {
        textoErrores.text = "Errores: " + errores;
    }

    void MostrarAviso(string mensaje)
    {
        textoAviso.text = mensaje;
        panelAviso.SetActive(true);

        CancelInvoke("OcultarAviso");
        Invoke("OcultarAviso", 2f);
    }

    void OcultarAviso()
    {
        panelAviso.SetActive(false);
    }

    void GuardarArchivo()
    {
        DatosPartida datos = new DatosPartida();

        datos.dineroTotal = dineroTotal;
        datos.diasTrabajados = diasTrabajados;
        datos.ultimoSueldo = ultimoSueldo;
        datos.ultimoDia = dia;
        datos.errores = errores;
        datos.correctos = correctos;
        datos.correosClasificados = correosClasificados;

        string json = JsonUtility.ToJson(datos, true);
        File.WriteAllText(rutaArchivo, json);

        Debug.Log("Partida guardada en: " + rutaArchivo);
    }

    void CargarArchivo()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            DatosPartida datos = JsonUtility.FromJson<DatosPartida>(json);

            dineroTotal = datos.dineroTotal;
            diasTrabajados = datos.diasTrabajados;
            ultimoSueldo = datos.ultimoSueldo;

            Debug.Log("Partida cargada desde: " + rutaArchivo);
        }
        else
        {
            Debug.Log("No existe archivo guardado todavía");
        }
    }
    void GuardarTamanosNormales()
    {
        tamRemitenteNormal = textoRemitente.fontSize;
        tamAsuntoNormal = textoAsunto.fontSize;
        tamCorreoNormal = textoCorreo.fontSize;
        tamResultadoNormal = textoResultado.fontSize;
        tamDiaNormal = textoDia.fontSize;
        tamErroresNormal = textoErrores.fontSize;
    }

    public void AlternarModoLectura()
    {
        modoLecturaActivado = !modoLecturaActivado;

        if (modoLecturaActivado)
        {
            textoRemitente.fontSize = 24;
            textoAsunto.fontSize = 24;
            textoCorreo.fontSize = 30;
            textoResultado.fontSize = 22;
            textoDia.fontSize = 22;
            textoErrores.fontSize = 22;

            MostrarAviso("Modo lectura activado");
        }
        else
        {
            textoRemitente.fontSize = tamRemitenteNormal;
            textoAsunto.fontSize = tamAsuntoNormal;
            textoCorreo.fontSize = tamCorreoNormal;
            textoResultado.fontSize = tamResultadoNormal;
            textoDia.fontSize = tamDiaNormal;
            textoErrores.fontSize = tamErroresNormal;

            MostrarAviso("Modo lectura desactivado");
        }
    }

}