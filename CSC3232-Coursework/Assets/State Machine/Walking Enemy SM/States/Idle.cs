using UnityEngine;

public class Idle : PassiveMS
{
    private const float WaitTime = 1f;
    private float _waitCounter;
    
    public Idle(GroundEnemySM stateMachine) : base("Idle", stateMachine) {}
    public override void Enter()
    {
        base.Enter();
        _waitCounter = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        // Start timer
        _waitCounter += Time.deltaTime;
        
        // After being idle for a certain amount of time, go to Patrol state
        if (_waitCounter >= WaitTime) StateMachine.ChangeState(_sm.PatrolState);
    }
}
