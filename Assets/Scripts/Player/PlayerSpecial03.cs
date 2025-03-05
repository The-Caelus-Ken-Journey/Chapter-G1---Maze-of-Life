using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSpecial03 : MonoBehaviour
{
    public enum AttackState { Logical, Emotional }
    public AttackState currentAttackState = AttackState.Logical;

    public LayerMask enemyLayer;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public GameObject protectionSpherePrefab;
    public TextMeshProUGUI debugText;
    public float attackRadius = 5f;
    public float bulletSpeed = 10f;
    public float homingStrength = 2f;
    public float shrinkScale = 0.3f;
    public List<GameObject> capturedEnemies = new List<GameObject>();

    private GameObject protectionSphere;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) PerformLogicalAttack();
        if (Input.GetKeyDown(KeyCode.Mouse1)) PerformEmotionalAttack();
        if (Input.GetKeyDown(KeyCode.F)) ThrowCapturedEnemy();
    }

    void PerformLogicalAttack()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
        int targetCount = Mathf.Min(enemies.Length, 5);

        for (int i = 0; i < targetCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();

            if (bulletScript != null)
            {
                bulletScript.SetTarget(enemies[i].transform, bulletSpeed); // Removed homing strength
            }
        }
        UpdateDebugText($"Logical: Shot at {targetCount} enemies");
    }

    void PerformEmotionalAttack()
    {
        if (protectionSphere == null)
            protectionSphere = Instantiate(protectionSpherePrefab, transform.position, Quaternion.identity, transform);

        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            enemy.transform.localScale *= shrinkScale;
            capturedEnemies.Add(enemy.gameObject);
        }
        UpdateDebugText($"Emotional: Captured {capturedEnemies.Count} enemies");
    }

    public void ThrowCapturedEnemy()
    {
        if (capturedEnemies.Count > 0)
        {
            GameObject thrownEnemy = capturedEnemies[0];
            capturedEnemies.RemoveAt(0);
            thrownEnemy.transform.localScale = Vector3.one;
            Rigidbody rb = thrownEnemy.AddComponent<Rigidbody>();
            rb.linearVelocity = transform.forward * bulletSpeed;
            UpdateDebugText("Threw an enemy");
        }
    }

    private void UpdateDebugText(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
            Debug.Log(message);
        }
    }
}
