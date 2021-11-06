using UnityEngine;

public class GroundEnemySM : StateMachine
{
    [HideInInspector] public Patrol PatrolState;
    [HideInInspector] public Idle IdleState;
    [HideInInspector] public GetInRange GetInRangeState;
    [HideInInspector] public Shoot ShootState;
    [HideInInspector] public Stunt StuntState;

    public Rigidbody2D enemy;
    public new SpriteRenderer renderer;
    public Animator animator;
    public Transform[] waypoints;
    public Transform castPoint;
    public Transform player;
    public GameObject bullet;
    public float moveSpeed = 4f;
    public float visionDistance = 10f;
    public float fireRate = 3f;
    private string _currentState;

    private void Awake()
    {
        IdleState = new Idle(this);
        PatrolState = new Patrol(this);
        GetInRangeState = new GetInRange(this);
        ShootState = new Shoot(this);
        StuntState = new Stunt(this);
    }

    protected override BaseState GetInitialState()
    {
        return PatrolState;
    }
    
    public void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        
        animator.Play(newState);

        _currentState = newState;
    }
}
