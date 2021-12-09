using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    public Collider2D AgentCollider { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        AgentCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity)
    {
        // Turn agent towards direction
        transform.up = velocity;
        
        // Move agent
        transform.position += (Vector3) velocity * Time.deltaTime;
        
        // Flip sprite
        renderer.flipX = velocity.x < 0;
    }
}
