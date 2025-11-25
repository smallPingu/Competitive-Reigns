using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    
    void Start()
    {
        // Asignar valor de instancia anterior (1 si no hay nada guardado)
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        
        // Cambiamos el volumen al iniciar
        AudioListener.volume = volumeSlider.value;
        
        // Añadimos un Listener para cuando cambie el slider
        volumeSlider.onValueChanged.AddListener(HandleVolumeChanged);
    }
    
    void HandleVolumeChanged(float value)
    {
        // Actualizar volumen
        AudioListener.volume = value;
        
        // Guardar el valor
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save(); // Importante escribir en disco
    }
    
    void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(HandleVolumeChanged);
    }
}
