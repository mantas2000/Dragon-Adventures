using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/ObstaclesAvoidance")]
public class ObstaclesAvoidanceBehaviour : FlockBehaviour
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
        
        // Filter
        context = Filter(context, flock);
        
        // Sum all points
        foreach (var item in context)
        {
            // Get closest object
            var closestPoint = (Vector3) item.gameObject.GetComponent<Collider2D>().ClosestPoint(agent.transform.position);
            
            if (Vector2.SqrMagnitude(closestPoint - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                numberOfObjects++;
                avoidanceMove += (Vector2) (agent.transform.position - item.position);
            }
        }

        // Get average
        if (numberOfObjects > 0)
        {
            avoidanceMove /= numberOfObjects;
        }

        return avoidanceMove;
    }
    
    private static List<Transform> Filter(IEnumerable<Transform> original, Flock flock)
    {
        // Return agents that are colliding with selected layers
        return original.Where(item => flock.ObstacleLayers == (flock.ObstacleLayers | (1 << item.gameObject.layer))).ToList();
    }
}