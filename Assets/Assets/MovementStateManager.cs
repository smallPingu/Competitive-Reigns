using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class MovementStateManager : MonoBehaviour
{
    public float velocidadMovimiento = 5f;
    public float fuerzaSalto = 10f;
    public float factorGravedad = 1.3f;
    public int saltosMaximos = 2;

    private Rigidbody rb;
    private Vector2 inputMovimiento;
    private bool intentoSalto = false;
    private int saltosActuales = 0;
    private PlayerInput playerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        MoverJugador();

        if (intentoSalto && saltosActuales < saltosMaximos)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            saltosActuales++;
            intentoSalto = false;
        }

        // Aplicar gravedad adicional después del primer salto para una sensación más realista
        if (saltosActuales > 0 && !EstaEnSuelo())
        {
            rb.AddForce(Vector3.down * factorGravedad, ForceMode.Acceleration);
        }
    }

    void MoverJugador()
{
    Vector3 direccion = new Vector3(inputMovimiento.x, 0, inputMovimiento.y);

    if (direccion.magnitude > 0.1f)
    {
        // Calcular dirección de movimiento en el mundo
        Vector3 direccionMundo = direccion.normalized;

        // Rotar el personaje hacia la dirección de movimiento
        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMundo);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * 10f);
    }

    // Mover al personaje
    Vector3 movimiento = direccion.normalized * velocidadMovimiento;
    rb.linearVelocity = new Vector3(movimiento.x, rb.linearVelocity.y, movimiento.z);
}

    void OnCollisionEnter(Collision colision)
    {
        if (colision.gameObject.CompareTag("Ground"))
        {
            saltosActuales = 0;
        }
    }

    private bool EstaEnSuelo()
    {
        // Realiza una pequeña superposición de esfera hacia abajo para detectar el suelo
        return Physics.CheckSphere(transform.position + Vector3.down * 0.1f, 0.2f, LayerMask.GetMask("Ground"));
    }

    // Métodos de Input System (Unity Events)
    public void OnMove(InputAction.CallbackContext context)
    {
        inputMovimiento = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            intentoSalto = true;
        }
    }
}