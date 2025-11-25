using UnityEngine;

public class BolaHinchar : MonoBehaviour
{
    [Header("Configuración de Inflado")]
    [SerializeField] private float inflationAmount = 0.2f;
    [SerializeField] private float maxInflation = 20f;
    [SerializeField] private float minScale = 1f;
    [SerializeField] private string foodTag = "Comida";
    [SerializeField] private string foodTag2 = "ComidaGrande";

    [Header("Configuración de Velocidad")]
    [SerializeField] private float speedReductionPerFood = 0.5f;
    [SerializeField] private float minSpeed = 1f;

    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoComer;
    [SerializeField] private AudioSource audioSource;


    private Vector3 originalScale;
    private float currentInflation;
    private Collider playerCollider;
    private MovementStateManager movementManager;
    private float originalSpeed;

    private void Start()
    {
        originalScale = transform.localScale;
        playerCollider = GetComponent<Collider>();
        movementManager = GetComponent<MovementStateManager>();

        if (movementManager != null)
        {
            originalSpeed = movementManager.velocidadMovimiento;
        }

        if (playerCollider == null)
        {
            Debug.LogWarning("No se encontró un Collider en el jugador.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool smallCompare = collision.gameObject.CompareTag(foodTag);
        bool bigCompare = collision.gameObject.CompareTag(foodTag2);
        if (smallCompare || bigCompare)
        {
            if(smallCompare){
                InflatePlayer("small");
                ReproducirSonido(sonidoComer);
            }else{
                InflatePlayer("big");
            }
            ReduceSpeed();
            Destroy(collision.gameObject);
        }
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void InflatePlayer(string size)
    {
        int veces = 1;
        if(size == "big")
            veces = 2;

        currentInflation = Mathf.Min(currentInflation + inflationAmount * veces, maxInflation);
        float newScale = minScale + currentInflation;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void ReduceSpeed()
    {
        if (movementManager != null)
        {
            // Reducir velocidad pero no por debajo del mínimo
            movementManager.velocidadMovimiento = Mathf.Max(
                originalSpeed - (currentInflation * speedReductionPerFood),
                minSpeed
            );
        }
    }

    // Método para resetear (opcional)
    public void ResetInflation()
    {
        currentInflation = 0f;
        transform.localScale = originalScale;
        if (movementManager != null)
        {
            movementManager.velocidadMovimiento = originalSpeed;
        }
    }

    
    public float ObtenerInflacionMaxima()
    {
        return maxInflation;
    }

    public float ObtenerInflacionActual()
    {
        return currentInflation;
    }

    public float ObtenerEscalaXZ()
    {
        return minScale + currentInflation;
    }
}