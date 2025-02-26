using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityScale = 5f;
    [SerializeField] private float maxJumpTime = 0.5f;

    [Header("Debug Settings")]
    [SerializeField] private bool showGizmos = true;

    private float groundCheckDistance = 1.1f;
    private Keyboard keyboard;
    private float jumpTimeCounter;
    private Rigidbody playerRigidbody;
    private float currentGravityScale;

    // Check if the player is grounded or not by raycasting down the ground
    private bool isGrounded
    {
        get
        {
            return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        }
    }

    private void Awake()
    {
        keyboard = Keyboard.current;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGravityScale = gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Jump input check
        HandleJumping();
    }

    private void HandleJumping()
    {
        // Check if the player is grounded and press the space key to jump
        // If the player is grounded, reset the jump time counter
        if (keyboard.spaceKey.wasPressedThisFrame && isGrounded)
        {
            playerRigidbody.linearVelocity = Vector3.up * jumpForce;
            jumpTimeCounter = maxJumpTime;
        }

        // If the player is still holding the space key and the jump time counter is not zero, keep jumping
        if (keyboard.spaceKey.isPressed && jumpTimeCounter > 0)
        {
            playerRigidbody.linearVelocity = Vector3.up * jumpForce;
            jumpTimeCounter -= Time.deltaTime;
        }
        // If the player is not holding the space key or the jump time counter is zero, stop jumping
        else
        {
            jumpTimeCounter = 0;
        }

        if (playerRigidbody.linearVelocity.y < 0)
        {
            playerRigidbody.linearVelocity += Vector3.up * Physics.gravity.y * (currentGravityScale - 1) * Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        }
    }
}
