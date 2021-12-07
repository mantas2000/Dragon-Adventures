using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
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

        return cohesionMove;
    }
}
