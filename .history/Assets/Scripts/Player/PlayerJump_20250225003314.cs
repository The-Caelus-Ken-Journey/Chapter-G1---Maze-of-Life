using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityScale = 5f;
    [SerializeField] private float maxJumpTime = 0.5f;
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
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            HandleJumping();
        }
    }

    private void FixedUpdate()
    {
        playerRigidbody.AddForce(Physics.gravity * (gravityScale - 1) * playerRigidbody.mass);
    }

    private void HandleJumping()
    {
        if (keyboard.spaceKey.wasPressedThisFrame && isGrounded) playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if (playerRigidbody.linearVelocity.y >= 0) currentGravityScale = gravityScale;
        else currentGravityScale = fallingGravityScale;
    }
}
