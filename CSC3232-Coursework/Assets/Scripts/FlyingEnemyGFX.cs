using Pathfinding;
using UnityEngine;

public class FlyingEnemyGFX : MonoBehaviour
{
    public AIPath AIPath;
        
    // Update is called once per frame
    private void Update()
    {
        // Flip enemy according to its desired destination
        if (AIPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-7f, 7f, 7f);
        }
        
        else if (AIPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(7f, 7f, 7f);
        }
    }
}
