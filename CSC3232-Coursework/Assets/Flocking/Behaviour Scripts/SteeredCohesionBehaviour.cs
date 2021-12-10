using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/SteeredCohesion")]
public class SteeredCohesionBehaviour : FlockBehaviour
{
    [SerializeField] private float agentSmoothTime = 0.5f; 
    private Vector2 currentVelocity;
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flockManager)
    {
        // Abort if no neighbours is found
        if (context.Count == 0)
        {
            return Vector2.zero;
        }
        
        // Sum all points
        var cohesionMove = context.Aggregate(Vector2.zero, (current, item) => current + (Vector2) item.position);
        
        // Get average
        cohesionMove /= context.Count;
        
        // Create offset from agent position
        cohesionMove -= (Vector2) agent.transform.position;
        
        // Smooth agent's movement
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);

        return cohesionMove;
    }
}
