using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private BoxCollider2D collider;
    private int _currentWaypoint;

    // Update is called once per frame
    private void Update()
    {
        // Check if game object reached one of the waypoints
        if (Vector2.Distance(waypoints[_currentWaypoint].transform.position, transform.position) < .1f)
        {
            // Set next waypoint
            _currentWaypoint = (_currentWaypoint + 1) % waypoints.Length;
        }
        
        // Move towards next waypoint
        transform.position = Vector2.MoveTowards(transform.position, waypoints[_currentWaypoint].transform.position, speed * Time.deltaTime);
        
        // Update AIPath grid during runtime
        AstarPath.active.UpdateGraphs(collider.bounds);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Make game object sticky
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Make game object not sticky
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(null);
        }
    }
}
