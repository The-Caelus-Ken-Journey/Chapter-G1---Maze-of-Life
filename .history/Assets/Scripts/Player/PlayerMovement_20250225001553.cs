using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody playerRigidbody;
    private float horizontalInput;
    private Keyboard keyboard;

    // Check if the player is grounded or not by raycasting down the ground
    private bool isGrounded
    {
        get
        {
            return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        }
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        keyboard = Keyboard.current;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Jump input check
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            HandleJumping();
        }
    }

    void FixedUpdate()
    {
        playerRigidbody.AddForce(Physics.gravity * (gravityMultiplier - 1) * playerRigidbody.mass);
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 movement = new Vector3(horizontalInput * speed, playerRigidbody.linearVelocity.y, 0);
        playerRigidbody.linearVelocity = movement;
    }

    private void HandleJumping()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.down * 0.1f, transform.position + Vector3.down * (0.1f + groundCheckDistance));
    }
}
