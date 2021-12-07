using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [SerializeField] private FlockAgent agentPrefab;
    [SerializeField] private FlockBehaviour behaviour;
    [Range(10, 500)] public int startingCount = 250;
    [Range(1f, 100f)] public float driveFactor = 10f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    [Range(1f, 10f)] public float neighbourRadius = 1.5f;
    [Range(0f, 1f)] public float avoidanceRadiusMultiplier = 0.5f;

    private List<FlockAgent> _agents = new List<FlockAgent>();
    private const float AgentDensity = 0.08f;
    private float _squareMaxSpeed;
    private float _squareNeighbourRadius;
    private float _squareAvoidanceRadius;

    public float SquareAvoidanceRadius => _squareAvoidanceRadius;

    // Start is called before the first frame update
    private void Start()
    {
        // Prepare variables
        _squareMaxSpeed = Mathf.Pow(maxSpeed, 2);
        _squareNeighbourRadius = Mathf.Pow(neighbourRadius, 2);
        _squareAvoidanceRadius = _squareNeighbourRadius * Mathf.Pow(avoidanceRadiusMultiplier, 2);

        for (int i = 0; i < startingCount; i++)
        {
            // Create new agent
            var newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * Random.Range(0, 360)),
                transform
            );
            
            // Set agent's name
            newAgent.name = "Agent " + i;
            
            // Add new agent to the agent's list
            _agents.Add(newAgent);
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

            move *= driveFactor;
            
            // Check if movement speed is not above maximum speed
            if (move.sqrMagnitude > _squareMaxSpeed)
            {
                // Normalise speed
                move = move.normalized * maxSpeed;
            }
            
            // Move agent
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        // Create transform list

        // Get all objects around agent in selected radius
        var contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighbourRadius);

        // Return all objects what are not associated with the agent
        return (from collider in contextColliders where collider != agent.AgentCollider select collider.transform).ToList();
    }
}
