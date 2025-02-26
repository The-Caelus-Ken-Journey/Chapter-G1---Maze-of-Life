using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSpecial02 : MonoBehaviour
{
    [Header("Special Jump Settings")]
    [SerializeField] private bool canDoubleJump = false;
    [SerializeField] private bool canTripleJump = false;
    [SerializeField] private float jumpForce = 5f;

    [Header("Game Mode")]
    [SerializeField] private bool isHardMode = false;

    [Header("Agility Meter Settings")]
    [SerializeField] private Slider agilitySlider;
    [SerializeField] private float maxAgility = 100f;
    [SerializeField] private float agilityRechargeRate = 10f;

    [Header("Debug UI")]
    [SerializeField] private TMP_Text debugText;
    [SerializeField] private Toggle hardModeToggle;

    private float agilityCost;
    private float currentAgility;
    private int jumpCount = 0;
    private Keyboard keyboard;
    private Rigidbody playerRigidbody;
    private float groundCheckDistance = 1.1f;

    private bool isGrounded => Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

    void Start()
    {
        keyboard = Keyboard.current;
        playerRigidbody = GetComponent<Rigidbody>();
        debugText.text = "Special Jump";

        currentAgility = maxAgility;
        agilitySlider.maxValue = maxAgility;
        agilitySlider.value = currentAgility;

        if (hardModeToggle != null)
        {
            hardModeToggle.onValueChanged.AddListener(delegate { ToggleHardMode(); });
        }
    }

    void Update()
    {
        if (isGrounded)
        {
            jumpCount = 0; // Reset jump count when grounded
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHardMode();
        }

        HandleDoubleTripleJump();
        RechargeAgility();
    }

    private void HandleDoubleTripleJump()
    {
        if (keyboard.spaceKey.wasPressedThisFrame && !isGrounded)
        {
            float requiredAgility = 0f;
            string jumpType = "";

            if (canDoubleJump && jumpCount < 1)
            {
                requiredAgility = Random.Range(25f, 35f);
                if (isHardMode) requiredAgility *= 1.15f;
                jumpType = "Double Jump";
            }
            else if (canTripleJump && jumpCount < 2)
            {
                requiredAgility = Random.Range(75f, 90f);
                if (isHardMode) requiredAgility *= 1.20f;
                jumpType = "Triple Jump";
            }

            // Check if player has enough agility before jumping
            if (currentAgility >= requiredAgility)
            {
                agilityCost = requiredAgility;
                PerformJump();
                UpdateSkillText(jumpType);
            }
            else
            {
                Debug.LogWarning($"Not enough Agility to perform {jumpType}!");
                StartCoroutine(StartCooldownCountdown(isHardMode ? 5.0f : 1.5f));
            }
        }
    }

    private void PerformJump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        currentAgility -= agilityCost;
        currentAgility = Mathf.Clamp(currentAgility, 0, maxAgility);
        agilitySlider.value = currentAgility;
        jumpCount++;
    }

    private IEnumerator StartCooldownCountdown(float cooldownTime)
    {
        float remainingTime = cooldownTime;

        while (remainingTime > 0)
        {
            debugText.text = $"Cooldown: {remainingTime:F1}s"; // Show countdown (1 decimal)
            yield return new WaitForSeconds(0.1f); // Update every 0.1 sec
            remainingTime -= 0.1f;
        }

        debugText.text = "Special Jump"; // Reset text after cooldown
    }

    private void RechargeAgility()
    {
        if (currentAgility < maxAgility)
        {
            float rechargeRate = isHardMode ? agilityRechargeRate * 0.5f : agilityRechargeRate; // Reduce recharge in Hard Mode
            currentAgility += rechargeRate * Time.deltaTime;
            currentAgility = Mathf.Clamp(currentAgility, 0, maxAgility);
            agilitySlider.value = currentAgility;
        }
    }

    void ToggleHardMode()
    {
        isHardMode = !isHardMode;
        if (hardModeToggle != null)
        {
            hardModeToggle.isOn = isHardMode;
        }
    }

    private void UpdateSkillText(string jumpType)
    {
        debugText.text = jumpType;
        StartCoroutine(ResetSkillText()); // Reset after a delay
    }

    private IEnumerator ResetSkillText()
    {
        yield return new WaitForSeconds(1.5f);
        debugText.text = "Special Jump";
    }
}
