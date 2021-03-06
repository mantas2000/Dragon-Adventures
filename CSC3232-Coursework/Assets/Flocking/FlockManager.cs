using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlockManager : MonoBehaviour
{
    [SerializeField] private FlockAgent agentPrefab;
    [SerializeField] private Vector2 sceneSize = new Vector2(10, 20);
    [SerializeField] private int startingCount = 20;
    [SerializeField] private Transform target;
    [SerializeField] private FlockBehaviour behaviour;
    [SerializeField] private LayerMask obstacleLayers;
    public float maxForce = 20f;
    [Range(1f, 10f)] public float neighbourRadius = 1.5f;
    [Range(0f, 1f)] public float avoidanceRadiusMultiplier = 0.5f;

    private readonly List<FlockAgent> _agents = new List<FlockAgent>();
    private float _squareMaxForce;
    private float _squareNeighbourRadius;
    private float _squareAvoidanceRadius;

    public float SquareAvoidanceRadius => _squareAvoidanceRadius;
    public Transform Target => target;
    public LayerMask ObstacleLayers => obstacleLayers;

    // Start is called before the first frame update
    private void Start()
    {
        // Prepare max values
        _squareMaxForce = Mathf.Pow(maxForce, 2);
        _squareNeighbourRadius = Mathf.Pow(neighbourRadius, 2);
        _squareAvoidanceRadius = _squareNeighbourRadius * Mathf.Pow(avoidanceRadiusMultiplier, 2);
        
        // Define spawn area
        var spawnArea = new Vector2(sceneSize.x - transform.position.x, sceneSize.y - transform.position.y);

        for (var i = 0; i < startingCount; i++)
        {
            // Create new agent
            var newAgent = Instantiate(
                agentPrefab,
                new Vector3(Random.Range(-spawnArea.x, spawnArea.x), Random.Range(-spawnArea.y, spawnArea.y), 0f),
                Quaternion.identity,
                transform
            );

            // Confirm agent spawned in valid location (not colliding with obstacles)
            if (VerifyLocation(newAgent))
            {
                // Set agent's name
                newAgent.name = "Agent " + i;

                // Add new agent to the agent's list
                _agents.Add(newAgent);
            }

            else
            {
                i--;
                Destroy(newAgent.gameObject);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (var agent in _agents)
        {
            // Get all objects around agent
            var contexts = GetNearbyObjects(agent);
            
            // Calculate agent's move
            var move = behaviour.CalculateMove(agent, contexts, this);

            move *= 20f;

            // Check if movement speed is not above maximum speed
            if (move.sqrMagnitude > _squareMaxForce)
            {
                // Normalise speed
                move = move.normalized * maxForce;
            }
            
            // Move agent
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        // Get all objects around agent in selected radius
        var contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);

        // Return all objects what are not associated with the agent
        return (from collider in contextColliders where collider != agent.AgentCollider select collider.transform).ToList();
    }

    private bool VerifyLocation(FlockAgent agent)
    {
        // True, if agent is not colliding with ground layer
        var touchingGround =  GetNearbyObjects(agent).All(agentCollider => agentCollider.gameObject.layer != 7);
        
        // True, if agent is not colliding with bridge layer
        var touchingBridge =  GetNearbyObjects(agent).All(agentCollider => agentCollider.gameObject.layer != 8);

        return touchingGround && touchingBridge;
    }
}
