using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float groundCheckDistance = 1.1f;

    [Header("Dash Meter Settings")]
    [SerializeField] private float dashMeter = 100f;
    [SerializeField] private float minDashMeterCost = 5f;
    [SerializeField] private float maxDashMeterCost = 10f;
    [SerializeField] private float meterRegenRate = 10f;
    [SerializeField] private float meterMax = 100f;

    private int maxAirDashes = 1;
    private bool isDashing = false;
    private bool canDash = true;
    private int airDashCount;
    private Vector3 dashDirection;
    private Rigidbody playerRigidbody;

    private bool isGrounded
    {
        get
        {
            return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        }
    }

    [Header("UI (Optional)")]
    [SerializeField] private Slider dashMeterUI; // Attach a UI Slider in Inspector

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckGrounded();
        RegenerateDashMeter();
        UpdateDashMeterUI(); // Updates UI if assigned

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashMeter > 0)
        {
            StartCoroutine(Dash());
        }
    }

    private void CheckGrounded()
    {
        if (isGrounded)
        {
            airDashCount = maxAirDashes;
        }
    }

    void RegenerateDashMeter()
    {
        if (dashMeter < meterMax)
        {
            dashMeter += meterRegenRate * Time.deltaTime;
            dashMeter = Mathf.Clamp(dashMeter, 0, meterMax);
            Debug.Log($"Regenerating... Current Meter: {dashMeter:F2}");
        }

    }

    void UpdateDashMeterUI()
    {
        if (dashMeterUI != null)
        {
            dashMeterUI.value = dashMeter / meterMax; // Normalize to 0-1 for UI Slider
        }
    }

    IEnumerator Dash()
    {
        if (!isGrounded && airDashCount <= 0)
        {
            yield break;
        }

        float dashCost = Random.Range(minDashMeterCost, maxDashMeterCost);

        // Ensure enough meter before dashing
        if (dashMeter < dashCost)
        {
            Debug.Log("Not enough Dash Meter!");
            yield break;
        }

        // Subtract dash cost
        dashMeter -= dashCost;
        Debug.Log($"Dash used! Current meter: {dashMeter:F2}");

        canDash = false;
        isDashing = true;
        playerRigidbody.useGravity = false;
        dashDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        float dashEndTime = Time.time + dashDuration;
        while (Time.time < dashEndTime)
        {
            playerRigidbody.linearVelocity = dashDirection * dashSpeed;
            yield return null;
        }

        playerRigidbody.useGravity = true;
        isDashing = false;
        playerRigidbody.linearVelocity = Vector3.zero;

        if (!isGrounded)
        {
            airDashCount--;
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
