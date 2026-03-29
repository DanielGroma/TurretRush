public class EnemyStateMachine
{
    private IEnemyState _currentState;

    public void SetState(IEnemyState newState)
    {
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update() => _currentState?.Update();
}