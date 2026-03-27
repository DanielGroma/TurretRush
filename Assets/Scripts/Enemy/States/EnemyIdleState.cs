using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private Enemy _enemy;
    private EnemyConfig _enemyConfig;
    private Transform _target;
    private GameStateManager _gameStateManager;

    private bool isChasing = false;

    public EnemyIdleState(Enemy enemy, Transform target, EnemyConfig enemyConfig, GameStateManager gameStateManager)
    {
        _enemy = enemy;
        _target = target;
        _enemyConfig = enemyConfig;
        _gameStateManager = gameStateManager;
    }

    public void Enter()
    {
        _enemy.animationHandler.PlayAnimation("Idle");
    }

    public void Update()
    {
        if (_target == null) return;

        float distance = Vector3.Distance(_enemy.transform.position, _target.position);
        if (distance <= _enemyConfig.activationDistance && !isChasing)
        {
            isChasing = true;
            if (_gameStateManager.CurrentState != GameState.Playing)
                return;
            _enemy.StartChasing();
        }
    }

    public void Exit() 
    {
        _enemy.animationHandler.StopAnimation();
    }
}