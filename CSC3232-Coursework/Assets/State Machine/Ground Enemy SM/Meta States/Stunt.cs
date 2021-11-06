using UnityEngine;

public class Stunt : BaseState
{
    private readonly GroundEnemySM _sm;
    private const float WaitTime = 1.5f;
    private float _waitCounter;

    public Stunt(GroundEnemySM stateMachine) : base("Stunt", stateMachine) {
        _sm = stateMachine;
    }
    
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
        if (_waitCounter >= WaitTime) StateMachine.ChangeState(_sm.GetInRangeState);
    }
    
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        
        // Set enemy's animation
        _sm.ChangeAnimationState("Walking_Enemy_Stunt");
    }
}