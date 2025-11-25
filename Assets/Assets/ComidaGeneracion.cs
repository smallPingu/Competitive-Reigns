using UnityEngine;
using System.Collections.Generic;

public class ComidaGeneracion : MonoBehaviour
{
    [Tooltip("Lista de prefabs a instanciar. Asegúrate de que tengan un componente Rigidbody para que caigan.")]
    public List<GameObject> prefabsToSpawn;

    [Tooltip("Tiempo entre generaciones (segundos)")]
    public float spawnInterval = 2f;

    [Header("Área de Generación (Anillo Superior)")]
    [Tooltip("Altura de generación sobre el centro del coliseo (Y)")]
    public float spawnAltitude = 11f;

    [Tooltip("Radio interior del anillo de generación (debe ser > radio del coliseo)")]
    public float spawnRingInnerRadius = 7.0f;

    [Tooltip("Radio exterior del anillo de generación")]
    public float spawnRingOuterRadius = 10.0f;

    [Header("Simulación de Lanzamiento por Ángulo")]
    [Tooltip("¿Simular lanzamiento con ángulo o dejar caer verticalmente?")]
    public bool simulateThrow = true;

    [Tooltip("Ángulo de lanzamiento en grados (0 = horizontal, -45 = 45 grados hacia abajo)")]
    public float launchAngleDegrees = -45f;

    [Tooltip("Fuerza inicial/velocidad del lanzamiento")]
    public float launchForce = 8f; // Ajusta según sea necesario

    private float timer = 0f;
    private Transform spawnedObjectsParent;

    void Start()
    {
        spawnedObjectsParent = new GameObject("SpawnedPrefabs").transform;
        spawnedObjectsParent.SetParent(transform.parent); // O null para ponerlo en la raíz
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnPrefab();
        }
    }

    void SpawnPrefab()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Count == 0)
        {
            Debug.LogWarning("No hay prefabs asignados en la lista 'prefabsToSpawn'.");
            return;
        }

        // 1. Seleccionar prefab
        int randomIndex = Random.Range(0, prefabsToSpawn.Count);
        GameObject prefab = prefabsToSpawn[randomIndex];

        // 2. Calcular posición en anillo superior
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float radius = Random.Range(spawnRingInnerRadius, spawnRingOuterRadius);
        float xPos = transform.position.x + radius * Mathf.Cos(angle);
        float zPos = transform.position.z + radius * Mathf.Sin(angle);
        Vector3 spawnPosition = new Vector3(xPos, transform.position.y + spawnAltitude, zPos);

        // 3. Instanciar
        GameObject newObject = Instantiate(
            prefab,
            spawnPosition,
            Random.rotation,
            spawnedObjectsParent
        );

        // 4. Asegurar Rigidbody
        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = newObject.AddComponent<Rigidbody>();
            Debug.LogWarning($"El prefab '{prefab.name}' no tenía Rigidbody. Se añadió uno.", newObject);
        }
        rb.useGravity = true;

        // 5. Simular lanzamiento con ángulo
        if (simulateThrow)
        {
            // Calcular dirección horizontal hacia el centro del coliseo (desde el punto de spawn)
            Vector3 horizontalDirectionToCenter = (transform.position - spawnPosition);
            horizontalDirectionToCenter.y = 0; // Ignorar diferencia de altura para la dirección horizontal
            horizontalDirectionToCenter.Normalize(); // Vector unitario XZ apuntando hacia adentro

            // Convertir ángulo de grados a radianes
            float launchAngleRadians = launchAngleDegrees * Mathf.Deg2Rad;

            // Calcular los componentes del vector de lanzamiento
            // Componente horizontal (proyección en el plano XZ)
            Vector3 horizontalVelocity = horizontalDirectionToCenter * Mathf.Cos(launchAngleRadians);
            // Componente vertical
            Vector3 verticalVelocity = Vector3.up * Mathf.Sin(launchAngleRadians);

            // Combinar y normalizar para obtener la dirección final del lanzamiento
            Vector3 launchDirection = (horizontalVelocity + verticalVelocity).normalized;

            // Aplicar la fuerza o establecer velocidad inicial
            // Usar AddForce con Impulse es bueno para un "empujón" inicial
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
            
            // Alternativa: Establecer velocidad directamente (puede sentirse diferente)
            // rb.velocity = launchDirection * launchForce; // Renombrar 'launchForce' a 'launchSpeed' si usas esto
        }

        // Debug.Log($"Prefab '{newObject.name}' generado en: {spawnPosition}");
    }

    // Método para limpiar todos los prefabs generados
    public void ClearAllPrefabs()
    {
        if (spawnedObjectsParent != null)
        {
            Destroy(spawnedObjectsParent.gameObject);
            spawnedObjectsParent = new GameObject("SpawnedPrefabs").transform;
            spawnedObjectsParent.SetParent(transform.parent);
        }
        timer = 0f;
    }

    // Dibujar gizmos para visualizar el área en el editor
    private void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position + Vector3.up * spawnAltitude;
        
        // Dibujar anillos de generación
        Gizmos.color = Color.cyan;
        for (int i = 0; i < 12; i++)
        {
            float angle = (i / 12f) * 2f * Mathf.PI;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            Gizmos.DrawLine(center + dir * spawnRingInnerRadius, center + dir * spawnRingOuterRadius);
        }

        // Dibujar el coliseo (aproximado)
        Gizmos.color = new Color(0.8f, 0.8f, 0.8f, 0.4f); 
        float coliseumRadius = 6.5f;
        float coliseumHeight = 9.5f;
        Vector3 baseColiseo = transform.position;
        Vector3 topColiseo = transform.position + Vector3.up * coliseumHeight;
        for (int i = 0; i < 8; i++) 
        {
             float angle = (i / 8f) * 2f * Mathf.PI;
             Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
             Gizmos.DrawLine(baseColiseo + dir * coliseumRadius, topColiseo + dir * coliseumRadius);
        }

        // Dibujar vectores de lanzamiento de ejemplo (si se simula)
        if (simulateThrow)
        {
            Gizmos.color = Color.red;
            int numVectorsToShow = 8; // Cuántos vectores de ejemplo dibujar
            float launchAngleRadians = launchAngleDegrees * Mathf.Deg2Rad;

            for (int i = 0; i < numVectorsToShow; i++)
            {
                // Calcular un punto de spawn de ejemplo en el anillo exterior
                float angle = (i / (float)numVectorsToShow) * 2f * Mathf.PI;
                Vector3 spawnPointExample = center + new Vector3(Mathf.Cos(angle) * spawnRingOuterRadius, 0, Mathf.Sin(angle) * spawnRingOuterRadius);
                
                // Calcular la dirección de lanzamiento desde ese punto (igual que en SpawnPrefab)
                 Vector3 horizontalDirectionToCenter = (transform.position - spawnPointExample);
                 horizontalDirectionToCenter.y = 0; 
                 horizontalDirectionToCenter.Normalize(); 
                 Vector3 horizontalVelocity = horizontalDirectionToCenter * Mathf.Cos(launchAngleRadians);
                 Vector3 verticalVelocity = Vector3.up * Mathf.Sin(launchAngleRadians);
                 Vector3 launchDirection = (horizontalVelocity + verticalVelocity).normalized;

                // Dibujar una línea corta representando el vector de lanzamiento
                Gizmos.DrawLine(spawnPointExample, spawnPointExample + launchDirection * 2f); // Longitud 2 para visualización
            }
        }
    }
}