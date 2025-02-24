using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityMultiplier = 5f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    private Keyboard keyboard;
    private Rigidbody playerRigidbody;

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

    // Update is called once per frame
    void Update()
    {

    }
}
