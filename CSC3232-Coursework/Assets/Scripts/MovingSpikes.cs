using UnityEngine;
using Random = UnityEngine.Random;

public class MovingSpikes : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool _completed = true;

    private void FixedUpdate()
    {
        // Check if previous change is completed
        if (!_completed) return;

        // Change to random spikes' state after a random time
        Invoke(nameof(ChangeState), Random.Range(1, 3));

        // Disable new random time generation
        _completed = false;
    }

    private void ChangeState()
    {
        // Select new state randomly
        var active = Random.value > 0.5f;

        // Change spikes state
        animator.SetBool("SpikesActive", active);
        
        // Change object's tag depending on state
        gameObject.tag = active ? "Obstacle" : "Not_Obstacle";
        
        // Mark change as completed
        _completed = true;
    }
}
