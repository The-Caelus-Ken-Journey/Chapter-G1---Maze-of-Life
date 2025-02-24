using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody playerRigidbody;
    private float horizontalInput;


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
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
