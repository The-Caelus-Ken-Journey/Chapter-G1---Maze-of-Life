using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    public void SetTarget(Transform newTarget, float newSpeed)
    {
        speed = newSpeed;

        if (newTarget != null)
        {
            direction = (newTarget.position - transform.position).normalized; // Calculate straight direction
        }
        else
        {
            direction = transform.forward; // Move forward if no target
        }

        Destroy(gameObject, 5f); // Destroy after 5 seconds if it doesn't hit anything
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime; // Move in a straight line
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Make sure enemies have the "Enemy" tag
        {
            Destroy(other.gameObject); // Destroy enemy on hit
            Destroy(gameObject); // Destroy bullet
        }
    }
}
