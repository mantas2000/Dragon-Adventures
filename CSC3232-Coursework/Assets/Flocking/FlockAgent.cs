using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    private Collider2D _agentCollider;
    
    public Collider2D AgentCollider => _agentCollider;

    // Start is called before the first frame update
    private void Start()
    {
        _agentCollider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity)
    {
        // Turn agent towards direction
        transform.up = velocity;
        transform.position += (Vector3) velocity * Time.deltaTime;
    }
}
