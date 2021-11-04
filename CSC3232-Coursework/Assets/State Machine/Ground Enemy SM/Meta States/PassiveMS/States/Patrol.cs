using UnityEngine;

public class Patrol : PassiveMS
{
    private int _currentWaypoint;

    public Patrol(GroundEnemySM stateMachine) : base("Patrol", stateMachine) {}
    public override void Enter()
    {
        base.Enter();
        _currentWaypoint = (_currentWaypoint + 1) % _sm.waypoints.Length;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        // Go to Idle state, if player reached one of the wayponts
        if (Vector2.Distance(_sm.enemy.position, _sm.waypoints[_currentWaypoint].position) < 1f)
        {
            StateMachine.ChangeState(_sm.IdleState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        
        // Flip enemy
        _sm.renderer.flipX = _sm.enemy.position.x < _sm.waypoints[_currentWaypoint].position.x;
        
        // Move towards next waypoint
        _sm.enemy.position = Vector2.MoveTowards(_sm.enemy.position, _sm.waypoints[_currentWaypoint].position, _sm.moveSpeed * Time.deltaTime);
    }
}