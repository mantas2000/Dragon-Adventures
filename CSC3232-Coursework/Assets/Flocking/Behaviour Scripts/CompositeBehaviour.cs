using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    public FlockBehaviour[] behaviours;
    [Range(0, 5)] public float[] weights;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flockManager)
    {
        // Make sure all variables are provided
        if (behaviours.Length != weights.Length)
        {
            return Vector2.zero;
        }
        
        // Create agent's move
        var move = Vector2.zero;
        
        // Iterate through provided behaviours
        for (var i = 0; i < behaviours.Length; i++)
        {
            // Calculate behaviour move
            var partialMove = behaviours[i].CalculateMove(agent, context, flockManager) * weights[i];

            if (partialMove != Vector2.zero)
            {
                // Check if behaviour weight is not above limits
                if (partialMove.sqrMagnitude > Mathf.Pow(weights[i], 2))
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }
                
                // Sum all behaviours move into one
                move += partialMove;
            }
        }
        
        return move;
    }
}
