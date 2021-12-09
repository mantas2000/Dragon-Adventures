using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Seek")]
public class SeekBehaviour : FlockBehaviour
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // Measure distance between agent and target
        var targetOffset = flock.Target.position - agent.transform.position;

        return targetOffset;
    }
}
