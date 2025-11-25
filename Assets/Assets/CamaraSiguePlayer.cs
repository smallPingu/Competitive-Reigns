using UnityEngine;

public class CamaraSiguePlayer : MonoBehaviour
{
    public float velocidad = 5F;
    public Transform objetivoNormal;
    public Transform objetivoBola;
    public Transform objetivo;

    void Start()
    {
        objetivo = objetivoNormal;
    }

    void Update()
    {
        if (objetivoNormal != null && !objetivoNormal.gameObject.activeSelf) 
        {
            objetivo = objetivoBola;
        }
        else if (objetivoNormal != null)
        {
            objetivo = objetivoNormal;
        }
    }

    void LateUpdate(){
        if (objetivo != null)
        {
            Vector3 nuevaPosicion = new Vector3(objetivo.position.x, transform.position.y, objetivo.position.z - 8.5F);
            transform.position = Vector3.Lerp(transform.position, nuevaPosicion, velocidad * Time.deltaTime);
        }
    }
}