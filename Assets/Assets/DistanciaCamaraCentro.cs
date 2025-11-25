using UnityEngine;
using Unity.Cinemachine;

public class DistanciaCamaraCentro : MonoBehaviour
{
    private float distancia = 12.2F;
    private CinemachineThirdPersonFollow thirdPersonFollow;
    public GameObject player1;
    public GameObject player2;
    private float distEntrePlayers;

    void Start(){
        thirdPersonFollow = GetComponent<CinemachineThirdPersonFollow>();

        if(thirdPersonFollow != null){
            thirdPersonFollow.CameraDistance = distancia;
        }
    }

    // Update is called once per frame
    void Update()
    {
        distEntrePlayers = Vector3.Distance(player1.transform.position, player2.transform.position);
        distanciaDinamica();
    }

    void distanciaDinamica(){
        // Aquí vamos a cambiar dinamicamente la distancia de la cámara a los dos jugadores dependiendo de
        // la distancia que estén entre ellos
    }
}
