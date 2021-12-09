using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    public Collider2D AgentCollider { get; private set; }
    private Quaternion rotation;

    // Start is called before the first frame update
    private void Start()
    {
        AgentCollider = GetComponent<Collider2D>();
        rotation = transform.rotation;
    }

    public void Move(Vector2 velocity)
    {
        // Turn agent towards direction
        transform.up = velocity;
        
        // Move agent
        transform.position += (Vector3) velocity * Time.deltaTime;
    }

    private void LateUpdate()
    {
        //transform.rotation = rotation;
    }
}
