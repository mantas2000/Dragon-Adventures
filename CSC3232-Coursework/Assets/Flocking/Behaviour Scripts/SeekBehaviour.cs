using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Seek")]
public class SeekBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, FlockManager flockManager)
    {
        // Measure distance between agent and target
        var targetOffset = flockManager.Target.position - agent.transform.position;

        return targetOffset;
    }
}
