using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 200f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private Transform enemy;
    public Seeker seeker;
    public Rigidbody2D rb;
    private int _currentWaypoint = 0;
    private bool _reachedEndOfPath = false;
    private Path _path;
    
    // Start is called before the first frame update
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        // Start path
        InvokeRepeating(nameof(UpdatePath), 0f, .5f);
    }
    
    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path path)
    {
        // Check for errors
        if (!path.error)
        {
            // Set new path
            _path = path;
            
            // Reset current waypoint
            _currentWaypoint = 0;
        }
    }
    
    private void FixedUpdate()
    {
        // Check if path is available
        if (_path == null) return;
        
        // Check if destination is reached
        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            _reachedEndOfPath = true;
            return;
        }
        else
        {
            _reachedEndOfPath = false;
        }
        
        // Calculate next direction
        var direction = ((Vector2) _path.vectorPath[_currentWaypoint] - rb.position).normalized;
        
        // Move game object
        var force = direction * (speed * Time.deltaTime);
        rb.AddForce(force);
        
        // Calculate distance between current position and next waypoint
        var distance = Vector2.Distance(rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            _currentWaypoint++;
        }
        
        // Flip enemy according to its desired destination
        if (force.x >= 0.01f)
        {
            enemy.localScale = new Vector3(-7f, 7f, 7f);
        }
        
        else if (force.x <= -0.01f)
        {
            enemy.localScale = new Vector3(7f, 7f, 7f);
        }
    }
}
