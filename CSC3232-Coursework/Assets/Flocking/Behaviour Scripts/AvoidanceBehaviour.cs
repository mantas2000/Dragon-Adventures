using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // Abort if no neighbours is found
        if (context.Count == 0)
        {
            return Vector2.zero;
        }
        
        // Define variables
        var avoidanceMove = Vector2.zero;
        var numberOfObjects = 0;
        
        // Sum all points
        foreach (var item in context.Where(item => Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius))
        {
            numberOfObjects++;
            avoidanceMove += (Vector2) (agent.transform.position - item.position);
        }

        // Get average
        if (numberOfObjects > 0)
        {
            avoidanceMove /= numberOfObjects;
        }

        // Create offset from agent position
        //avoidanceMove -= (Vector2) agent.transform.position;

        return avoidanceMove;
    }
}
