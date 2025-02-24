using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody playerRigidbody;
    private float horizontalInput;

    private bool isGrounded
    {
        get
        {
            return Physics.Raycast(transform.position + Vector3.down * 0.5f, Vector3.down, groundCheckDistance, groundLayer);
        }
    }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Jump input check
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJumping();
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 movement = new Vector3(horizontalInput * speed, playerRigidbody.velocity.y, 0);
        playerRigidbody.velocity = movement;
    }

    private void HandleJumping()
    {
        if (isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.down * 0.1f, transform.position + Vector3.down * (0.1f + groundCheckDistance));
    }
}
