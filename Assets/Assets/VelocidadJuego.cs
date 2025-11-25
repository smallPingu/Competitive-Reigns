using UnityEngine;
using UnityEngine.UI;

public class VelocidadJuego : MonoBehaviour
{
    public Slider velocidadSlider;
    
    void Start()
    {
        Time.timeScale = PlayerPrefs.GetFloat("GameSpeed", 1f);

        velocidadSlider.value = PlayerPrefs.GetFloat("GameSpeed", 1f);

        // Añadimos un Listener para cuando cambie el slider
        velocidadSlider.onValueChanged.AddListener(HandleVelocity);
    }

    void HandleVelocity(float value)
    {
        Time.timeScale = value;

        // Guardar el valor
        PlayerPrefs.SetFloat("GameSpeed", value);
        PlayerPrefs.Save(); // Importante escribir en disco
    }


    void OnDestroy()
    {
        velocidadSlider.onValueChanged.RemoveListener(HandleVelocity);
    }
}
