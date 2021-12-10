using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class AlignmentBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flockManager)
    {
        // Do nothing if no neighbours is found
        if (context.Count == 0)
        {
            return agent.transform.up;
        }
        
        // Sum all points
        var alignmentMove = context.Aggregate(Vector2.zero, (current, item) => current + (Vector2) item.transform.up);
        
        // Get average
        alignmentMove /= context.Count;

        return alignmentMove;
    }
}
