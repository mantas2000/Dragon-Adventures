using System;
using Pathfinding;
using UnityEngine;

public class FlyingEnemyGFX : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private const float WaitTime = 1.5f;
    private float _waitCounter;
    private float _maxSpeed;
    public AIPath AIPath;

    private void Awake()
    {
        // Get enemy's max speed
        _maxSpeed = AIPath.maxSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (animator.GetBool("IsStunned"))
        {
            // Start timer
            _waitCounter += Time.deltaTime;

            // Disable enemy's movement
            AIPath.maxSpeed = 0;
            
            // After being idle for a certain amount of time, change animation
            if (_waitCounter >= WaitTime)
            {
                animator.SetBool("IsStunned", false);
            }
        }
        else
        {
            // Enable enemy's movement
            _waitCounter = 0;
            AIPath.maxSpeed = _maxSpeed;
        }
        
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
