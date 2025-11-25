using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CuentaAtras : MonoBehaviour
{
    [Tooltip("Referencia al script GestorPuntuaciones para actualizar y determinar el ganador.")]
    [SerializeField] private GestorPuntuaciones gestorPuntuaciones;

    [Tooltip("Referencia al PersonajeHinchar del Jugador 1 para comprobar su inflación máxima.")]
    [SerializeField] private PersonajeHinchar jugador1;

    [Tooltip("Referencia al PersonajeHinchar del Jugador 2 para comprobar su inflación máxima.")]
    [SerializeField] private PersonajeHinchar jugador2;

    [Tooltip("Referencia a la bola1")]
    [SerializeField] private BolaHinchar bola1;

    [Tooltip("Referencia a la bola2")]
    [SerializeField] private BolaHinchar bola2;

    [Tooltip("Componente TextMeshProUGUI para mostrar el tiempo restante.")]
    [SerializeField] private TextMeshProUGUI textoTiempoRestante; 

    [Tooltip("Tiempo límite de la partida en segundos.")]
    [SerializeField] private float tiempoLimite = 60f;

    private float tiempoRestante; 
    private bool juegoTerminado = false;

    private void Start()
    {
        tiempoRestante = tiempoLimite;
        juegoTerminado = false;
       
        if (gestorPuntuaciones == null)
        {
            Debug.LogError("¡El GestorPuntuaciones no está asignado en CuentaAtras!");
        }
        if (jugador1 == null || jugador2 == null)
        {
            Debug.LogError("¡Uno o ambos personajes no están asignados en CuentaAtras!");
        }
        if (textoTiempoRestante == null) 
        {
            Debug.LogError("¡El TextMeshProUGUI para el tiempo restante no está asignado en CuentaAtras!");
        }

        ActualizarTextoTiempo(); 
    }

    private void Update()
    {
        if (juegoTerminado) return; 

        tiempoRestante -= Time.deltaTime;
        ActualizarTextoTiempo();

        if (gestorPuntuaciones != null)
        {
            gestorPuntuaciones.ActualizarPuntuaciones();
        }

       
        bool jugador1Max = false;
        if (bola1 != null)
        {
            jugador1Max = bola1.ObtenerInflacionActual() >= bola1.ObtenerInflacionMaxima();
        }

        bool jugador2Max = false;
        if (bola2 != null)
        {
            jugador2Max = bola2.ObtenerInflacionActual() >= bola2.ObtenerInflacionMaxima();
        }

        
        if (tiempoRestante <= 0f || jugador1Max || jugador2Max)
        {
            FinDeJuego();
        }
    }

    private void ActualizarTextoTiempo()
    {
        if (textoTiempoRestante != null)
        {
            
            float tiempoAMostrar = Mathf.Max(0f, tiempoRestante);
            
            textoTiempoRestante.text = "Tiempo: " + tiempoAMostrar.ToString("F2");
        }
    }

    
    private void FinDeJuego()
    {
        juegoTerminado = true; 

        
        if (textoTiempoRestante != null)
        {
            textoTiempoRestante.text = "Tiempo: 0.00";
        }

        if (gestorPuntuaciones != null)
        {
            
            gestorPuntuaciones.EvaluarGanador();
        }
        
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
    public float ObtenerTiempoRestante()
    {
        return tiempoRestante;
    }
}
