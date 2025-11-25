using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    public Toggle fullscreenToggle;

    void Start()
    {
        // Inicializar el toggle con el estado actual
        fullscreenToggle.isOn = Screen.fullScreen;
        
        // Añadir listener para cambios
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);

        // Para tener consistencia entre instancias
        Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    private void OnFullscreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        
        // Para tener consistencia entre instancias
        PlayerPrefs.SetInt("Fullscreen", Screen.fullScreen ? 1 : 0);
    }

    void OnDestroy()
    {
        // Limpiar el listener cuando se destruya el objeto
        fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggle);
    }
}