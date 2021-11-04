using UnityEngine;

public class GroundEnemySM : StateMachine
{
    [HideInInspector] public Patrol PatrolState;
    [HideInInspector] public Idle IdleState;
    [HideInInspector] public GetInRange GetInRangeState;
    [HideInInspector] public Shoot ShootState;

    public new Rigidbody2D rigidbody2D;
    public new SpriteRenderer renderer;
    public Transform[] waypoints;
    public Transform castPoint;
    public Transform player;
    public GameObject bullet;
    public float moveSpeed = 4f;
    public float visionDistance = 10f;
    public float fireRate = 3f;

    private void Awake()
    {
        IdleState = new Idle(this);
        PatrolState = new Patrol(this);
        GetInRangeState = new GetInRange(this);
        ShootState = new Shoot(this);
    }

    protected override BaseState GetInitialState()
    {
        return PatrolState;
    }
}
