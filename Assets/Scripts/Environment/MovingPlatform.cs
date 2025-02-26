using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints; // Points to move between
    [SerializeField] private float speed = 3f;
    private int targetIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length == 0) return;

        // Move platform towards target waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[targetIndex].position, speed * Time.deltaTime);

        // Check if the platform reached the waypoint
        if (Vector3.Distance(transform.position, waypoints[targetIndex].position) < 0.1f)
        {
            targetIndex = (targetIndex + 1) % waypoints.Length; // Loop back to first waypoint
        }
    }

    // Called when the Collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    // Called when the Collider other exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
