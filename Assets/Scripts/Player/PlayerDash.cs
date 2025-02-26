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

    [Header("Game Mode")]
    [SerializeField] private bool isHardMode = false;

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
    [SerializeField] private Slider dashMeterUI;
    [SerializeField] private Toggle hardModeToggle;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (hardModeToggle != null)
        {
            hardModeToggle.onValueChanged.AddListener(delegate { ToggleHardMode(); });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHardMode = !isHardMode;
        }
        CheckGrounded();
        if (!isHardMode) RegenerateDashMeter();
        UpdateDashMeterUI();

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

    void ToggleHardMode()
    {
        isHardMode = hardModeToggle.isOn;
        Debug.Log($"Hard Mode: {(isHardMode ? "ON" : "OFF")}");
    }

    void RegenerateDashMeter()
    {
        if (isHardMode) return;

        if (dashMeter < meterMax)
        {
            dashMeter += meterRegenRate * Time.deltaTime;
            dashMeter = Mathf.Clamp(dashMeter, 0, meterMax);
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
            Debug.LogWarning("Not enough Dash Meter!");
            yield break;
        }

        // Subtract dash cost
        dashMeter -= dashCost;

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
