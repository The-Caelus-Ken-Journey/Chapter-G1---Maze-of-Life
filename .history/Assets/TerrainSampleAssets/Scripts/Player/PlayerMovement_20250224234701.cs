using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody playerRigidbody;
    private float horizontalInput;
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
        HandleJumping();
    }

    // Private method to handle player movement using a rigidbody component.
    private void HandleMovement()
    {
        Vector3 movement = new Vector3(horizontalInput, 0, 0);
        playerRigidbody.linearVelocity = movement * speed;
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
