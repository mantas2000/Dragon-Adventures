using UnityEngine;

public class GetInRange : AggressiveMS
{
    private readonly GroundEnemySM _sm;

    public GetInRange(GroundEnemySM stateMachine) : base("GetInRange", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        // Calculate distance between enemy and player
        var distance = Vector2.Distance(_sm.enemy.position, _sm.player.position);
        
        // Go to Shoot state if player is in shooting range
        if (distance < 7f) StateMachine.ChangeState(_sm.ShootState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        
        // Increase enemy's speed
        var catchSpeed = _sm.moveSpeed * 3;
        
        // Chase down player
        _sm.enemy.position =
            Vector2.MoveTowards(_sm.enemy.position, _sm.player.position, catchSpeed * Time.deltaTime);
    }
}