using UnityEngine;

public class StayInMiddle : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private Vector3 posicion1;
    private Vector3 posicion2;
    private Vector3 posicionMedia;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actualizarPosicion();
        calcularMediatriz();
    }

    // Update is called once per frame
    void Update()
    {
        actualizarPosicion();
        calcularMediatriz();
        transform.position = new Vector3(posicionMedia.x, transform.position.y, posicionMedia.z);
    }

    void actualizarPosicion()
    {
        posicion1 = player1.transform.position;
        posicion2 = player2.transform.position;
    }

    void calcularMediatriz()
    {
        float x1 = posicion1.x;
        float x2 = posicion2.x;
        float z1 = posicion1.z;
        float z2 = posicion2.z;

        posicionMedia.x = (x1 + x2) / 2;
        posicionMedia.z = (z1 + z2) / 2;
    }
}