using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState _currentState;

    private void Start()
    {
        _currentState = GetInitialState();
        _currentState?.Enter();
    }

    private void Update()
    {
        _currentState?.UpdateLogic();
    }

    private void LateUpdate()
    {
        _currentState?.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    private void OnGUI()
    {
        var content = _currentState != null ? _currentState.Name : "No current state";
        GUILayout.Label($"<color='white'><size=40>{content}</size></color>");
    }
}
