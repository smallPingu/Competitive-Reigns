using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MostrarGanador : MonoBehaviour
{
    [Header("Jugador Rojo")]
    public GameObject bolaRoja;
    public GameObject pequeRoja;

    [Header("Jugador Cyan")]
    public GameObject bolaCyan;
    public GameObject pequeCyan;

    [Header("Textos Ganador (TextMeshPro)")]
    public TextMeshProUGUI ganadorCyan;
    public TextMeshProUGUI ganadorRojo;
    public TextMeshProUGUI ganadorEmpate;

    [Header("Sistema de Partículas")]
    public ParticleSystem sistemaParticulasGanador;

    void Awake()
    {
        bolaRoja.SetActive(false);
        pequeRoja.SetActive(false);
        bolaCyan.SetActive(false);
        pequeCyan.SetActive(false);
        ganadorRojo.gameObject.SetActive(false);
        ganadorCyan.gameObject.SetActive(false);
        ganadorEmpate.gameObject.SetActive(false);
        
        if (sistemaParticulasGanador != null)
        {
            sistemaParticulasGanador.Stop();
            sistemaParticulasGanador.gameObject.SetActive(false);
        }

        if (PlayerPrefs.HasKey("ganador"))
        {
            int winner = PlayerPrefs.GetInt("ganador");
            Debug.Log("El ganador es: " + winner);

            if (winner == 1)
            {
                bolaRoja.SetActive(true);
                pequeCyan.SetActive(true);
                ganadorRojo.gameObject.SetActive(true);
                ganadorEmpate.gameObject.SetActive(false);

                if (sistemaParticulasGanador != null)
                {
                    Vector3 posGanador = bolaRoja.transform.position;
                    sistemaParticulasGanador.transform.position = new Vector3(posGanador.x, sistemaParticulasGanador.transform.position.y, posGanador.z);
                    sistemaParticulasGanador.gameObject.SetActive(true);
                    sistemaParticulasGanador.Play();
                }
            }
            else if (winner == 2)
            {
                pequeRoja.SetActive(true);
                bolaCyan.SetActive(true);
                ganadorCyan.gameObject.SetActive(true);
                ganadorEmpate.gameObject.SetActive(false);

                if (sistemaParticulasGanador != null)
                {
                    Vector3 posGanador = bolaCyan.transform.position;
                    sistemaParticulasGanador.transform.position = new Vector3(posGanador.x, sistemaParticulasGanador.transform.position.y, posGanador.z);
                    sistemaParticulasGanador.gameObject.SetActive(true);
                    sistemaParticulasGanador.Play();
                }
            }
            else if (winner == 0)
            {
                pequeRoja.SetActive(true);
                pequeCyan.SetActive(true);
                ganadorEmpate.gameObject.SetActive(true);

                if (sistemaParticulasGanador != null)
                {
                    sistemaParticulasGanador.Stop();
                    sistemaParticulasGanador.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Ganador ID inesperado: " + winner + ". No se ha establecido la visualización del ganador.");
            }
        }
        else
        {
            Debug.Log("No se encontró la clave 'ganador' en PlayerPrefs. Asegúrate de que el ganador se guarde correctamente.");
        }
    }

    public void OnProceed()
    {
        SceneManager.LoadScene(0);
    }
}