using UnityEngine;
using TMPro;

public class GestorPuntuaciones : MonoBehaviour
{
    [Tooltip("Componente TextMeshProUGUI para mostrar la puntuación del Jugador 1.")]
    [SerializeField] private TextMeshProUGUI puntuacionJugador1;

    [Tooltip("Componente TextMeshProUGUI para mostrar la puntuación del Jugador 2.")]
    [SerializeField] private TextMeshProUGUI puntuacionJugador2;

    [Tooltip("Referencia al PersonajeHinchar del Jugador 1 para obtener su escala.")]
    [SerializeField] private PersonajeHinchar jugador1;

    [Tooltip("Referencia al PersonajeHinchar del Jugador 2 para obtener su escala.")]
    [SerializeField] private PersonajeHinchar jugador2;

    [Tooltip("Referencia a BolaHinchar del Jugador 1 para obtener su escala.")]
    [SerializeField] private BolaHinchar bola1;

    [Tooltip("Referencia a BolaHinchar del Jugador 2 para obtener su escala.")]
    [SerializeField] private BolaHinchar bola2;

    private void Start()
    {
        if (puntuacionJugador1 == null || puntuacionJugador2 == null)
        {
            Debug.LogError("¡Los componentes TextMeshProUGUI de puntuación no están asignados en GestorPuntuaciones!");
        }
        if (bola1 == null || bola2 == null)
        {
            Debug.LogError("¡Uno o ambos personajes no están asignados en GestorPuntuaciones!");
        }
    }

    public void ActualizarPuntuaciones()
    {
        if (puntuacionJugador1 != null && jugador1 != null)
        {
           
            puntuacionJugador1.text = "Jugador 1: " + ((jugador1.ObtenerEscalaXZ() + bola1.ObtenerEscalaXZ())*10 - 20).ToString();
        }

        if (puntuacionJugador2 != null && jugador2 != null)
        {
            
            puntuacionJugador2.text = "Jugador 2: " + ((jugador2.ObtenerEscalaXZ() + bola2.ObtenerEscalaXZ())*10 - 20).ToString();;
        }
    }

    
    public void EvaluarGanador()
    {
        float escala1 = 0f;
        float escala2 = 0f;
        if (bola1 != null)
        {
            escala1 = bola1.ObtenerEscalaXZ();
        }

        if (bola2 != null)
        {
            escala2 = bola2.ObtenerEscalaXZ();
        }

        int ganador = 0;

        if (escala1 > escala2)
        {
            ganador = 1; 
        }
        else if (escala2 > escala1)
        {
            ganador = 2;
        }
        else
        {
            ganador = 0; 
        }

        PlayerPrefs.SetInt("ganador", ganador);
        Debug.Log("Juego terminado. Ganador: Jugador " + ganador + ". Puntuaciones: J1=" + escala1.ToString("F2") + ", J2=" + escala2.ToString("F2"));
    }
}
