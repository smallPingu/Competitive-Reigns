using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RodarBola : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;
    public float jumpForce = 10f;
    public float gravityMultiplier = 1.3f;
    [Range(0.1f, 1f)] public float turnSmoothing = 0.5f;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.5f;

    [Header("Bounce Settings")]
    public float bounceForce = 5f;

    private Vector2 moveInput;
    private bool jumpInput;

    private Rigidbody rb;
    private int maxJumps = 1;
    private int currentJumps = 0;
    private bool isGrounded;
    private Vector3 currentMoveDirection;

    public Camera camara;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpInput = true;
        }
        else if (context.canceled)
        {
            jumpInput = false;
        }
    }

    private void Update()
    {
        if (jumpInput && (isGrounded || currentJumps < maxJumps))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            currentJumps++;
            isGrounded = false;
            jumpInput = false;
        }

        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        if (isGrounded && currentJumps > 0)
        {
            currentJumps = 0;
        }

        float moveX = moveInput.x;
        float moveZ = moveInput.y;

        Vector3 cameraForward = camara.transform.forward;
        Vector3 cameraRight = camara.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 targetMoveDirection = (cameraForward * moveZ + cameraRight * moveX).normalized;

        currentMoveDirection = Vector3.Lerp(currentMoveDirection, targetMoveDirection, turnSmoothing);

        if (currentMoveDirection.magnitude > 0.1f)
        {
            Vector3 targetVelocity = currentMoveDirection * moveSpeed;
            targetVelocity.y = rb.linearVelocity.y;

            Vector3 velocityChange = (targetVelocity - rb.linearVelocity) * 0.5f;
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            if (isGrounded)
            {
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, currentMoveDirection);
                float rotationMagnitude = rb.linearVelocity.magnitude * rotationSpeed * Time.fixedDeltaTime;
                rb.AddTorque(rotationAxis * rotationMagnitude * Mathf.Clamp01(currentMoveDirection.magnitude), ForceMode.VelocityChange);
            }
        }

        rb.linearDamping = isGrounded ? 1f : 0f;
        rb.angularDamping = isGrounded ? 1f : 0.05f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pared"))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 bounceDirection = Vector3.Reflect(rb.linearVelocity.normalized, contact.normal);

            bounceDirection.y = rb.linearVelocity.y * 0.5f;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + currentMoveDirection * 2f);
    }
}