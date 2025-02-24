using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody playerRigidbody;
    private float horizontalInput;

    private bool isGrounded
    {
        get
        {
            return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    // FixedUpdate is called once per frame, but it is called on a fixed time step
    void FixedUpdate()
    {
        HandleMovement();
    }

    // Private method to handle player movement using a rigidbody component.
    private void HandleMovement()
    {
        Vector3 movement = new Vector3(horizontalInput, 0, 0);
        playerRigidbody.linearVelocity = movement * speed;
    }

    // Private method to make the player jump
    private void HandleJumping()
    {
        if (isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Gizmos Drawing for movement debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

}
