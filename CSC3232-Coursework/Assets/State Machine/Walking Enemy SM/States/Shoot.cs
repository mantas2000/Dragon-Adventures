using UnityEngine;

public class Shoot : AggressiveMS
{
    private readonly GroundEnemySM _sm;
    private float _nextShot;

    public Shoot(GroundEnemySM stateMachine) : base("Shoot", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _nextShot = Time.time;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        // Get distance to player
        var distance = Vector2.Distance(_sm.rigidbody2D.position, _sm.player.position);
        
        // If player is too far, chase player
        if (distance > 7f) StateMachine.ChangeState(_sm.GetInRangeState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        // Fire rate control
        if (!(Time.time > _nextShot)) return;
        
        // Instantiate new bullet
        Object.Instantiate(_sm.bullet, _sm.castPoint.position, _sm.castPoint.rotation);
        
        // Log time when bullet was fired
        _nextShot = Time.time + _sm.fireRate;
    }
}
