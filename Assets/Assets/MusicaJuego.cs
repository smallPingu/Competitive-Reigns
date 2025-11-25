using UnityEngine;

public class MusicaJuego : MonoBehaviour
{
    void Start(){
      AudioListener.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }
}
