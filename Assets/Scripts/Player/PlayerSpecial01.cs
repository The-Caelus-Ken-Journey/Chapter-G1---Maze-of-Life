using UnityEngine;

public class PlayerSpecial01 : MonoBehaviour
{
    [Header("Climbing Settings")]
    public float climbSpeed = 3f;
    public float wallCheckDistance = 0.6f;

    private Rigidbody rb;
    private bool isClimbing;
    private bool isTouchingWall;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckWall();
        HandleClimbing();
    }

    private void CheckWall()
    {
        RaycastHit hit;
        isTouchingWall = Physics.Raycast(transform.position, transform.forward, out hit, wallCheckDistance);

        // Alternative: Check for a wall by detecting colliders instead of using Raycast
        // isTouchingWall = Physics.CheckSphere(transform.position + transform.forward * wallCheckDistance, 0.3f);
    }

    private void HandleClimbing()
    {
        if (isTouchingWall && Input.GetKey(KeyCode.W)) // Hold 'W' to climb
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, climbSpeed, rb.linearVelocity.z);
            isClimbing = true;
        }
        else if (isClimbing) // Stop climbing if key is released
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            isClimbing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Optional: If you want to ensure climbing only happens on actual walls, you can check tags
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }
    }
}
