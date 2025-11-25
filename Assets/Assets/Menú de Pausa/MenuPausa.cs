using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    private bool pausaActiva = false;
    public GameObject menuPausaUI;
    public GameObject botonPausaUI;
    public void Pausa()
    {
        if (!pausaActiva)
        {
            Time.timeScale = 0f;
            pausaActiva = true;
            AudioListener.volume = PlayerPrefs.GetFloat("MusicVolume", 1f) * 0.25f;
            menuPausaUI.SetActive(true);
            botonPausaUI.SetActive(false);
        }
        else
        {
            Time.timeScale = PlayerPrefs.GetFloat("GameSpeed", 1f);
            pausaActiva = false;
            AudioListener.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            menuPausaUI.SetActive(false);
            botonPausaUI.SetActive(true);
        }
    }

    public void SalirMenuPrincipal(){        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Pausa();
    }
}
