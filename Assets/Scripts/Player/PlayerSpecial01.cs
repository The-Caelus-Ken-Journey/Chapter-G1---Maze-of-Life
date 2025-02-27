using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecial01 : MonoBehaviour
{
    [Header("Wall Climb Settings")]
    [SerializeField] private float wallClimbSpeed = 3f;
    [SerializeField] private float maxClimbTime = 2f;
    [SerializeField] private float wallStickTime = 0.2f;

    [Header("Charged Climb Settings")]
    [SerializeField] private float maxChargeTime = 1f;
    [SerializeField] private float chargedClimbBoost = 6f;

    [Header("Wall Jump Settings")]
    [SerializeField] private float normalJumpForce = 5f;
    [SerializeField] private float chargedJumpForce = 10f;
    [SerializeField] private float wallJumpCooldown = 0.5f;

    [Header("Detection")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius = 0.5f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrainNormal = 2f;
    [SerializeField] private float staminaDrainCharged = 50f;
    [SerializeField] private float staminaDrainRate = 15f;
    [SerializeField] private float staminaRegenRate = 0.5f;
    private float currentStamina;

    [Header("Debug UI")]
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private Slider staminaSlider;

    private Rigidbody rb;
    private bool isTouchingWall;
    private bool isWallClimbing;
    private bool isChargingClimb;
    private float climbTimer;
    private float chargeTimer;
    private bool canWallJump = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentStamina = maxStamina;
        staminaSlider.value = currentStamina;
        debugText.text = "Wall Climb";
    }

    private void Update()
    {
        CheckWallCollision();
        HandleWallClimb();
        HandleWallJump();
        RegenerateStamina();
    }

    private void CheckWallCollision()
    {
        isTouchingWall = Physics.CheckSphere(wallCheck.position, wallCheckRadius, wallLayer);

        if (!isTouchingWall)
        {
            isWallClimbing = false;
            isChargingClimb = false;
            climbTimer = maxClimbTime;
        }
    }

    private void HandleWallClimb()
    {
        if (isTouchingWall && Input.GetKey(KeyCode.W) && currentStamina > 0)
        {
            if (climbTimer > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z);
                climbTimer -= Time.deltaTime;
                currentStamina -= staminaDrainNormal * Time.deltaTime * staminaDrainRate;
                isWallClimbing = true;
                debugText.text = "Climbing";
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl) && isTouchingWall)
        {
            isChargingClimb = true;
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0, maxChargeTime);
            debugText.text = "Charging Climb";
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && isChargingClimb)
        {
            float climbBoost = Mathf.Lerp(0, chargedClimbBoost, chargeTimer / maxChargeTime);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed + climbBoost, rb.linearVelocity.z);
            currentStamina -= staminaDrainCharged;
            isChargingClimb = false;
            chargeTimer = 0;
        }
    }

    private void HandleWallJump()
    {
        if (isTouchingWall && canWallJump && Input.GetKeyDown(KeyCode.Space) && currentStamina > 0)
        {
            float jumpForce = normalJumpForce;
            float staminaDrain = staminaDrainNormal;

            if (isChargingClimb)
            {
                jumpForce = chargedJumpForce;
                staminaDrain = staminaDrainCharged;
                isChargingClimb = false;
                chargeTimer = 0;
                debugText.text = "Charged Jump";
            }
            else
            {
                debugText.text = "Normal Jump";
            }

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            currentStamina -= staminaDrain;
            canWallJump = false;
            Invoke(nameof(ResetWallJump), wallJumpCooldown);
        }
    }

    private void ResetWallJump()
    {
        canWallJump = true;
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            staminaSlider.value = currentStamina;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
    }
}
