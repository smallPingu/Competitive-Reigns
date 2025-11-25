using UnityEngine;
// For Cinemachine, uncomment this if you're using it
// using Cinemachine;

public class PersonajeHinchar : MonoBehaviour
{
    [Header("Configuración de Inflado")]
    [SerializeField] private float inflationAmount = 0.1f;
    [SerializeField] private float maxInflation = 3f; // This is the scale threshold for the swap
    [SerializeField] private float minScale = 1f; // Base scale for calculation
    [SerializeField] private string foodTag = "Comida";
    [SerializeField] private string foodTag2 = "ComidaGrande";

    [Header("Configuración de Velocidad")]
    [SerializeField] private float speedReductionPerFood = 0.5f;
    [SerializeField] private float minSpeed = 1f;

    [SerializeField] private GameObject modeloGrande;   // Arrastra tu modelo grande (inicialmente inactivo) aquí
    private bool modeloCambiado = false; // Bandera para asegurar que el cambio ocurre una vez

    [Header("Configuración de Cámara")]
    [SerializeField] private Camera mainCamera;
    private CamaraSiguePlayer camaraSiguePlayerScript;

    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoComer;
    [SerializeField] private AudioClip sonidoGolpeJugador;
    [SerializeField] private AudioClip sonidoBola;
    [SerializeField] private AudioSource audioSource;


    private Vector3 originalScale;
    private float currentInflation = 0f;
    private Collider playerCollider;
    private MovementStateManager movementManager;
    private float originalSpeed;

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void DeflatePlayer(float deflationAmount)
    {
        currentInflation = Mathf.Max(currentInflation - deflationAmount, 0f);
        float newScaleXZ = minScale + currentInflation;
        gameObject.transform.localScale = new Vector3(newScaleXZ, originalScale.y, newScaleXZ);

        if (movementManager != null)
        {
            movementManager.velocidadMovimiento = Mathf.Max(
                originalSpeed - (currentInflation * speedReductionPerFood),
                minSpeed
            );
        }
    }

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

        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
        if (modeloGrande != null)
        {
            modeloGrande.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Solo procesa si el modelo no ha sido cambiado todavía
        if (modeloCambiado) return;

        // Si choca con otro jugador, pierde tamaño
        if (collision.gameObject.CompareTag("Player"))
        {
            DeflatePlayer(inflationAmount * 2);
            ReproducirSonido(sonidoGolpeJugador);
            Debug.Log("¡Golpeado por otro jugador! Se reduce el tamaño.");
            return; // Evita seguir procesando comida si era un jugador
        }


        bool smallCompare = collision.gameObject.CompareTag(foodTag);
        bool bigCompare = collision.gameObject.CompareTag(foodTag2);

        if (smallCompare || bigCompare)
        {
            if (smallCompare)
            {
                InflatePlayer("small");
            }
            else
            {
                InflatePlayer("big");
            }


            ReproducirSonido(sonidoComer);
            ReduceSpeed();
            Destroy(collision.gameObject);
            Debug.Log(currentInflation);

            // Si la inflación actual ha alcanzado o superado el umbral, y no hemos cambiado ya
            if (currentInflation >= maxInflation && !modeloCambiado)
            {
                ReproducirSonido(sonidoBola);
                CambiarModelo();
            }
        }
    }

    private void InflatePlayer(string size)
    {
        int veces = 1;
        if (size == "big")
            veces = 3;

        // Infla la escala actual, sin exceder el máximo definido
        currentInflation = Mathf.Min(currentInflation + inflationAmount * veces, maxInflation);

        float newScaleXZ = minScale + currentInflation;
        gameObject.transform.localScale = new Vector3(newScaleXZ, originalScale.y, newScaleXZ);
    }

    private void ReduceSpeed()
    {
        if (movementManager != null)
        {
            movementManager.velocidadMovimiento = Mathf.Max(
                originalSpeed - (currentInflation * speedReductionPerFood),
                minSpeed
            );
        }
    }

    private void CambiarModelo()
    {
        if (gameObject == null || modeloGrande == null)
        {
            Debug.LogError("Modelos pequeño o grande no asignados en el script PersonajeHinchar.");
            return;
        }

        // Desactiva el modelo pequeño
        gameObject.SetActive(false);

        // Coloca el modelo grande en la misma posición y rotación
        modeloGrande.transform.position = gameObject.transform.position;
        modeloGrande.transform.rotation = gameObject.transform.rotation;

        // Activa el modelo grande
        modeloGrande.SetActive(true);

        // Ajusta la cámara para que mire al nuevo modelo
        AjustarCamaraATarget(modeloGrande.transform);

        modeloCambiado = true; // Marca que el modelo ya fue cambiado
        Debug.Log("¡Modelo del personaje cambiado a la versión grande!");
    }
    private void AjustarCamaraATarget(Transform newTarget)
    {
        if (camaraSiguePlayerScript != null)
        {
            camaraSiguePlayerScript.objetivo = newTarget;
            Debug.Log($"Cámara ahora siguiendo a: {newTarget.name}");
        }
        else
        {
            Debug.LogWarning("El script 'CamaraSiguePlayer' no está asignado o no se encontró. La cámara no actualizará su objetivo.");
        }
    }

    // Método para resetear (opcional)
    public void ResetInflation()
    {
        currentInflation = 0f;
        // Si el modelo ya cambió, desactiva el grande y activa el pequeño de nuevo
        if (modeloCambiado)
        {
            if (modeloGrande != null) modeloGrande.SetActive(false);
            if (gameObject != null) gameObject.SetActive(true);
            AjustarCamaraATarget(gameObject.transform); // Volver a apuntar la cámara
            modeloCambiado = false;
        }
        gameObject.transform.localScale = originalScale; 

        if (movementManager != null)
        {
            movementManager.velocidadMovimiento = originalSpeed;
        }
        Debug.Log("Inflación reseteada.");
    }

    public float ObtenerEscalaXZ()
    {
        return minScale + currentInflation;
    }

}