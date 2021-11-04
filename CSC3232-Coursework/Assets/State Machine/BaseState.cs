public class BaseState
{
    public readonly string Name;
    protected readonly StateMachine StateMachine;

    protected BaseState(string name, StateMachine stateMachine)
    {
        Name = name;
        StateMachine = stateMachine;
    }
    
    public virtual void Enter() {}
    public virtual void UpdateLogic() {}
    public virtual void UpdatePhysics() {}
    public virtual void Exit() {}
}
