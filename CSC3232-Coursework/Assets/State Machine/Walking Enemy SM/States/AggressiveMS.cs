using UnityEngine;

public class AggressiveMS : BaseState
{
    private readonly GroundEnemySM _sm;

    protected AggressiveMS(string name, GroundEnemySM stateMachine) : base(name, stateMachine)
    {
        _sm = stateMachine;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        // Calculate distance between enemy and player
        var distance = Vector2.Distance(_sm.rigidbody2D.position, _sm.player.position);

        // If player is out of vision, go to Passive Meta state
        if (distance > _sm.visionDistance) StateMachine.ChangeState(_sm.IdleState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _sm.renderer.flipX = _sm.rigidbody2D.position.x < _sm.player.position.x;
    }
}