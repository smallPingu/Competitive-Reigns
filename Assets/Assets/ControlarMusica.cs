using UnityEngine;

public class ControlarMusica : MonoBehaviour
{
    [Header("Canción Inicio")]
    public AudioSource audioSource1;
    [Header("Tambores En Loop")]
    public AudioSource audioSource2;

    private bool introPlayed = false;

    private void Awake()
    {
        audioSource1.Stop();
        audioSource2.Stop();

        // Primera Canción (intro)
        audioSource1.Play();
    }

    private void Update()
    {
        if (audioSource1.isPlaying && !introPlayed)
        {
            if (audioSource1.time >= audioSource1.clip.length - 0.05f) 
            {
                PlaySecondSong();
                introPlayed = true;
            }
        }
    }

    private void PlaySecondSong()
    {
        audioSource1.Stop();

        // Que se repetirá en bucle
        audioSource2.loop = true;
        audioSource2.Play();
    }
}