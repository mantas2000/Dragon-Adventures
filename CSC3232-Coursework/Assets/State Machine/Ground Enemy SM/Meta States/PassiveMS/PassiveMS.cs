using UnityEngine;

public class PassiveMS : BaseState
{
    protected GroundEnemySM _sm;

    protected PassiveMS(string name, GroundEnemySM stateMachine) : base(name, stateMachine)
    {
        _sm = stateMachine;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        // Calculate linecast end coordinates
        var endPos = _sm.renderer.flipX switch
        {
            true => _sm.castPoint.position + Vector3.right * -_sm.visionDistance,
            false => _sm.castPoint.position + Vector3.right * _sm.visionDistance
        };
        
        // Raycast to check if enemy can see player
        var hit2D = Physics2D.Linecast(_sm.castPoint.position, endPos, 1 << 6);
        
        // If player is found, go to Aggressive Meta state
        if (hit2D.collider != null && hit2D.collider.CompareTag("Player"))
        {
            // Get results from Markov Chart
            var markovAnalysis = MarkovDecisionProcesses.MarkovChartAnalysis();

            // Depending on results from Markov Chart Analysis, go to chosen state
            switch (markovAnalysis)
            {
                case "Shoot":
                    StateMachine.ChangeState(_sm.ShootState);
                    break;
            
                case "Chase":
                    StateMachine.ChangeState(_sm.GetInRangeState);
                    break;
                
                case "GetClose&Shoot":
                    StateMachine.ChangeState(_sm.GetInRangeState);
                    break;
            }
        }
    }
}